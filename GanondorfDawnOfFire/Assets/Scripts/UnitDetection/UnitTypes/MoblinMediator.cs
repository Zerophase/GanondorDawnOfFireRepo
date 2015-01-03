using UnityEngine;
using System.Collections;

public class MoblinMediator : UnitMediator 
{
    public override void Init(Director director, Unit unit)
    {
        switch (unit.Side)
        {
            case Allegiance.ALLY:
                unpicked = Color.blue;
                picked = Color.red;
                break;
            case Allegiance.ENEMY:
                unpicked = Color.yellow;
                break;
            default:
                break;
        }
        
        base.Init(director, unit);
    }
}
