using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour 
{

    IndexedPriorityQueue queue;
    List<float> listOfFloats = new List<float>();
	// Use this for initialization
	void Start () 
    {
        listOfFloats.Add(0.0f);

        queue = new IndexedPriorityQueue(listOfFloats, 5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
