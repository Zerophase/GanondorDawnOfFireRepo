using UnityEngine;
using System.Collections;

public class UnitMediator : Mediator {

    private Director director;

    private static int nextID = 0;

    protected Color unpicked;
    protected Color picked;

    private GameObject child;
    private Unit unitAssigned;

    [HideInInspector]
    public int ID;

    public virtual void Init(Director director, Unit unit)
    {
        this.director = director;

        ID = nextID++;

        unitAssigned = unit;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == "Model")
                child = gameObject.transform.GetChild(i).gameObject;
        }

        changeColor(unpicked);
    }

	void changeColor (Color color)
	{
		for (int i = 0; i < child.transform.childCount; i++) 
        {
			child.transform.GetChild (i).renderer.material.color = color;
		}
	}

    public void Picked()
    {
        this.director.Hovered(this);
    }
	
    protected void Update () 
    {
        if (Picker.Instance.InSphere(gameObject) && unitAssigned.Side == Allegiance.ALLY)
        {
			changeColor(picked);
        }
        else if (child.transform.GetChild(0).renderer.material.color == picked)
        {
            changeColor(unpicked);
        }
	}
}
