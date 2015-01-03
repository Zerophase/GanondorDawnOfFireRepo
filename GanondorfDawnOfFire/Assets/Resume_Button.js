#pragma strict

/*var pause : boolean = true;

public var myUI_Root :GameObject;
public var myResume :GameObject;
var down  = Input.GetButtonDown("myResume");*/

//static function GetButtonDown(buttonName: Resume): bool;

/*public var buttonName = "myResume";

function Update () 
{
 
    if(Input.GetButtonDown("myResume") && pause == true)
   {
   pause = false;
   
   myUI_Root.SetActive(false);
   
   Time.timeScale = 1;
   }*/
   
   
   
   
   /*function DeactivateParent(g: GameObject, a: boolean) 
   {
	g.activeSelf = a;

	for (var parent: Transform in g.transform) 
	{
		DeactivateParent(parent.gameObject, a);
	}
}*/
   
   /*if(down)
   {
   pause = false;
   
   myUI_Root.SetActive(false);
   
   Time.timeScale = 1;
   }*/
   
    /*var down  = Input.GetButtonDown("Resume");
    var held = Input.GetButton("Resume");
    var up = Input.GetButtonUp("Resume");
    
    if(down)
    {
        graphic.texture = downgfx;
    }
    else if(held)
    {
        graphic.texture = heldgfx;
    }
    else if(up)
    {
        graphic.texture = upgfx;
    }
    else
    {
        graphic.texture = standard;
    }

    guiText.text = " " + down + "\n " + held + "\n " + up;
   */
//}   


/*#pragma strict

public var graphic : GUITexture;
public var standard : Texture2D;
public var downgfx : Texture2D;
public var upgfx : Texture2D;
public var heldgfx : Texture2D;

function Start()
{
    graphic.texture = standard;
}

function Update ()
{
    var down  = Input.GetButtonDown("Jump");
    var held = Input.GetButton("Jump");
    var up = Input.GetButtonUp("Jump");
    
    if(down)
    {
        graphic.texture = downgfx;
    }
    else if(held)
    {
        graphic.texture = heldgfx;
    }
    else if(up)
    {
        graphic.texture = upgfx;
    }
    else
    {
        graphic.texture = standard;
    }

    guiText.text = " " + down + "\n " + held + "\n " + up;
}*/