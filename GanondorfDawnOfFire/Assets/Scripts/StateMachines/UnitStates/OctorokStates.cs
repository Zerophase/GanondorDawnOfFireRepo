using UnityEngine;
using System.Collections;
using System.Linq;

public class OctorokGlobalState : IState<Octorok> 
{
    private static OctorokGlobalState instance;
    public static OctorokGlobalState Instance
    {
        get { return instance ?? (instance = new OctorokGlobalState()); }
    }

    public void Enter(Octorok octorok)
    {
        octorok.SubscribeGlobal();
    }

    public void Execute(Octorok octorok)
    {
        if (Input.GetKeyDown(KeyCode.Space) && Picker.Instance.InSphere(octorok.gameObject) &&
            !octorok.Held && octorok.StateMachine.CurrentState != OctorokSummoned.Instance &&
            octorok.Side == Allegiance.ALLY && (Summoned.Instance.HeldUnitType.OfType<Octorok>().Any() ||
            Summoned.Instance.HeldUnitType.Count == 0))
        {
            octorok.PlayParticleSystem();
            octorok.flipUnitRenderer();
        }
        else if (octorok.Held)
        {
            octorok.Pull();
        }

        if (octorok.StateMachine.CurrentState != OctorokSummoned.Instance && octorok.Side == Allegiance.ALLY)
        {
            if (octorok.Held)
            {
                if(octorok.StopParticleSystem())
                {
                    octorok.DispatchSummonMessage();
                    octorok.StateMachine.ChangeState(OctorokSummoned.Instance);
                }
            }
        }
    }

    public void Exit(Octorok octorok)
    {
        Debug.Log("Exiting Octorok Global State");
    }

    public void OnMessage(Octorok octorok, UnitTelegram telegram)
    {
        if (telegram is RockTelegram)
        {
            OnMessage(octorok, telegram as RockTelegram);
        }
    }

    public void OnMessage(Octorok octorok, RockTelegram rockTelegram)
    {
        octorok.RockHit(rockTelegram);
    }
}

public class OctorokSummoned : IState<Octorok>
{
    private static OctorokSummoned instance;
    public static OctorokSummoned Instance
    {
        get { return instance ?? (instance = new OctorokSummoned()); }
    }

    public void Enter(Octorok octorok)
    {
        octorok.transform.position = ThirdPerson_Controller.Instance.transform.position;
    }

    public void Execute(Octorok octorok)
    {
        if (octorok.Held)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                octorok.PlayParticleSystem();
        }

        if (!octorok.Held)
        {
            if (octorok.Push())
                octorok.StateMachine.ChangeState(OctorokPatrol.Instance);
        }
    }

    public void Exit(Octorok octorok)
    {
        octorok.DispatchUnSummonMessage();
        octorok.flipUnitRenderer();
    }

    public void OnMessage(Octorok octorok, UnitTelegram telegram)
    {
        throw new System.NotImplementedException();
    }
}

public class OctorokPatrol : IState<Octorok>
{
    private static OctorokPatrol instance;
    public static OctorokPatrol Instance
    {
        get { return instance ?? (instance = new OctorokPatrol()); }
    }

    public void Enter(Octorok octorok)
    {
        octorok.Tag();
    }

    public void Execute(Octorok octorok)
    {
        try
        {
            octorok.Patrol();
        }
        catch (System.NullReferenceException)
        {

            Debug.Log("What's up");
        }
        

        if (octorok.Engage())
            octorok.StateMachine.ChangeState(OctorokCombat.Instance);
    }

    public void Exit(Octorok octorok)
    {
        octorok.UnTag();
    }

    public void OnMessage(Octorok octorok, UnitTelegram telegram)
    {
        Debug.Log(telegram.Receiver + " : has received message from " + telegram.Sender + " in OctorokPatrol.");
    }
}

public class OctorokStandStill : IState<Octorok>
{
    private static OctorokStandStill instance;
    public static OctorokStandStill Instance
    {
        get { return instance ?? (instance = new OctorokStandStill()); }
    }

    public void Enter(Octorok octorok)
    {
        Debug.Log("Holding one Octorok implement what happens.");
    }

    public void Execute(Octorok octorok)
    {
    }

    public void Exit(Octorok octorok)
    {
    }

    public void OnMessage(Octorok octorok, UnitTelegram telegram)
    {
       
    }
}

public class OctorokCombat : IState<Octorok>
{
    private static OctorokCombat instance;
    public static OctorokCombat Instance
    {
        get { return instance ?? (instance = new OctorokCombat()); }
    }

    public void Enter(Octorok octorok)
    {
        Debug.Log("Octorok in Comabt: " + octorok.ID + "Octorok side: " + octorok.Side.ToString());
        octorok.Subscribe();
    }

    public void Execute(Octorok octorok)
    {
        octorok.Attack();

        if (!octorok.Engage())
            octorok.StateMachine.ChangeState(OctorokPatrol.Instance);
    }

    public void Exit(Octorok octorok)
    {
        octorok.UnSubscribe();
        octorok.DisEngage();
    }

    public void OnMessage(Octorok octorok, UnitTelegram telegram)
    {
        if (octorok.ID == telegram.Receiver.ID)
        {
            octorok.Defend(telegram.Sender.Strength);
        }
    }
}