using UnityEngine;
using System.Collections;
using MBA.UI;

public class SimpleRotation : MonoBehaviour {

	public float mSpeed = 5f;
	public RobotEditor mRobot;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// ...also rotate around the World's Y axis
		if(mRobot != null)
			transform.Rotate(Vector3.up * Time.deltaTime * mSpeed, Space.World);
		
	}
}
