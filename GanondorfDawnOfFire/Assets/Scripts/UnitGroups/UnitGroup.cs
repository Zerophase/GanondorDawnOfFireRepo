using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGroup
{
    private List<Unit> units = new List<Unit>();
    
    public Unit Commander { get; private set; }
    
    private SteeringBehaviors steeringBehaviors;
    private PathFinder pathFinder;

    private GameObject playZone;

    public void Start()
    {
        playZone = GameObject.Find("PlayZone");
        //TODO test if playzone igonres raycast
        playZone.layer = 2;
    }
    public void Add(Unit unit)
    {
        if (units.Count == 0)
        {
            Commander = unit;
            //Don't believe a seperate areaaround is needed for commander
            //Commander.set_areaAround();
            unit.IsCommander = true;
            if(unit.IsCommander)
                CreateNewPathFinder(unit);

            steeringBehaviors = new SteeringBehaviors(units);
         }
        
        units.Add(unit);
    }

    private void CreateNewPathFinder(Unit unit)
    {
        pathFinder = new PathFinder();
        //TODO check math
        float forward = 100f;
        float right = 100f;
        float halfHeight = playZone.transform.position.x + (playZone.transform.forward.x * playZone.transform.lossyScale.x) / 2;
        float halfWidth = playZone.transform.position.z + (playZone.transform.right.z * playZone.transform.lossyScale.z) / 2;
        float unitPosRelativeToBox_x = Mathf.Abs((playZone.transform.position.x - unit.transform.position.x) + forward);
        float unitPosRelativeToBox_y = Mathf.Abs((playZone.transform.position.z - unit.transform.position.z) + right);
        while (Mathf.Abs((playZone.transform.position.x - unit.transform.position.x ) + forward) > halfHeight / 3)
            forward -= 10f;
        while (Mathf.Abs((playZone.transform.position.z - unit.transform.position.z) + right) > halfWidth / 3)
            right -= 10f;

        pathFinder.UpdateGraph(0, unit.transform.position);
        pathFinder.UpdateGraph(1, unit.transform.position + unit.transform.forward * forward);
        pathFinder.UpdateGraph(2, unit.transform.position + unit.transform.forward * forward + unit.transform.right * right);
        pathFinder.UpdateGraph(3, unit.transform.position + unit.transform.right * right);

        for (int i = 0; i < 4; i++)
        {
            pathFinder.AddAllNeighborsToNode(i);
        }
    }

    public void Remove(Unit unit)
    {
        
        if (units.Count == 1)
        {
            unit.IsCommander = false;
            units.Clear();
        }
        else if (unit.IsCommander)
        {
            unit.IsCommander = false;
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].ID != unit.ID)
                {
                    units[i].IsCommander = true;
                    Commander = units[i];
                    CreateNewPathFinder(Commander);
                    break;
                }   
            }
        }

        if (units.Count > 0)
        {
            Unit unitToRemove;
            unitToRemove = units.Find(item => item.ID == unit.ID);
            units.Remove(unitToRemove);
        }
    }

    public void ReassignKeys(int key)
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].Key = key;
        }
    }

    public int Count()
    {
        return units.Count;
    }

    public Vector3 Patrol(Unit unit)
    {
        if (unit.IsCommander)
        {
            if (pathFinder.UpdateDistanceTraveled(unit.transform.position) < 30f || !pathFinder.firstSearchDone)
                unit.Target = pathFinder.CreatePathAStarDistanceSquared(unit.transform.position);
        }
            

        if (units.Count > steeringBehaviors.Units.Count)
            steeringBehaviors.Units = units;

        return steeringBehaviors.PriotizedDithering(unit);
    }

    public bool HasUnit(Unit unit)
    {
        if (units.Contains(unit))
            return true;
        else
            return false;
    }
}
