using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Crosshair : MonoBehaviour {
	
	private Camera mCameraFacing;	

	// Use this for initialization
	void Start () {
		this.mCameraFacing = Camera.main;
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(this.mCameraFacing.transform.position);
		this.transform.Rotate(0, 180f, 0);
	}
}
