using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public enum Allegiance {NULL, ALLY, ENEMY};

public class Unit : BaseUnit, IResetable
{
    public int ID { get; protected set; }
    
    [HideInInspector]
    public int Key;

    protected static int id = 0;
    public static void ResetID()
    {
        id = 0;
    }

    public float MaxSpeed { get; protected set; }
    public float MaxForce { get; protected set; }
    public Vector3 Velocity { get; protected set; }
    protected Vector3 previousPos;
    public Vector3 Target { get;  set; }

    protected Vector3 steeringForce;
    protected Vector3 acceleration;

    protected float mass;
    public bool IsCommander { get; set; }
    SphereCollider areaAround;

    public Particle SummonParticle;

    protected SteeringBehaviors individualUnitSteering;

    public bool Held {get; protected set; }
    public bool Incombat { get; protected set; }

    protected int strength;
    public int Strength { get { return strength; } }
    
    private List<Vector3> patrolPoints = new List<Vector3>();

    protected UnitMediator unitMediator;
    protected MediatorTelegram mediatorTelegram;

    protected ParticleSystem particleSystem;
    protected float speedParticleSystem = 6f;

    protected enum Grab { GRAB = -1, THROW = 1 };

    protected float timerPull;
    protected float keyPressTimer;

    protected float attackTimer = 0f;

    protected UnitTelegram unitTelegram;
    protected PlayerTelegram playerTelegram;
    protected BaseUnit unitTargeted;

    private DamageArea damageArea;

    private GameObject model;

    protected float resummon_y;

    public float Radius = 15f;

    public void set_areaAround()
    {
        GameObject g = new GameObject();
        g.transform.parent = gameObject.transform;
        g.name = "AreaAround";
        areaAround = g.AddComponent<SphereCollider>();
        areaAround.isTrigger = true;
        areaAround.radius = Radius / transform.localScale.z;
        areaAround.transform.position = gameObject.transform.position;
        LayerMask ignoreLayer = Physics.IgnoreRaycastLayer >> 1;
        areaAround.gameObject.layer = ignoreLayer;
    }

    protected override void Start()
    {
        ID = id++;

        //TODO Make sure Parent Object of primitives is centered on Unit
        set_areaAround();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if(gameObject.transform.GetChild(i).name == "Model")
            {
                model = gameObject.transform.GetChild(i).gameObject;
            }
        }

        timerPull = 0;
        keyPressTimer = 0;

        Key = -1;

        //TODO check if works
        Velocity = Vector3.zero;
        previousPos = Vector3.zero;

        individualUnitSteering = new SteeringBehaviors();

        IsCommander = false;
        Incombat = false;

        if (Levels.VILLAGELEVEL == (Levels)Application.loadedLevel)
            Teams.Instance.Add(this);
        
        damageArea = GetComponentInChildren<DamageArea>();

        switch (Side)
        {
            case Allegiance.ALLY:
                enemy = Allegiance.ENEMY;
                break;
            case Allegiance.ENEMY:
                enemy = Allegiance.ALLY;
                break;
            default:
                break;
        }
    }

    protected override void Update()
    {
		acceleration = steeringForce; /// mass; //Mass is alway 1 we can avoid the division

        Velocity += acceleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);
		Velocity.Set(Velocity.x, 0f, Velocity.z);

        transform.rotation = Quaternion.LookRotation(Velocity, Vector3.up);
 
		if (Velocity.y > 0 || Velocity.y < 0)
			Velocity = new Vector3(Velocity.x, 0f, Velocity.z);
        transform.position += Velocity * Time.deltaTime;

        if (IsCommander)
        {
        }
        else if (Key > -1)
        {
            try 
	        {
                Target = UnitGroupManager.Instance.UnitGrouping(Key);
	        }
	        catch (System.Exception)
	        {
		
		        throw;
	        }
        }
        else if(!Incombat)
            Target = transform.forward * 10;

        Debug.DrawLine(transform.position, Target, Color.black);
    }

    public void Tag()
    {
        UnitGroupManager.Instance.Add(this);
        Debug.Log("Tag Units Method start up Count: " + ID);
    }

    public void UnTag()
    {
        UnitGroupManager.Instance.Remove(this);
    }

    public void Patrol()
    {
        steeringForce = UnitGroupManager.Instance.Patrol(this);
    }

    public bool Engage()
    {
        if (Teams.Instance.Find(this, enemy))
            return true;
        else
            return false;
    }

    public void DisEngage()
    {
        this.Incombat = false;
    }

    protected void calculateVelocity()
    {
        Velocity = (transform.position - previousPos) / Time.deltaTime;
        previousPos = transform.position;
    }

    //TODO Add attackSpeed variable
    public virtual void Attack()
    {
        unitTargeted = Teams.Instance.Find(this, enemy);
    }

    public void Defend(int damage)
    {
        if (damageArea.Hit)
        {
            if (physicalDefense >= damage)
                health -= 1;
            else
                health -= damage - physicalDefense;
        }
        
        Die();
    }

    protected void Die()
    {
        if (health <= 0)
        {
            MessageDispatcher.Instance.DispatchMessage(mediatorTelegram);
            Teams.Instance.Remove(this);
            UnitGroupManager.Instance.Remove(this);
            Destroy(gameObject);
        }  
    }

    public void Pull()
    {
        //TODO check unit type here for grab?
        grabMechanic((int)Grab.GRAB);
    }

    public bool Push()
    {
        timerPull += Time.deltaTime;
        if (timerPull > 2.0f)
        {
            particleSystem.Stop();
			particleSystem.Clear();
            transform.position = new Vector3(transform.position.x, resummon_y, transform.position.z);
            Debug.Log("Moblin Position: " + transform.position);
            Debug.Log("Player Position: " + ThirdPerson_Controller.Instance.transform.position);
            timerPull = 0.0f;
            return true;
        }

        grabMechanic((int)Grab.THROW);
        return false;
    }

    protected virtual void grabMechanic(int flip)
    {
        Vector3 direction = new Vector3(ThirdPerson_Camera.Instance.transform.forward.x, 0f,
			ThirdPerson_Camera.Instance.transform.forward.z) * flip;
        gameObject.transform.position += (direction.normalized * speedParticleSystem) * Time.deltaTime;
    }

    float stopParticleSystemTimer = 0f;
    public bool StopParticleSystem()
    {
        if (stopParticleSystemTimer >= 2.0f)
        {
            if (particleSystem.isPlaying)
            {
                Debug.Log("Particle system has stopped");
                particleSystem.Stop();
	            particleSystem.Clear();
                gameObject.transform.position = ThirdPerson_Controller.Instance.transform.position;
            }
            stopParticleSystemTimer = 0f;
            return true;
        }
        else
        {
            stopParticleSystemTimer += Time.deltaTime;
            return false;
        }
    }

    private bool V3Equal(Vector3 a, Vector3 b)
    {
        if (System.Math.Round(a.x, 1) == System.Math.Round(b.x, 1) &&
                System.Math.Round(a.z, 1) == System.Math.Round(b.z, 1))
        {
            return true;
        }
        else
            return false;
    }

    public void DispatchSummonMessage()
    {
        MessageDispatcher.Instance.DispatchMessage(new SummonTelegram(this, MessagePurpose.ADD, HandledBy.LOCAL));
    }

    public void DispatchUnSummonMessage()
    {
        MessageDispatcher.Instance.DispatchMessage(new SummonTelegram(this, MessagePurpose.REMOVE, HandledBy.LOCAL));
    }

    public void PlayParticleSystem()
    {
        if (!particleSystem.isPlaying)
        {
            if (Held)
                gameObject.transform.position = ThirdPerson_Controller.Instance.transform.position;
            particleSystem.Play();
            Held = !Held;
        }
    }

    public void flipUnitRenderer()
    {
        for (int i = 0; i < model.transform.childCount; i++)
		{
            model.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = !model.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled;
		}
        gameObject.GetComponent<BoxCollider>().enabled = !gameObject.GetComponent<BoxCollider>().enabled;
        
    }

    public bool InRadius(Unit unit)
    {
        Transform goTransform = unit.transform;
        Transform areaAroundTransform = areaAround.transform;

        if(Mathf.Pow((goTransform.position.x - areaAroundTransform.position.x), 2) + 
            Mathf.Pow((goTransform.position.y - areaAroundTransform.position.y), 2) + 
            Mathf.Pow((goTransform.position.z - areaAroundTransform.position.z), 2) < Mathf.Pow(areaAround.radius, 2))
        {
            return true;
        }
        else
            return false;
    }

    public bool InRadius(BaseUnit unit)
    {
        Transform goTransform = unit.transform;
        Transform areaAroundTransform = areaAround.transform;

        if (Mathf.Pow((goTransform.position.x - areaAroundTransform.position.x), 2) +
            Mathf.Pow((goTransform.position.y - areaAroundTransform.position.y), 2) +
            Mathf.Pow((goTransform.position.z - areaAroundTransform.position.z), 2) < Mathf.Pow(areaAround.radius, 2))
        {
            return true;
        }
        else
            return false;
    }

    public void Reset()
    {
        
    }
}
