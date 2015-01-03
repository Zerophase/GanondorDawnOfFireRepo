#pragma strict

/*function Start () {

}*/

/*function Update () {

}*/

var pause : boolean = false;
//var pauseGUI : GUITexture;  //was added later
//var pauseGUI : UI_Root;
//pauseGUI.enabled = false;  //was added later
 


public var myPauseMenu :GameObject;
//public var myResume :GameObject;
//var down  = Input.GetButtonDown("Resume");

function Start ()
{
    //Debug.Log("Active Self: " + myPauseMenu.activeSelf);
    //Debug.Log("Active in Hierarchy" + myPauseMenu.activeInHierarchy);
} 


function Update () {
 
 
 
    if(Input.GetKeyDown("p") && pause == false)
   {
   
   pause = true;
   
   Time.timeScale = 0;
   
   Debug.Log( "I Paused the Game" );
   
   myPauseMenu.SetActive(true);
   Debug.Log( "I'm Enabled" );
   
   }
   //was added later
   /*if(pause == true) {
   pauseGUI.enabled = true;
   }*/
   
   //end of what was added
   
   else if(Input.GetKeyDown("p") && pause == true) {
   
   Debug.Log( "I Unpaused the Game" );
   
   pause = false;
   
   Time.timeScale = 1;
   
   myPauseMenu.SetActive(false);
   
   //pauseGUI.enabled = false;  //was added later
   
   }
   
   /*else if(Input.GetButtonDown("Resume") && pause == true)
   {
   pause = false;
   
   myPauseMenu.SetActive(false);
   
   Time.timeScale = 1;
   }*/
 
}