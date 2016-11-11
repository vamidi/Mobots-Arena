using UnityEngine;
using System.Collections;

public class Car : Part {
	
	/// <summary>
	/// The speed of the robot
	/// </summary>
	[SerializeField]
	private float mSpeed;

	/// <summary>
	/// The jump strengtj pf the robot
	/// </summary>
	private float mJumpForce = 5f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mPart = PART.CAR;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public float GetSpeed(){
		return this.mSpeed;
	}
	
	public float GetJumpPower(){
		return this.mJumpForce;
	}
	
	public void SetSpeed(float speed){
		speed = 2800f;
		this.mSpeed = speed/this.mRobot.GetRobotMass();
	}

	public void SetJumpStrength(float strength){
		this.mJumpForce = strength/this.mRobot.GetRobotMass();
	}
}
