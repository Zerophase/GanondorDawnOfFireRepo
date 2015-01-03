using UnityEngine;
using System.Collections;

public abstract class BaseUnit : MonoBehaviour
{
    protected int health;
    protected int physicalDefense;

    public Allegiance Side;
    protected Allegiance enemy;


    protected virtual void Start()
    {
       
    }

    protected virtual void Update()
    {

    }
}
