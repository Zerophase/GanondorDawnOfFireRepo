using UnityEngine;
using System.Collections;

public class PowerAnimation : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			anim.SetTrigger("Hit");
			anim.SetBool("Bottom",true);
			anim.SetBool("Top",false);
			anim.SetTrigger("Power");
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			anim.SetTrigger("Hit 2");
			anim.SetBool("Bottom",false);
			anim.SetBool("Top",true);
			anim.SetTrigger("Power 2");
			
		}
	
	}
}
