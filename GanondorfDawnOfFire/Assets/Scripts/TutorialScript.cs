using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {
	GameObject tutorial;
	GameObject leftSphere;
	GameObject rightSphere;
	GameObject upSphere;
	public GUIText TutorialText;

	Animator anim;
	// Use this for initialization
	void Start () {

		//Set all game objects used and GUI TEXt from the scene
		anim = GetComponent<Animator>();
		tutorial = GameObject.Find("TutorialText");
		TutorialText = tutorial.GetComponent<GUIText>();
		leftSphere = GameObject.Find("LookleftSphere");
		rightSphere = GameObject.Find("LookRightSphere");
		upSphere = GameObject.Find("LookUpSphere");
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			anim.SetTrigger("Hit");
			anim.SetBool("Bottom",true);
			anim.SetBool("Top",false);
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			anim.SetTrigger("Hit 2");
			anim.SetBool("Bottom",false);
			anim.SetBool("Top",true);

		}

	}



	void OnTriggerEnter(Collider collider){

		//Only displays certain text based on which collider the player goes through
		//The Text is displayed then starts the couroutine to take away text then destroys collider so cant access it again

		if(collider.tag == "FirstTutorialBox")
		{
		TutorialText.text = "You can use the Spacebar to grab the Enemy and hit space again to throw them";
			StartCoroutine(DisableText());
			Destroy(collider);
		}
		if(collider.tag == "SecondTutorialBox")
		{
			TutorialText.text = "Nice, Hold down the right Mouse Button to look around";
			leftSphere.renderer.enabled = true;
			rightSphere.renderer.enabled = true;
			upSphere.renderer.enabled = true;
			StartCoroutine(DisableText());
			Destroy (collider);
		}
	}
	IEnumerator DisableText()
	{
		//waits for 3 seconds till it gets rid of text in GUI TEXt
		yield return new WaitForSeconds(3f);
		TutorialText.text = " ";
	}
	
}
