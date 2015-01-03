using UnityEngine;
using System.Collections;

public class MovieScript : MonoBehaviour {
	public MovieTexture MoveTexture;
	private float timer;
	// Use this for initialization
	void Start () {
		renderer.material.mainTexture =  MoveTexture;
		MoveTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {

		if(timer >= MoveTexture.duration)
			MoveTexture.Stop();
		else
			timer += Time.deltaTime;

		if (!MoveTexture.isPlaying) 
		{
			Application.LoadLevel((int)Levels.TUTORIAL);
		}

        if (Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel((int)Levels.TUTORIAL);
	}
}
