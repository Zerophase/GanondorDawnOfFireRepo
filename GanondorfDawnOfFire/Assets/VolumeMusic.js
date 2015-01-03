#pragma strict

var hSliderValue : float = 0.0;

function Start () {

}

function Update () {

}

function OnGUI()
{
hSliderValue = GUI.HorizontalSlider (Rect (50, 50, 100, 30), hSliderValue, 0.0, 1.0);
GUI.Label(Rect(100, 100, 200, 20), "Value="+hSliderValue);
audio.volume=hSliderValue;
}