using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//If flocking breaks this is the culprit
public class UnitGroupManager
{
    private Dictionary<int, UnitGroup> unitGrouping = new Dictionary<int, UnitGroup>();
    public Vector3 UnitGrouping(int key)
    {
        return unitGrouping[key].Commander.transform.position;
    }
    private static UnitGroupManager instance;
    public static UnitGroupManager Instance
    {
        get { return instance ?? (instance = new UnitGroupManager()); }
    }

    private static int key;
    public static void ResetKey()
    {
        key = 0;
    }

    public void Reset()
    {
        unitGrouping.Clear();
    }

    private  const int firstValue =  0;

    public void Add(Unit unit)
    {
        if(unitGrouping.Count == 0)
        {
            unitGrouping.Add(key, new UnitGroup());
            unitGrouping[key].Start();
            unitGrouping[key].Add(unit);
            unit.Key = key++;
            return;
        }

        for (int i = 0; i <= unitGrouping.Count; i++)
        {
            try
            {
                if (i == unitGrouping.Count)
                {
                    unitGrouping.Add(key, new UnitGroup());
                    unitGrouping[key].Start();
                    unitGrouping[key].Add(unit);
                    unit.Key = key++;
                    return;
                }
                else if (unitGrouping[i].Commander.InRadius(unit) && 
                    unitGrouping[i].Commander.GetType() == unit.GetType())
                {
                    unitGrouping[i].Add(unit);
                    unit.Key = unitGrouping[i].Commander.Key;
                    return;
                }
            }
            catch (System.NullReferenceException)
            {
                Debug.Log("Null reference exception on unit with ID " + unit.ID);
                throw;
            }
           
        }
    }

    public void Remove(Unit unit)
    {
        UnitGroup ug;
        if (!unitGrouping.TryGetValue(unit.Key, out ug) || !unitGrouping[unit.Key].HasUnit(unit))
            return;
        // TODo Removes unit
        unitGrouping[unit.Key].Remove(unit);
        if (unitGrouping[unit.Key].Count() == 0)
        {
            // TODO if key spot is empty shifts elements from other keys to its place.
            UnitGroup uGroup;
            for (int i = unit.Key; i < unitGrouping.Count - 1; i++)
            {
                unitGrouping[i + 1].ReassignKeys(i);
                uGroup = unitGrouping[i + 1];
                unitGrouping[i] = uGroup;
            }

            unitGrouping.Remove(unitGrouping.Count - 1);
            key--;
        }

        unit.Key = -1;
    }

    public Vector3 Patrol(Unit unit)
    {
        try
        {
            return unitGrouping[unit.Key].Patrol(unit);
        }
        catch (KeyNotFoundException e)
        {

            Debug.Log("Key: " + unit.Key + "Unit: " + unit.GetType());
            throw;
        }
       
    }

    public void PrintUnitCount()
    {
        Debug.Log("UnitGroupManager has " + unitGrouping.Count + " in unitGrouping.");
    }
}
