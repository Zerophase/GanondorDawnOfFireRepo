using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
    Vector3 down = new Vector3(0f, 1f);

    private bool triggered = false;
    private float moveSpeed = 4f;

    void Start()
    {
        triggered = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Moblin" && triggered == false)
        {
            triggered = true;
            StartCoroutine(lowerDoor());
        }
    }
	
    IEnumerator lowerDoor()
    {
        for (int i = 0; i < 60; i++)
        {
            transform.parent.position -= down * Time.deltaTime * moveSpeed;
            yield return new WaitForFixedUpdate();
        }
    }
}
