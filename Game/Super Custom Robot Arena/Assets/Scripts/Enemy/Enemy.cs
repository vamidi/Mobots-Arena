using UnityEngine;
using System.Collections;

using SCRA.Humanoids;

public enum STATES {
	PATROL, CHASE, ATTACK
}

public class Enemy : Robot {
	
	public Transform mPlayer;
	public bool mDebug = true;
	public bool canShoot = false;
	public float shootTimer = 2f, mReset = 2f;
	public int mCurrentWP = 0;
	public float mAccWP = 5f;
	public GameObject[] mWaypoints;
	
	private float mForward = 1f;
	private STATES mState = STATES.PATROL;
	
	protected override void Start(){
		base.Start();
		this.mWaypoints = GameObject.FindGameObjectsWithTag("Target");
		this.mParts [0] = this.goHead.GetComponent<EnemyHead> ();
		this.mParts [1] = this.goLarm.GetComponent<EnemyLarm> ();
		this.mParts [2] = this.goRarm.GetComponent<EnemyRarm> ();
		this.mParts [3] = this.goCar.GetComponent<EnemyCar> ();

		if (this.mParts [0].GetPart () != PART.HEAD)
			Debug.LogError ("The part is not a head part");

		if (this.mParts [1].GetPart () != PART.LARM)
			Debug.LogError ("The part is not a left arm part");

		if (this.mParts [2].GetPart () != PART.RARM)
			Debug.LogError ("The part is not a right arm part");

		if (this.mParts [3].GetPart () != PART.CAR)
			Debug.LogError ("The part is not a car part");
		
		this.mResetMass = this.mMass = this.mParts [0].mRobotWegith + this.mParts [1].mRobotWegith + this.mParts [2].mRobotWegith + this.mParts [3].mRobotWegith;
		((EnemyCar)this.mParts[3]).SetSpeed(1825);
	}
	
	protected override void Update(){
		base.Update();
		
		switch(this.mState){
			case STATES.PATROL:
				this.Patrol();
				break;
			case STATES.CHASE:
				this.Chase();
				break;
			case STATES.ATTACK:
				this.Attack();
				break;
			default:
				this.Patrol();
				break;
		}
		
		
		if(mDebug)
			this.DebugEnemy();
		
	}
	
	// FixedUpdate is called 
	protected override void FixedUpdate() {
//		this.Move();
//		this.Turn();
//		this.Jump();

//		this.mRigidbody.velocity = this.transform.TransformDirection(this.mVelocity);
	}
	
	private void Patrol(){
//		float angle = Vector3.Angle(direction, this.transform.forward);
//		direction.y = 0;
//		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
		
		if(this.mWaypoints.Length > 0){
			
			if(Vector3.Distance(this.mWaypoints[this.mCurrentWP].transform.position, this.transform.position) < this.mAccWP){
				this.mCurrentWP++;
				if(this.mCurrentWP >= this.mWaypoints.Length){
					this.mCurrentWP = 0;
				}
			}
			
		}
	}
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	protected override void Turn() {
		// Rotate 
		Vector3 direction = Vector3.zero; 
		
		switch(this.mState){
			case STATES.PATROL:
				direction = this.mWaypoints[this.mCurrentWP].transform.position - this.transform.position;
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.mRotateVel);		
				break;
		}
	}
	
	protected override void Move(){
		this.mVelocity.z = this.mForward * ((EnemyCar)this.mParts[3]).GetSpeed();
	}
	
	protected override void Jump(){
		
	}
	
	private void Chase(){
		Vector3 direction = this.mPlayer.position - this.transform.position;
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
		this.transform.Translate(0, 0, 0.05f);
		if(direction.magnitude > 15){
			this.mState = STATES.PATROL;
		}
	}
	
	private void Attack(){
		this.shootTimer -= Time.deltaTime;
		
		if(this.shootTimer <= 0){
			this.canShoot = true;
			this.shootTimer = mReset;
		}
			
		
		if(this.canShoot){
			((EnemyLarm)this.mParts[1]).Shoot();
			((EnemyRarm)this.mParts[2]).Shoot();
			this.canShoot = false;
		}
		
	}
	
	private void DebugEnemy(){
		
	}
}
