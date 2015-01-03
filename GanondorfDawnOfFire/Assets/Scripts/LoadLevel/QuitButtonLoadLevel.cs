using UnityEngine;
using System.Collections;

public class QuitButtonLoadLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		Application.LoadLevel((int)Levels.FRONTEND);
	}
}
