using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

using MBA.UI;
using MBA.Humanoids;

public class Player : Robot {

	/****************************** PUBLIC PROPERTIES *********************/
	public Image mCurrentDamageBar, mCurrentWeightBar;
	public Text mTimerText, mWeightTimerText; 
	public float mTimer = 30f, mWeightTimer = 30f;
	public float mSpeedTimer = 2f;
	public bool mStartTimer, mStart;

	private float mResetTimer = 0f, mResetWeightTimer = 0f;
	/// <summary>
	/// Deadzone foor the input
	/// </summary>
	public float mInputDelay = 0.1f;                                                                                                                                                                                             
	
	/****************************** PRIVATE PROPERTIES *********************/
	
	/// <summary>
	/// The class for the input settings
	/// </summary>
	[SerializeField]
	private InputSettings mInput = new InputSettings();
	/// <summary>
	/// Position settings
	/// </summary>
	[SerializeField]
	public PositionSettings mPosition = new PositionSettings();                                                                                              
	
	/// <summary>
	/// Forward, RotateInput calculations
	/// </summary>
	[SerializeField]
	private float mForwardInput, mRotateInput, mJumpInput;                                                                                               

	/// <summary>
	/// Input variables
	/// </summary>
	private float mVOrbitInput, mHOrbitInput, mOrbitSnapInput;                                    
		
	/****************************** PUBLIC METHODS *********************/
	
	public void Initialize(){
		DontDestroyOnLoad(this.gameObject);
		this.mParts [0] = this.goHead.GetComponent<Head> ();
		this.mParts [1] = this.goLarm.GetComponent<Larm> ();
		this.mParts [2] = this.goRarm.GetComponent<Rarm> ();
		this.mParts [3] = this.goCar.GetComponent<Car> ();
			
		if (this.mParts [0].GetPart () != PART.HEAD)
			Debug.LogError ("The part is not a head part");

		if (this.mParts [1].GetPart () != PART.LARM)
			Debug.LogError ("The part is not a left arm part");

		if (this.mParts [2].GetPart () != PART.RARM)
			Debug.LogError ("The part is not a right arm part");

		if (this.mParts [3].GetPart () != PART.CAR)
			Debug.LogError ("The part is not a car part");

		this.mResetMass = this.mMass =  this.mParts [0].mRobotWegith + this.mParts [1].mRobotWegith + this.mParts [2].mRobotWegith + this.mParts [3].mRobotWegith;
		((Car)this.mParts[3]).SetSpeed(1825);
	}

	/// <summary>
	/// Increases the damage.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	public void IncreaseDamage(int x){
		this.mStartTimer = true;
		((Arm)this.mParts[1]).IncreaseDamage(x);
		((Arm)this.mParts[2]).IncreaseDamage(x);
	}
	
	public void IncreaseMovement(int x){ 
		/* 5%, 10%, 15% of the full weight of the robot */ 
		float check = 100 - x;
		check = check / 100;
		this.mMass = this.mMass * check;
		this.mStart = true;
		((Car)this.mParts[3]).SetSpeed(1825);
	}	
	
	#region UNITYMETHODS
	/****************************** UNITY METHODS *********************/

	protected override void Awake() {
		//				DontDestroyOnLoad(this.gameObject);
//		DontDestroyOnLoad(this.goHead);
//		DontDestroyOnLoad(this.goLarm);
//		DontDestroyOnLoad(this.goRarm);
//		DontDestroyOnLoad(this.goCar);
		base.Awake();
	}
	
	protected override void Start() {
		base.Start();
		
		this.mForwardInput = this.mRotateInput = this.mJumpInput = 0;
		
		this.mResetTimer = this.mTimer;
		this.mResetWeightTimer = this.mWeightTimer;
		this.mStartTimer = false;
		this.mStart = false;
	}
	
	// Update is called once per frame
	protected override void Update() {
		if(this.mParts[0] == null || this.mParts[1] == null || this.mParts[2] == null || this.mParts[3] == null)
			this.Initialize();
		
		if(this.isControllable){
			this.GetInput();
			this.OrbitRobot();
			this.ActivateTimer();
		}
	}

	// FixedUpdate is called 
	protected override void FixedUpdate() {
		if(this.isControllable){
			this.Move();
			this.Turn();
			this.Jump();
			
			this.mRigidbody.velocity = this.transform.TransformDirection(this.mVelocity);
		}
	}

	// LateUpdate is called after each frame
	protected override void LateUpdate() {
		if(this.isControllable)
			this.MoveToTarget();
	}

	protected override void OnCollisionStay(Collision col) {
		base.OnCollisionStay(col);
	}

	protected override void OnCollisionExit(Collision col){
		base.OnCollisionExit(col);
	}
	
	#endregion
	
	#region ROTATIONMETHODS
	
	/****************************** ROTATION METHODS *********************/

	/// <summary>
	/// This method is for to calculate the
	/// orbiting of the torso
	/// </summary>
	protected override void OrbitRobot() {
		if (this.mOrbitSnapInput > 0) {
			this.mOrbit.mYRotation = 0f;
		}

		this.mOrbit.mXRotation += this.mVOrbitInput * this.mOrbit.mVorbitSmooth * Time.deltaTime;
		this.mOrbit.mYRotation += -this.mHOrbitInput * this.mOrbit.mHorbitSmooth * Time.deltaTime;

		// cap the orbiting
		if (this.mOrbit.mXRotation > this.mOrbit.mMaxXRotation) {
			this.mOrbit.mXRotation = this.mOrbit.mMaxXRotation;
		}

		if (this.mOrbit.mXRotation < this.mOrbit.mMinXRotation) {
			this.mOrbit.mXRotation = mOrbit.mMinXRotation;
		}
	}

	/// <summary>
	/// Applying the rotation to the torso
	/// </summary>
	protected override void MoveToTarget() {
		if (this.mTorsoTransform) {
			this.mTargetRotTorso = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
			this.mTorsoTransform.rotation = Quaternion.Lerp(this.mTorsoTransform.rotation, this.mTargetRotTorso , Time.deltaTime * this.mPosition.mLookSmooth);
		}
	}
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	protected override void Turn() {
		float angle = mRotateVel * mRotateInput * Time.deltaTime;
		this.mTargetRot *= Quaternion.AngleAxis(angle, Vector3.up);
		transform.rotation = this.mTargetRot;
	}

	#endregion
	
	#region MOVEMENTMETHODS
	/****************************** MOVEMENT METHODS *********************/
	
	/// <summary>
	/// Movement of the robot
	/// </summary>
	protected override void Move() {
		if(Mathf.Abs(this.mForwardInput) > this.mInputDelay) {
			// Move the player
			if(this.mParts[3] != null)
				this.mVelocity.z = this.mForwardInput * ((Car)this.mParts[3]).GetSpeed();
		} else {
			this.mVelocity.z = 0;
		}
	}
	
	/// <summary>
	/// Jump this instance.
	/// </summary>
	protected override void Jump(){
//		Debug.Log(this.mGrounded);
		if(Mathf.Abs(mJumpInput) > 0 && mGrounded ){
			this.mVelocity.y = mPhysics.mJumpVel;
		}else if(mJumpInput == 0 && mGrounded){
			mVelocity.y = 0;
		}else{
			Vector3 vel = mVelocity;
			vel.y -=mPhysics.mDownAcc * Time.deltaTime;
			this.mVelocity = vel;
		}
	}
	
	#endregion
	
	#region INPUT METHODS
	/****************************** INPUT METHODS *********************/

	/// <summary>
	/// Is this method we get the input of the player
	/// </summary>
	private void GetInput() {

		// robot movement
		this.mForwardInput = Input.GetAxis(this.mInput.mVertical);
		this.mRotateInput = Input.GetAxis(mInput.mHorizontal);

		// body + arms rotation
		this.mVOrbitInput = Input.GetAxis(this.mInput.mMouseVertical);
		this.mHOrbitInput = Input.GetAxis(this.mInput.mMouseHorizontal);
		this.mOrbitSnapInput = Input.GetAxis(this.mInput.mOrbitHorizontalSnap);

		// Jump movement
		this.mJumpInput = Input.GetAxisRaw(this.mInput.mJump);
	}

	#endregion
	
	/// <summary>
	/// Activates the timer.
	/// </summary>
	private void ActivateTimer(){
		if(this.mStartTimer){
			this.mTimer -= Time.deltaTime * this.mSpeedTimer;

			if(this.mTimer <= 0){
				this.mTimer = this.mResetTimer;
				((Arm)this.mParts[1]).ResetDamage();
				((Arm)this.mParts[2]).ResetDamage();
				this.mStartTimer = false;
			}
			
			this.mTimerText.text = "Damage Timer: 00:" + this.mTimer.ToString("F2");
		}
		
		if(this.mStart){
			this.mWeightTimer -= Time.deltaTime * this.mSpeedTimer;

			if(this.mWeightTimer <= 0){
				this.mWeightTimer = this.mResetWeightTimer;
				this.mMass = this.mResetMass;
				((Car)this.mParts[3]).SetSpeed(1825);
				this.mStart = false;
			}

			this.mWeightTimerText.text = "Weight Timer: 00:" + this.mWeightTimer.ToString("F2");
		}
		
		this.UpdateTimerBar();
	}
	
	/// <summary>
	/// Updates the timer bar.
	/// </summary>
	private void UpdateTimerBar(){
		float ratio;
		if(this.mStartTimer){
			ratio = this.mTimer / 30f;
			this.mCurrentDamageBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
		}
		
		if(this.mStart){
			ratio = this.mWeightTimer / 30f;
			this.mCurrentWeightBar.rectTransform.localScale = new Vector3(ratio , 1, 1);
		}
	}

}
