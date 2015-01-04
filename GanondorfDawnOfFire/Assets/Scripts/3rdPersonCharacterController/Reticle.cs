using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour 
{
    ForwardDirection forwardDirection;

    Vector3 pos;
    Vector3 zOffSet = new Vector3(0, 0, 18);

    void Start ()
    {
        forwardDirection = GameObject.FindObjectOfType<ForwardDirection>();
    }
	// Update is called once per frame
	void Update () 
    {
		gameObject.transform.TransformDirection(forwardDirection.RayExtreme);
	}
}
