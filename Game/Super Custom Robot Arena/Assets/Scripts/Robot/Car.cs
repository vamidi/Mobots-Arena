using UnityEngine;
using System.Collections;

public class Car : Part {

	/// <summary>
	/// The speed of the robot
	/// </summary>
	private float mSpeed = 5f;

	/// <summary>
	/// The jump strengtj pf the robot
	/// </summary>
	private float mJumpStrength = 15f;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.mPart = PART.CAR;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
