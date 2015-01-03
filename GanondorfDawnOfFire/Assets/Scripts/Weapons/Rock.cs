using UnityEngine;
using System.Collections;

public class Rock : Weapon
{
    public Unit owner;
    void Awake()
    {
        Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
	void Update () 
    {
        transform.position += transform.forward * Time.deltaTime * 5f;
	}
}
