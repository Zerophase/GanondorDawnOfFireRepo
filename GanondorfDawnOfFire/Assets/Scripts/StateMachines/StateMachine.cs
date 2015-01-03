using UnityEngine;
using System.Collections;

public class StateMachine<T> where T : Unit
{
    private T owner;

    private IState<T> currentState;
    private IState<T> previousState;
    private IState<T> globalState;

    public IState<T> CurrentState { get { return currentState; } set { currentState = value; } }
    public IState<T> PreviousState { set { previousState = value; } }
    public IState<T> GlobalState 
    { 
        set 
        { 
            globalState = value;
            globalState.Enter(owner);
        } 
    }

    public StateMachine(T t)
    {
        owner = t;

        currentState = null;
        previousState = null;
        globalState = null;
    }

    public void Update ()
    {

        if (globalState != null)
            globalState.Execute(owner);

        if (currentState != null)
            currentState.Execute(owner);
    }

    public void HandleMessage(UnitTelegram telegram)
    {
        // TODO figure out how to only send to appropriate HandleMessage
        // This is a quick fix to avoid the message being passed further along.
        //
        currentState.OnMessage(owner, telegram);
    }

    public void HandleGlobalMessage(UnitTelegram telegram)
    {
        globalState.OnMessage(owner, telegram);
    }

    public void HandleGlobalMessage(RockTelegram rockTelegram)
    {
        globalState.OnMessage(owner, rockTelegram as RockTelegram);
    }

    public void ChangeState(IState<T> newState)
    {
        if(currentState != null)
        {
            previousState = currentState;
            currentState.Exit(owner);
        }
            
        currentState = newState;
        currentState.Enter(owner);
    }
}
