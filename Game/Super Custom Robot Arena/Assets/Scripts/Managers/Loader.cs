using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loader : MonoBehaviour {

	public Slider mSlider;	
	
	// Use this for initialization
	void Start () {
		if(mSlider)
			GameObject.FindObjectOfType<SceneLoader>().StartLevelAsync("", this.mSlider, GameUtilities.sceneLoaded);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
