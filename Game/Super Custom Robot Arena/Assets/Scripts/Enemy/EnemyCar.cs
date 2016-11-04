using UnityEngine;
using System.Collections;

public class EnemyCar : Part {
	
	/// <summary>
	/// The speed of the robot
	/// </summary>
	private float mSpeed;

	/// <summary>
	/// The jump strengtj pf the robot
	/// </summary>
	private float mJumpForce = 5f;

	public float GetSpeed(){
		return this.mSpeed;
	}

	public float GetJumpPower(){
		return this.mJumpForce;
	}

	public void SetSpeed(float speed){
		this.mSpeed = speed/this.mRobot.GetRobotMass();

	}

	public void SetJumpStrength(float strength){
		this.mJumpForce = strength/this.mRobot.GetRobotMass();
	}

	/// <summary>
	/// Damage the specified part
	/// </summary>
	/// <param name="d">Full damage</param>
	public override void Damage(float d){
		// Damgeperround = 20;
		// Shieldstrenght = 30;
		// DamageDone = ((100-Shieldstrenght)/100) * Damageperround
		// Damagedone = ((100-30)/100) * 20
		// Damagedone = 0.7 * 20
		// Damagedone = 14

		// Get the Head part
		EnemyHead tempHead = (EnemyHead) this.mRobot.GetPart(0);
		float damageOnHealth;

		if(tempHead.ArmorHealth <= 0){
			damageOnHealth = d;
		}else {
			damageOnHealth = ( (100f - tempHead.Strenght) / 100f ) * d;
		}

		this.mHealth -= damageOnHealth;
		tempHead.ArmorHealth -= d;

		// Update the healthbars
		if(this.mHealthBar)
			this.mHealthBar.UpdateHealthBar();
		tempHead.UpdateShieldBar();

	}
	
	// Called before the Start function
	protected override void Awake(){
		this.mRobot = this.transform.root.GetComponent<Enemy>();
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mPart = PART.CAR;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
}
