using UnityEngine;
using System.Collections;

public interface IObserver 
{
    void Subscribe();
    void UnSubscribe();
    void SubscribeGlobal();
    void UnSubscribeGlobal();
}
