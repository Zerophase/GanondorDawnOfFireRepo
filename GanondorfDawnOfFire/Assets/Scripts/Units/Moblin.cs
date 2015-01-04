using UnityEngine;
using System.Collections;

public class Moblin : Unit, IObserver
{
    //Each Unit has their own StateMachine
    private StateMachine<Moblin> stateMachine;
    public StateMachine<Moblin> StateMachine { get { return stateMachine; } }

    private GameObject spearObj;
    private Spear spear;

    protected override void Start()
    {
        unitMediator = gameObject.GetComponent<MoblinMediator>();
        mediatorTelegram = new MediatorTelegram(unitMediator, HandledBy.LOCAL);
        
		particleSystem = gameObject.GetComponent<ParticleSystem>();
        base.Start();

        switch (Side)
        {
            case Allegiance.ALLY:
                MaxSpeed = 12;
                MaxForce = 12;
                mass = 1;
                health = 50;
                strength = 5;
                physicalDefense = 5;
                resummon_y = 2.0f;
                break;
            case Allegiance.ENEMY:
                MaxSpeed = 10;
                MaxForce = 10;
                mass = 1;
                resummon_y = 2.5f;
                strength = 7;
                physicalDefense = 8;
                health = 75;
                break;
            default:
                break;
        }

        SubscribeGlobal();

        stateMachine = new StateMachine<Moblin>(this);

        switch ((Levels)Application.loadedLevel)
        {
            case Levels.TUTORIAL:
                stateMachine.ChangeState(MoblinStandStill.Instance);
                break;
            case Levels.VILLAGELEVEL:
                stateMachine.ChangeState(MoblinPatrol.Instance);
                break;
            default:
                break;
        }
        
        stateMachine.GlobalState = MoblinGlobalState.Instance;
    }

    protected override void Update()
    {
        if (stateMachine.CurrentState != MoblinSummoned.Instance)
            base.Update();
        
        stateMachine.Update();
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

    //Sends message to unit being attacked.
    public override void Attack()
    {
        base.Attack();

        // if targ != null sends message to targets out of range add InRadius here
        // Teams.Instance.Find(this, enemy) already checks for InRadius so shouldn't
        // break
        if (unitTargeted != null)
        {
			if (attackTimer >= 0f)
				attackTimer -= Time.deltaTime;

            Incombat = true;
            Target = unitTargeted.transform.position;
            steeringForce = individualUnitSteering.PriotizedDithering(this);

            if(attackTimer <= 0f)
            {
				attackTimer = 1f;
                if (unitTargeted is Player)
                {
                    playerTelegram = new PlayerTelegram(this, (Player)unitTargeted,
                        HandledBy.LOCAL);
                    MessageDispatcher.Instance.DispatchMessage(playerTelegram);
                }
                else
                {
                    unitTelegram = new UnitTelegram(this, (Unit)unitTargeted, HandledBy.LOCAL);
                    MessageDispatcher.Instance.DispatchMessage(unitTelegram);
                }
            }
        }
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
            {
	            Vector3 roation = new Vector3(ThirdPerson_Camera.Instance.transform.forward.x,
		            0f, ThirdPerson_Camera.Instance.transform.forward.z);
	            particleSystem.transform.rotation = Quaternion.LookRotation(roation);
            }
            base.grabMechanic(flip);
        }
    }
    //TODO Remove if Octorok doesn't need this.
    //public bool KeyPressTimer()
    //{
    //    if(keyPressTimer > 0.5f)
    //    {
    //        keyPressTimer = 0.0f;
    //        return true;
    //    }
    //    else
    //    {
    //        keyPressTimer += Time.deltaTime;
    //        return false;
    //    }
            
    //}
}
