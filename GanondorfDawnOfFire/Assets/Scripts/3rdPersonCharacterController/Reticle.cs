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
		gameObject.transform.position = new Vector3(forwardDirection.transform.position.x + 1.4f,
		                                            forwardDirection.transform.position.y + 3.9f,
		                                            forwardDirection.transform.position.z + zOffSet.z);

    }
	// Update is called once per frame
	void Update () 
    {
        gameObject.transform.TransformDirection(forwardDirection.transform.position.x, 
		                                        forwardDirection.transform.position.y,
		                                        forwardDirection.transform.position.z + zOffSet.z);
	}
}
