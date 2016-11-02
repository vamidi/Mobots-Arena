using UnityEngine;
using System.Collections;

public class RayViewer : MonoBehaviour {
	
	private float mRange = 50f;
	private Camera mCamera;

	// Use this for initialization
	void Start () {
		this.mCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 lineOrigin = this.mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
		Debug.DrawRay(lineOrigin, this.mCamera.transform.forward * mRange, Color.green);
	}
}
