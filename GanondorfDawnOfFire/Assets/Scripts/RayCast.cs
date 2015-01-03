using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour 
{
	GameObject message;
	GUIText messageGUITEXT;
	GameObject interactiveObject;
	public ParticleSystem gold;

	private ForwardDirection playerForward;

	void Start () 
	{
		message = GameObject.Find("GUI Text");
		messageGUITEXT = message.GetComponent<GUIText>();
		playerForward = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<ForwardDirection>();
	}

	void Update () 
	{
		if(Picker.Instance.InSphere(gameObject))
		{
			interactiveObject = playerForward.Hit().collider.gameObject;
			messageGUITEXT.text = "Pull";

			if(Input.GetKey(KeyCode.P))
			{
				gold.Play();
				gold.transform.position = interactiveObject.transform.position;
            	gold.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward * -1);
				Debug.Log(interactiveObject);
				interactiveObject.SetActive(false);
				
            print("pull");	
			}
		
		}
		else
		{
			messageGUITEXT.text = "";
		}

        if (gold.isPlaying)
        {
            gold.transform.position = (gameObject.transform.position - gold.transform.position); 
        }
	}
}
