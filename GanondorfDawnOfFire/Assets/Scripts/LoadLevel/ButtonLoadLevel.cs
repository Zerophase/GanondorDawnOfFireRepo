using UnityEngine;
using System.Collections;

public enum Levels { FRONTEND = 0, VIDEO, TUTORIAL,  VILLAGELEVEL, AFTERACTION};
public class ButtonLoadLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
    {
		Application.LoadLevel((int)Levels.FRONTEND);
	}

	void OnTriggerEnter(Collider collider){
		Debug.Log("Enter");
		Application.LoadLevel("afterAction");

	}
}
