using UnityEngine;
using System.Collections;

public interface IState<T> where T : Unit 
{
    void Enter(T t);
    void Execute(T t);
    void Exit(T t);
    void OnMessage(T t, UnitTelegram telegram);
}
