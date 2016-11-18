using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SCRA.Humanoids;

public class Enemy : Robot {
	
	public Transform mPlayer = null;
	public NavMeshAgent Agent { get { return this.mAgent; } set { this.mAgent = value; } }
	public Image mCurrentHealthBar;
	public Text mRatioText;
	public Speed mSpeed = new Speed();
	public bool mDebug = true;
	public bool canShoot = false;
	public bool isAlive = true;
	public float researchArea = 10f, mResetArea = 0f;
	public float mColorLerpSpeed = .2f;
	public int mCurrentWP = 0;
	public int mPrevWP = -1;
	public float mAccWP = 5f;
	public GameObject[] mWaypoints;

	private NavMeshAgent mAgent = null;
	private FieldOfView fov = null;
	[SerializeField]
	private StateMachine mStateMachine = null;
	private float mHealth;
	private float mMaxHealth;
	private Color[] mColorArr = new Color[2];
	
	public FieldOfView GetFieldOfView () {
		return this.fov;
	}

	public StateMachine GetFSM (){
		return this.mStateMachine;
	}

	public float GetHealth () {
		return this.mHealth;
	}

	public void TriggerEnemy(){
		this.mPlayer = GameObject.FindGameObjectWithTag("Robot").transform;
		this.mStateMachine.ChangeState(AttackState.Instance());
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
		if(this.mWaypoints != null && this.mWaypoints.Length == 0)
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
	
		this.mStateMachine = new StateMachine (this);
		this.mStateMachine.SetCurrentState(PatrolState.Instance());
//		this.mStateMachine.SetGlobalState (PatrolState.Instance ());
	}
	
	protected override void Update(){		
		if(this.mIsAlive){
			base.Update();
			this.mStateMachine.Update();
			
			if(this.mHealth <= 0f){
				this.mHealth = 0;
				this.mIsAlive = false;
			}
			
			if(mDebug)
				this.DebugEnemy();
		}
		
		this.UpdateHealthBar();
		
	}
	
	// FixedUpdate is called 
	protected override void FixedUpdate() {
		if(this.mIsAlive){
			base.FixedUpdate();
			this.mStateMachine.FixedUpdate();
		
			this.Move();
			this.Turn();
//			this.Jump();
		}
	}
	
	protected override void LateUpdate() {
		base.LateUpdate();
	}
	
	#endregion
	
	#region OVERRIDE METHODS
	
	protected override void Move () { }
	
	protected override void Turn () { }
	
	protected override void Jump () { }
	
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
