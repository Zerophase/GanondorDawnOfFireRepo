using UnityEngine;
using System.Collections;

public class KeyInput : MonoBehaviour
{
	public GUITexture graphic;
	public Texture2D standard;
	public Texture2D downgfx;
	public Texture2D upgfx;
	public Texture2D heldgfx;
	
	void Start()
	{
		graphic.texture = standard;
	}
	
	void Update ()
	{
		bool down = Input.GetKeyDown(KeyCode.P);
		bool held = Input.GetKey(KeyCode.P);
		bool up = Input.GetKeyUp(KeyCode.P);
		
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
	}
}