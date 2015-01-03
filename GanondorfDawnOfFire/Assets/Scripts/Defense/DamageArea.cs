using UnityEngine;
using System.Collections;

public class DamageArea : MonoBehaviour 
{
    public bool Hit { get; protected set; }

    protected void Start()
    {
        Hit = false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (IgnoreCol(other))
            return;

        //TODO debug for rocks hitting moblins, and character capsule
        try
        {
            //Debug.Log("Before Rock Collision detected collider name is: " + other.name);
            if (other.collider.name == "Rock" && this.transform.parent.name != "OctoRok")
            {
                Hit = true;
                MessageDispatcher.Instance.DispatchMessage(new RockTelegram(transform.parent.GetComponent<Unit>(),
                    other.GetComponent<Rock>().owner, other.GetComponent<Rock>(), HandledBy.GLOBAL));
                return;
            }
            else if (this.transform.parent.name == "OctoRok" || this.transform.parent.name == "Moblin")
                return;
            else if (other.collider.GetComponent<Unit>().Side != transform.parent.GetComponent<Unit>().Side
            && other.collider.GetType() == typeof(BoxCollider) &&
            this.transform.parent != other.transform.parent && !(other.collider is SphereCollider))
            {
                //Debug.Log("Hit " + other.collider.GetComponent<Unit>().ID);
                Hit = true;
                //Debug.Log("After Collision detected collider name is:" + other.name);
                return;
            }

            if (other.name == "CharacterCapsule" || other.transform.parent.name == "CharacterCapsule")
            {
                return;
            }
        }
        catch (System.NullReferenceException)
        {
            //Debug.Log("NullReferenceException " + "Collider's name: " + other.collider.name);
        }
        
    }

    protected void OnTriggerExit(Collider other)
    {
        if (IgnoreCol(other))
            return;

        try
        {
            if (other.collider.GetComponent<Unit>() == null)
                return;
            else if (other.collider.GetComponent<Unit>().Side != transform.parent.GetComponent<Unit>().Side)
            {
                Hit = false;
            }
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("NullReferenceException " + "Collider's name: " + other.collider.name);
        }
        
    }

    private bool IgnoreCol(Collider other)
    {
        try
        {
            if (other.collider is TerrainCollider || other.collider.isTrigger)
                return true;
            else if(other.transform.parent != null)
            {
                if (other.collider.name == this.transform.parent.name)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        catch (System.Exception e)
        {

            Debug.Log("NullReferenceException " + "Collider's name: " + other.collider.name);
            Debug.Log(other.collider.gameObject);
            Debug.Log(this.transform.parent.name);
            return false;
        }
        
    }
}
