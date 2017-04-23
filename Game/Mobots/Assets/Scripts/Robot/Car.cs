using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HealthBar))]
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
	
	public override void Initialize() {
		if(this.mHealthBar)
			this.mHealthBar.Initialize();
	}
	
	protected override void Awake(){
		base.Awake();
		this.mPart = PART.CAR;
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
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
		if(this.mRobot != null)
			this.mSpeed = speed/this.mRobot.GetRobotMass();
	}

	public void SetJumpStrength(float strength){
		if(this.mRobot != null)
			this.mJumpForce = strength/this.mRobot.GetRobotMass();
	}
}
