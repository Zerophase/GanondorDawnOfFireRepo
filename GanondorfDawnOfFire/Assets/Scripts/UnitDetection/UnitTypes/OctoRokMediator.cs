 using UnityEngine;
using System.Collections;

public class OctoRokMediator : UnitMediator 
{
    public override void Init(Director director, Unit unit)
    {
        switch (unit.Side)
        {
            case Allegiance.ALLY:
                unpicked = Color.magenta;
                picked = Color.green;
                break;
            case Allegiance.ENEMY:
                unpicked = Color.gray;
                break;
            default:
                break;
        }

        base.Init(director, unit);
    }
}
