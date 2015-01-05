using UnityEngine;
using System.Collections;

public class MusicVolume : MonoBehaviour 
{
	private UISlider volumeSlider;
	
	void Start()
	{
		volumeSlider = gameObject.GetComponent<UISlider>();
		volumeSlider.sliderValue = 1;
		EventDelegate.Add(volumeSlider.onChange, () => OnValueChange(volumeSlider.sliderValue));
	}

	void OnValueChange(float val)
	{
		audio.volume = val;
	}
}
