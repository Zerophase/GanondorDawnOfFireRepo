using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Octorok : Unit, IObserver
{
    private StateMachine<Octorok> stateMachine;
    public StateMachine<Octorok> StateMachine { get { return stateMachine; } }

    private RockPooler rockPooler = new RockPooler();

    private List<GameObject> rocks = new List<GameObject>();

    private FuzzyModule fuzzyModule;
    float lastDesirabilityScore;
	// Use this for initialization
	protected override void Start () 
    {
        MaxSpeed = 12;
        MaxForce = 12;
        mass = 1;

        health = 25;
        strength = 10;
        physicalDefense = 1;

        resummon_y = 1.4f;

        unitMediator = gameObject.GetComponent<OctoRokMediator>();
        mediatorTelegram = new MediatorTelegram(unitMediator, HandledBy.LOCAL);

        particleSystem = gameObject.GetComponent<ParticleSystem>();
        base.Start();

        stateMachine = new StateMachine<Octorok>(this);
        stateMachine.ChangeState(OctorokPatrol.Instance);
        StateMachine.GlobalState = OctorokGlobalState.Instance;

        SubscribeGlobal();

		rockPooler.Start(this);

        fuzzyModule = new FuzzyModule();

        FuzzyVariable distToTarget = fuzzyModule.CreateFLV("DistToTarget");

        FzSet Target_Close = distToTarget.AddLeftShoulderSet("Target_Close", 0f, 1f, 2f);
        FzSet Target_Medium = distToTarget.AddTriangularSet("Target_Medium", 2f, 3f, 4f);
        FzSet Target_Far = distToTarget.AddRightSHoulderSet("Target_Far", 4f, 5f, 6f);

        FuzzyVariable desirability = fuzzyModule.CreateFLV("Desirability");

        FzSet Undesirable = desirability.AddLeftShoulderSet("Undesirable", 0f, 25f, 50f);
        FzSet Desirable = desirability.AddTriangularSet("Desirable", 25f, 50f, 100f);
        FzSet VeryDesirable = desirability.AddRightSHoulderSet("VeryDesirable", 50f, 100f, 150f);


        FuzzyVariable timeTillNextShot = fuzzyModule.CreateFLV("timeTillNextShot");

        FzSet Time_Low = timeTillNextShot.AddLeftShoulderSet("Time_Low", 0f, .1f, .2f);
        FzSet Time_Mid = timeTillNextShot.AddTriangularSet("Time_Mid", .1f, .2f, .5f);
        FzSet Time_High = timeTillNextShot.AddRightSHoulderSet("Time_High", .2f, .5f, 1f);

        FuzzyVariable desirabilityShot = fuzzyModule.CreateFLV("DesirabilityShot");

        FzSet UndesirableShot = desirability.AddRightSHoulderSet("UndesirableShot", .2f, .5f, 1f);
        FzSet DesirableShot = desirability.AddTriangularSet("DesirableShot", .1f, .3f, .5f);
        FzSet VeryDesirableShot = desirability.AddLeftShoulderSet("VeryDesirableShot", 0f, .1f, .2f);

        fuzzyModule.AddRule(Target_Close, Undesirable);
        fuzzyModule.AddRule(Target_Medium, Desirable);
        fuzzyModule.AddRule(Target_Far, VeryDesirable);

        fuzzyModule.AddRule(Time_Low, VeryDesirableShot);
        fuzzyModule.AddRule(Time_Mid, DesirableShot);
        fuzzyModule.AddRule(Time_High, UndesirableShot);
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        if(stateMachine.CurrentState != OctorokSummoned.Instance)
            base.Update();

        stateMachine.Update();

        for (int i = 0; i < rocks.Count; i++)
        {
            float test = Vector3.Distance(rocks[i].transform.position, gameObject.transform.position);
            if (Vector3.Distance(rocks[i].transform.position, gameObject.transform.position) > 10f)
            {
                rockPooler.DeactivatePooledRock(rocks[i]);
                rocks.RemoveAt(i);
            }
        }
	}

    public void HandleMessage(Telegram telegram)
    {
        if (telegram is UnitTelegram)
        {
            UnitTelegram unitTelegram = telegram as UnitTelegram;
            if (unitTelegram.Receiver.ID == this.ID)
                stateMachine.HandleMessage(unitTelegram);
        }
    }

    public void HandleGlobalMessage(Telegram telegram)
    {
        if (telegram is UnitTelegram && !(telegram is RockTelegram))
        {
            UnitTelegram unitTelegram = telegram as UnitTelegram;
            if (unitTelegram.Receiver.ID == this.ID)
                stateMachine.HandleGlobalMessage(unitTelegram);
        }
        else if(telegram is RockTelegram)
        {
            RockTelegram rockTelegram = telegram as RockTelegram;
            if (rockTelegram.Receiver.ID == this.ID)
                stateMachine.HandleGlobalMessage(rockTelegram);
        }
    }

    public void Subscribe()
    {
        MessageDispatcher.Instance.SendMessage += new MessageDispatcher.SendMessageHandler(HandleMessage);
    }

    public void UnSubscribe()
    {
        MessageDispatcher.Instance.SendMessage -= new MessageDispatcher.SendMessageHandler(HandleMessage);
    }

    public void SubscribeGlobal()
    {
        MessageDispatcher.Instance.GlobalMessage += new MessageDispatcher.SendMessageHandler(HandleGlobalMessage);
    }

    public void UnSubscribeGlobal()
    {
        MessageDispatcher.Instance.GlobalMessage -= new MessageDispatcher.SendMessageHandler(HandleGlobalMessage);
    }

    

    public override void Attack()
    {
        base.Attack();

        if (unitTargeted != null)
        {
            if (attackTimer >= 0f)
                attackTimer -= Time.deltaTime;

            Incombat = true;
            Target = unitTargeted.transform.position;
            steeringForce = individualUnitSteering.PriotizedDithering(this);

            if (attackTimer <= 0f && Retreat(attackTimer))
            {
                rocks.Add(rockPooler.GetPooledRocks());
                rocks.Last<GameObject>().transform.position = (gameObject.transform.position +
                    gameObject.transform.forward);
                rocks.Last<GameObject>().transform.rotation = gameObject.transform.rotation;

                attackTimer = 0.5f;
            }
            else
                Target = unitTargeted.transform.position * -1;
        }
    }

    private bool Retreat(float time)
    {
        float distToTarget = Vector3.Distance(unitMediator.transform.position, Target);

        float score = GetDesirabilityDist(distToTarget);
        score += GetDesirabilityTime(time);

        if (score > .25f)
        {
            return true;
        }
        else
            return false;
    }

    public float GetDesirabilityTime(float shotTime)
    {
        fuzzyModule.Fuzzify("timeTillNextShot", shotTime);
        lastDesirabilityScore = fuzzyModule.DeFuzzify("DesirabilityShot", FuzzyModule.DefuzzifyMethod.max_av);
        return lastDesirabilityScore;
    }

    public float GetDesirabilityDist(float distToTarget)
    {
        fuzzyModule.Fuzzify("DistToTarget", distToTarget);
        lastDesirabilityScore = fuzzyModule.DeFuzzify("Desirability", FuzzyModule.DefuzzifyMethod.max_av);
        return lastDesirabilityScore;
    }

    public void RockHit(RockTelegram rockTelegram)
    {
        Debug.Log("Rock Hit");
        unitTelegram = new UnitTelegram(this, rockTelegram.Sender, HandledBy.LOCAL);
        MessageDispatcher.Instance.DispatchMessage(unitTelegram);
        rockPooler.DeactivatePooledRock(rockTelegram.RockCollidedWIth.gameObject);
        rocks.Remove(rockTelegram.RockCollidedWIth.gameObject);
    }

    protected override void grabMechanic(int flip)
    {
        if (particleSystem.isPlaying)
        {
            if (stateMachine.CurrentState != MoblinSummoned.Instance)
            {
                particleSystem.transform.rotation = Quaternion.LookRotation(
                (ThirdPerson_Controller.Instance.transform.position - gameObject.transform.position));
            }
            else
                particleSystem.transform.rotation = Quaternion.LookRotation(
                    ThirdPerson_Controller.Instance.transform.forward);
            base.grabMechanic(flip);
        }
    }
}
