using UnityEngine;
using System.Collections;

public class LimitUnitMovement : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player" && other.name != "DamageArea"
		    && other.name != "AreaAround")
		{
			other.GetComponent<Unit>().StopMovement();
			Debug.Log("Stop At wall");
		}
	}
}
