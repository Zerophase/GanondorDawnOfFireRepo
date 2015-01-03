using UnityEngine;
using System.Collections;
public class Picker : MonoBehaviour {

    [HideInInspector]
    public static SphereCollider Sphere;

    private static Picker instance;

    private Vector3 defaultPos = new Vector3(100, 1000);

    private UnitMediator unitType;

    public static Picker Instance
    {
       get 
        { 
            if(instance == null)
            {
                instance = new GameObject("Picker").AddComponent<Picker>();
                Sphere = instance.gameObject.AddComponent<SphereCollider>();
                Sphere.isTrigger = true;
                Sphere.radius = 15f;
                LayerMask ignoreLayer = Physics.IgnoreRaycastLayer >> 1;
                Sphere.gameObject.layer = ignoreLayer;
            }
            return instance;
                
        }
        set { instance = value; }
    }
	
    public void UpdateSpherePos(Mediator mediator)
    {
        unitType = (UnitMediator)mediator;
        Sphere.transform.position = mediator.transform.position;
    }

    public void UpdateSpherePos()
    {
        Sphere.transform.position = defaultPos;
    }
    public bool InSphere(GameObject gameObject)
    {
        Transform goTransform = gameObject.transform;
        Transform sphereTransform = Sphere.transform;

        UnitMediator checkType = gameObject.GetComponent<UnitMediator>();
        if ((Mathf.Pow((goTransform.position.x - sphereTransform.position.x), 2) + 
            Mathf.Pow((goTransform.position.y - sphereTransform.position.y), 2) + 
            Mathf.Pow((goTransform.position.z - sphereTransform.position.z), 2)) < Mathf.Pow(Sphere.radius, 2) &&
            unitType.GetType() == checkType.GetType())
        {
            return true;
        }
        else
            return false;
    }
}
