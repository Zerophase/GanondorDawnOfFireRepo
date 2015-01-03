using UnityEngine;
using System.Collections;
using System.Linq;

public class MoblinGlobalState : IState<Moblin>
{
    private static MoblinGlobalState instance;
    public static MoblinGlobalState Instance
    {
        get { return instance ?? (instance = new MoblinGlobalState()); }
    }
    public void Enter(Moblin moblin)
    {
        moblin.SubscribeGlobal();
    }

    public void Execute(Moblin moblin)
    {
        if (Input.GetKeyDown(KeyCode.Space) && Picker.Instance.InSphere(moblin.gameObject) && 
            !moblin.Held && moblin.StateMachine.CurrentState != MoblinSummoned.Instance &&
            moblin.Side == Allegiance.ALLY && (Summoned.Instance.HeldUnitType.OfType<Moblin>().Any() ||
            Summoned.Instance.HeldUnitType.Count == 0))
        {
            moblin.PlayParticleSystem();
            moblin.flipUnitRenderer();
        }
        else if(moblin.Held)
        {
            moblin.Pull();
        }

        if(moblin.StateMachine.CurrentState != MoblinSummoned.Instance && moblin.Side == Allegiance.ALLY)
        {
            if(moblin.Held)
            {
                if(moblin.StopParticleSystem())
                {
                    moblin.DispatchSummonMessage();
                    moblin.StateMachine.ChangeState(MoblinSummoned.Instance);
                }  
            }
        }
    }

    public void Exit(Moblin moblin)
    {
        Debug.Log("Exiting Moblin Global State");
    }

    public void OnMessage(Moblin moblin, UnitTelegram telegram)
    {
        //TODO IMplement moblin global message handling if needed
        //Debug.Log(telegram.Receiver + " : has received message from " + telegram.Sender + " in MoblinGlobalState.");
    }
}

public class MoblinPatrol : IState<Moblin>
{
    private static MoblinPatrol instance;
    public static MoblinPatrol Instance
    {
        get { return instance ?? (instance = new MoblinPatrol()); }
    }

    public void Enter(Moblin moblin)
    {
        moblin.Tag();
    }

    public void Execute(Moblin moblin)
    {
        if(!moblin.Held)
            moblin.Patrol();

        if (moblin.Engage())
            moblin.StateMachine.ChangeState(MoblinCombat.Instance);
    }

    public void Exit(Moblin moblin)
    {
        moblin.UnTag();
    }

    public void OnMessage(Moblin moblin, UnitTelegram telegram)
    {
        try
        {
            if (moblin.ID == telegram.Receiver.ID)
            {
                moblin.Defend(telegram.Sender.Strength);
            }
        }
        catch (System.Exception)
        {

            Debug.Log("Whoop");
        }
    }
}

public class MoblinStandStill : IState<Moblin>
{
	private static MoblinStandStill instance;
	public static MoblinStandStill Instance
	{
		get { return instance ?? (instance = new MoblinStandStill()); }
	}
	
	public void Enter(Moblin moblin)
	{
		Debug.Log("Holding one moblin. Implement waht happens");
	}
	
	public void Execute(Moblin moblin)
	{

	}
	
	public void Exit(Moblin moblin)
	{
	}
	
	public void OnMessage(Moblin moblin, UnitTelegram telegram)
	{
        try
        {
            if (moblin.ID == telegram.Receiver.ID)
            {
                moblin.Defend(telegram.Sender.Strength);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
	}
}

public class MoblinSummoned : IState<Moblin>
{
    private static MoblinSummoned instance;
    public static MoblinSummoned Instance
    {
        get { return instance ?? (instance = new MoblinSummoned()); }
    }

    public void Enter(Moblin moblin)
    {
        moblin.transform.position = ThirdPerson_Controller.Instance.transform.position;
        Debug.Log("Moblin: " + moblin.ID + " is in: " + typeof(MoblinSummoned));
    }

    public void Execute(Moblin moblin)
    {
        if (moblin.Held)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                moblin.PlayParticleSystem();
        }
        
        if (!moblin.Held)
        {
            if(moblin.Push())
            {
                if((Levels)Application.loadedLevel == Levels.TUTORIAL)
                    moblin.StateMachine.ChangeState(MoblinStandStill.Instance);
                else
                    moblin.StateMachine.ChangeState(MoblinPatrol.Instance);
            }
        }
    }

    public void Exit(Moblin moblin)
    {
        moblin.DispatchUnSummonMessage();
        moblin.flipUnitRenderer();
    }

    public void OnMessage(Moblin moblin, UnitTelegram telegram)
    {
    }
}

public class MoblinCombat : IState<Moblin>
{
    private static MoblinCombat instance;
    public static MoblinCombat Instance
    {
        get { return instance ?? (instance = new MoblinCombat()); }
    }

    public void Enter(Moblin moblin)
    {
        Debug.Log("Moblin in Comabt: " + moblin.ID + "Moblin side: " + moblin.Side.ToString());
        moblin.Subscribe();
    }

    public void Execute(Moblin moblin)
    {
        moblin.Attack();

        if (!moblin.Engage())
            moblin.StateMachine.ChangeState(MoblinPatrol.Instance);
    }

    public void Exit(Moblin moblin)
    {
        moblin.UnSubscribe();
        moblin.DisEngage();
    }

    public void OnMessage(Moblin moblin, UnitTelegram telegram)
    {
        try
        {
            if (moblin.ID == telegram.Receiver.ID)
            {
                moblin.Defend(telegram.Sender.Strength);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}
