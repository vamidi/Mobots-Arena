using UnityEngine;
using System.Collections;

public class SimpleRotation : MonoBehaviour {

	public float mSpeed = 5f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// ...also rotate around the World's Y axis
		transform.Rotate(Vector3.up * Time.deltaTime * mSpeed, Space.World);	
	}
}
