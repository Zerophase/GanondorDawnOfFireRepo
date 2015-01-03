using UnityEngine;
using System.Collections;

public class ForwardDirection : MonoBehaviour {

    Ray ray;
    float distance = 20f;

    RaycastHit hitInfo;

    private LayerMask layersToHit;

    public Transform ForwardPosition { get { return transform; } }
	// Use this for initialization
	void Start () 
    {
        ray = new Ray( transform.position, transform.forward);

	}
	
	// Update is called once per frame
	void Update () 
    {
        ray.origin = transform.position;
        ray.direction = transform.forward;
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
	}

    public RaycastHit Hit()
    {
       Physics.Raycast(ray.origin, ray.direction, out hitInfo,  distance);
       return hitInfo;
    }
}
