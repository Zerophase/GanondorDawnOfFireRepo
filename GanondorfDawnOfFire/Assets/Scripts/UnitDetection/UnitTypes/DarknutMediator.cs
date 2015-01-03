using UnityEngine;
using System.Collections;

public class DarknutMediator : UnitMediator 
{
    public override void Init(Director director, Unit unit)
    {
        unpicked = Color.yellow;
        picked = Color.magenta;

        base.Init(director, unit);
    }
}
