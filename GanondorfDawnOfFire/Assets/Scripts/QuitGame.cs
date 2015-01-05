using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour 
{
	private float quitTimer = 5.0f;

	void Start()
	{
		DontDestroyOnLoad(this);
	}

	void Update ()
	{
		if ((Levels)Application.loadedLevel != Levels.VIDEO && quitTimer < 0.0f &&
			Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("Quitting Application");
			Application.Quit();
		}

		if (Application.isLoadingLevel)
		{
			quitTimer = 5.0f;
			Debug.Log("Quit Timer has Reset");
		}
		else
			quitTimer -= Time.deltaTime;
	}
}
