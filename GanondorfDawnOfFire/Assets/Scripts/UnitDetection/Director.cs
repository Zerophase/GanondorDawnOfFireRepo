using UnityEngine;
using System.Collections;

public abstract class Director : MonoBehaviour {

    public virtual void Hovered(Mediator mediator)
    {
        createSphere(mediator);
    }

    private void createSphere(Mediator mediator)
    {
       Picker.Instance.UpdateSpherePos(mediator);
    }
	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
