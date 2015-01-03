using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teams 
{
    private Dictionary<Allegiance, List<BaseUnit>> team = new Dictionary<Allegiance, List<BaseUnit>>();
    public Dictionary<Allegiance, List<BaseUnit>> Team { get { return team; } }

    private int allyCount;
    private int enemyCount;

    private static Teams instance;
    public static Teams Instance
    {
        get { return instance ?? (instance = new Teams()); }
    }

    public void Add(BaseUnit baseUnit)
    {
        if (!team.ContainsKey(baseUnit.Side))
        {
            team.Add(baseUnit.Side, new List<BaseUnit>());
            team[baseUnit.Side].Add(baseUnit);
        }   
        else
            team[baseUnit.Side].Add(baseUnit);

        addToCount(baseUnit);
    }
    
    private void addToCount(BaseUnit baseUnit)
    {
        switch (baseUnit.Side)
        {
            case Allegiance.ALLY:
                allyCount++;
                break;
            case Allegiance.ENEMY:
                enemyCount++;
                break;
            default:
                break;
        }
    }

    public void Remove(BaseUnit unit)
    {
        if (team.ContainsKey(unit.Side))
        {
            team[unit.Side].Remove(unit);
        }
    }

    public void Remove(Unit unit)
    {
        if (team.ContainsKey(unit.Side))
        {
            team[unit.Side].Remove(unit);
        }
    }

    public void Clear()
    {
        team.Clear();
    }

    public void unitsLeft()
    {
        int allies = allyCount - team[Allegiance.ALLY].Count;
        int enemies = enemyCount - team[Allegiance.ENEMY].Count;

        MessageDispatcher.Instance.DispatchMessage(new InterfaceTelegram(allies.ToString(), TargetText.Ally, HandledBy.LOCAL));
        MessageDispatcher.Instance.DispatchMessage(new InterfaceTelegram(enemies.ToString(), TargetText.Enemy, HandledBy.LOCAL));
    }

    public BaseUnit Find(Unit unit, Allegiance allegiance)
    {
        for (int i = 0; i < team[allegiance].Count; i++)
		{
            if (unit.InRadius(team[allegiance][i]))
                return team[allegiance][i];
		}

        return null;
    }
}
