using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RockPooler
{
    List<GameObject> rocks = new List<GameObject>();

    public int PooledAmount = 100;

    private string clone = "(Clone)";

    Unit unit;

	public void Start (Unit unit) 
    {

        //TODO Add parent position in for where to instantiate
        for (int i = 0; i < PooledAmount; i++)
        {
            AddRock();
            rocks[i].GetComponent<Rock>().owner = this.unit = unit;
            rocks[i].SetActive(false);
        }

        removeClone();
	}

    private void removeClone()
    {
        foreach (GameObject rock in rocks)
        {
            rock.name = rock.name.Trim(clone.ToCharArray());
        }
    }

    private void AddRock()
    {
        rocks.Add(GameObject.Instantiate(Resources.Load<GameObject>("Prefab/Weapon/Rock")) as GameObject);
    }

    public GameObject GetPooledRocks()
    {
        for (int i = 0; i < rocks.Count; i++)
        {
            if (!rocks[i].activeInHierarchy)
            {
                rocks[i].SetActive(true);
                return rocks[i];
            }
        }

        if (rocks[rocks.Count - 1].activeInHierarchy)
        {
            AddRock();
            removeClone(rocks.Count - 1);
            rocks[rocks.Count - 1].SetActive(true);
            rocks[rocks.Count - 1].GetComponent<Rock>().owner = this.unit;
            return rocks[rocks.Count - 1];
        }

        return null;
    }

    private void removeClone(int i)
    {
        rocks.ElementAt<GameObject>(i).name = rocks.ElementAt<GameObject>(i).name.Trim(clone.ToCharArray());
    }

    public void DeactivatePooledRock(GameObject rock)
    {
        rocks.Find(item => item == rock).SetActive(false);
    }
}
