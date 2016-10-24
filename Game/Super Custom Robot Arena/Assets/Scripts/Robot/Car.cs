using UnityEngine;
using System.Collections;

public class Car : Part {

	/// <summary>
	/// The speed of the robot
	/// </summary>
	[SerializeField]
	private float mSpeed = 5f;

	/// <summary>
	/// The jump strengtj pf the robot
	/// </summary>
	[SerializeField]
	private float mJumpForce = 15f;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.mPart = PART.CAR;
	}
	
	// Update is called once per frame
	protected override void Update () {
		
	}
	
	public float GetSpeed(){
		return this.mSpeed;
	}
	
	public float GetJumpPower(){
		return this.mJumpForce;
	}

	public void SetSpeed(float speed){
		this.mSpeed = speed;
	}

	public void SetJumpStrength(float strength){
		this.mJumpForce = strength;
	}
}
