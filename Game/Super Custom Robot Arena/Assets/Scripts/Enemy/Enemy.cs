using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SCRA.Humanoids;

public enum STATES {
	PATROL, CHASE, ATTACK
}

public enum PRIORITY {
	RUN, COVER, SEARCH, NOTHING
}

public class Enemy : Robot {
	
	public Transform mPlayer = null;
	public Image mCurrentHealthBar;
	public Text mRatioText;
	public bool mDebug = true;
	public bool canShoot = false;
	public bool isAlive = true;
	public float researchArea = 10f, mResetArea = 0f;
	public float mColorLerpSpeed = .2f;
	public int mCurrentWP = 0;
	public int mPrevWP = -1;
	public float mAccWP = 5f;
	public GameObject[] mWaypoints;
	
	private Speed mSpeed = new Speed();
	private NavMeshAgent mAgent = null;
	private FieldOfView fov = null;
	public STATES mState = STATES.PATROL;
	private PRIORITY mPriority = PRIORITY.NOTHING;
	private float mHealth;
	private float mMaxHealth;
	private Color[] mColorArr = new Color[2];
	
	public void TriggerEnemy(){
		this.mPlayer = GameObject.FindGameObjectWithTag("Robot").transform;
		this.mState = STATES.ATTACK;
	}
	
	#region UNITYMETHODS
	
	protected override void Start(){
		base.Start();
		
		this.mAgent = this.GetComponent<NavMeshAgent>();
		if(!this.mAgent)
			Debug.LogError("There is no navmesh agent attached to this gameobject");
		
		this.fov = this.GetComponent<FieldOfView>();
		if(!this.fov)
			Debug.LogError("There is no field of view script attached to this gameobject");
		
		this.mWaypoints = GameObject.FindGameObjectsWithTag("Target");
		if(this.mWaypoints && this.mWaypoints.Length == 0)
			Debug.LogError("There are no waypoints set in the map");
		
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
		
		this.mAgent.speed = this.mSpeed.mChaseSpeed = ((EnemyCar)this.mParts[3]).GetSpeed();
		
		this.mColorArr[0] = new Color(1f, .007f, .007f);
		this.mColorArr[1] = new Color(.17f, .96f, 0f);
		
		for(int i = 0; i < this.mParts.Length; i++){
			// Debug.Log("Part: " + this.mParts[i].GetPart() + " Health " + this.mParts[i].GetHealth());
			this.mMaxHealth += mParts[i].GetMaxHealth();
		}

		this.mHealth = this.mMaxHealth;
		
		this.mResetArea = this.researchArea;
	}
	
	protected override void Update(){
		base.Update();
		
		if(this.mIsAlive){
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
		
	}
	
	// FixedUpdate is called 
	protected override void FixedUpdate() {
		this.Move();
		this.Turn();
//		this.Jump();
		
		float healthPercentage = (this.mHealth / 100 ) * 50;
		
		if(this.mHealth < healthPercentage)
			this.mPriority = PRIORITY.SEARCH;
	}
	
	protected override void LateUpdate() {
		base.LateUpdate();
	}
	
	#endregion
	
	protected override void Move(){
		switch(mState){
			case STATES.PATROL:
				/*if(this.mPriority == PRIORITY.SEARCH){
					if(this.fov.mVisibleTargets.Count > 0){
						foreach(Transform target in this.fov.mVisibleTargets){
							if(target.GetComponent<Capsule>().mKind == KIND.HEALTH){
								this.mAgent.SetDestination(target.position);
								break;
							}
						}
					}
				}else */
				if(this.mAgent.destination != this.mWaypoints[this.mCurrentWP].transform.position)
					this.mAgent.SetDestination(this.mWaypoints[this.mCurrentWP].transform.position);
				break;
			case STATES.CHASE:
				this.mAgent.speed = mSpeed.mChaseSpeed;
				if(this.mPlayer != null){
					if(this.mAgent.destination != this.mPlayer.position)
						this.mAgent.SetDestination(this.mPlayer.position);
				}
				break;
			case STATES.ATTACK:
				this.mAgent.speed = 0;
				break;
		}
	}
	
	protected override void Turn(){
		Vector3 direction = Vector3.zero; 
		Vector3 viewAngleA, viewAngleB; 
		switch(this.mState){
			case STATES.PATROL:
				break;
			case STATES.CHASE:
				direction = this.mPlayer.position - this.transform.position;
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
				break;
			case STATES.ATTACK:
				if(this.mPlayer){
					direction = this.mPlayer.position - this.transform.position;
					this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.mRotateVel);	

					if(Vector3.Angle( this.transform.forward, direction ) < 5f){
						this.mState = STATES.ATTACK;
					}
				}
				
				viewAngleA = fov.DirectionFromAngle(-15f, false); 
				viewAngleB = fov.DirectionFromAngle(15f, false); 
				//				Debug.Log(Vector3.Distance(target.transform.position, viewAngleA) > 15f );
				//				Debug.Log(Vector3.Distance(target.transform.position, viewAngleB) > 5f );
				if(Vector3.Distance(this.mPlayer.transform.position, viewAngleA) > 15f ||
					Vector3.Distance(this.mPlayer.transform.position, viewAngleB) > 0 ){
					direction = this.mPlayer.position - this.transform.position;
					this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.mRotateVel);	
				}
				break;
		}
	}
	
	protected override void Jump() { }
	
	#region ENEMYSTATES

	private void Patrol(){
		if(this.mWaypoints && this.mWaypoints.Length > 0){
			if(Vector3.Distance(this.mWaypoints[this.mCurrentWP].transform.position, this.transform.position) < this.mAccWP){
				this.mCurrentWP++;
				if(this.mCurrentWP >= this.mWaypoints.Length){
					this.mCurrentWP = 0;
				}
			}
		}

		if(this.mPlayer)
			this.mState = STATES.CHASE;
	}
	
	private void Chase(){
		if(this.mPlayer != null && Vector3.Distance(this.transform.position, this.mPlayer.position) < this.fov.mViewRadius ){
			this.researchArea = this.mResetArea;
			this.mState = STATES.ATTACK;
		}
		
		// do the timer to see if the player is behind a wall
		this.researchArea -= Time.deltaTime;
		if(this.researchArea <= 0){
			this.researchArea = this.mResetArea;
			this.mPlayer = this.fov.FindTarget();
			if(!this.mPlayer){
				this.mState = STATES.PATROL;
			}
		}
	}
	
	private void Attack(){
		((EnemyLarm)this.mParts[1]).Shoot();
		((EnemyRarm)this.mParts[2]).Shoot();
		
		if(this.mPlayer != null && Vector3.Distance(this.transform.position, this.mPlayer.position) > this.fov.mViewRadius){
			this.mState = STATES.CHASE;
		}	
	}
	
	#endregion
	
	private void DebugEnemy(){
		Vector3 viewAngleA = fov.DirectionFromAngle(-15f, false); 
		Vector3 viewAngleB = fov.DirectionFromAngle(15f, false); 

		Debug.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.mViewRadius, Color.yellow);
		Debug.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.mViewRadius, Color.yellow);
	}

	#region HEALTHMETHODS
	
	private float Map(float value, float inMin, float inMax, float outMin, float outMax){
		return ( value - inMin ) * ( outMax - outMin) / ( inMax - inMin ) + outMin;
	}
	
	private void UpdateHealthBar(){
		this.mHealth = 0;
		for(int i = 0; i < this.mParts.Length; i++){
			this.mHealth += mParts[i].GetHealth();
		}
	
		if(this.mCurrentHealthBar){
			float ratio = Map( this.mHealth, 0, this.mMaxHealth, 0, 1);
			this.mCurrentHealthBar.fillAmount = Mathf.Lerp(this.mCurrentHealthBar.fillAmount, ratio, Time.deltaTime * this.mColorLerpSpeed);
			this.mCurrentHealthBar.color = Color.Lerp(this.mColorArr[0], this.mColorArr[1], ratio);
	
			if(mRatioText)
				mRatioText.text = (ratio * 100 ).ToString("0") + "%";			
		}
	
	}

	#endregion
}
