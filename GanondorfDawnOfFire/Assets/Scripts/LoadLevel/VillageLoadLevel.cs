using UnityEngine;
using System.Collections;

public class VillageLoadLevel : MonoBehaviour 
{
	void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            UnitGroupManager.ResetKey();
            UnitGroupManager.Instance.Reset();
            Unit.ResetID();
            Application.LoadLevel((int)Levels.VILLAGELEVEL);
        }
	}
}
