using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
    protected float timer = 0;
    protected bool runForward = true;
    [HideInInspector]
    public bool running = false;
	void Start () 
    {
	
	}

    public virtual IEnumerator Animation()
    {
        yield return new WaitForSeconds(0f);
    }

    
}
