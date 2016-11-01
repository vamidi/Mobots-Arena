using UnityEngine;
using System.Collections;

public class Arm : Part, IShootable {
	
	public Transform mGunEnd;
	
	/// <summary>
	/// The damage that the robot deals to 
	/// the other player
	/// </summary>
	protected float mDamagePerRound = 5f;
	/// <summary>
	/// Rounds per second.
	/// How often they can fire
	/// </summary>
	protected float mRoundsPerSecond = 1f;
	/// <summary>
	/// Range of how far the player can shoot
	/// </summary>
	protected float mRange = 50f;
	/// <summary>
	/// The Robot accuracy.
	/// </summary>
	protected float mAccuracy = 3f;
	protected float mMouseVertical;
	[SerializeField]
	protected InputSettings mInput = new InputSettings();
	[SerializeField]
	protected OrbitSettings mOrbit = new OrbitSettings();
	protected float currentXrotation;
	protected float refRotateVel;
	protected float dampVel = 0.1f;
	protected Camera mCam;
	protected WaitForSeconds shotDuration = new WaitForSeconds(.07f);
	protected AudioSource mGunSound;


	public void SetDamagePerRound(float damage){
		this.mDamagePerRound = damage;
	}

	public void SetRoundsPerSecond(float seconds){
		this.mRoundsPerSecond = seconds;
	}

	public void SetAccuracy(float accuracy){
		this.mAccuracy = accuracy;
	}

	public void Shoot() {
		// left btn click
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Shoot");
		}
		
		if(Input.GetMouseButtonDown(1)){
			
		}
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mOrbit.mVorbitSmooth = 5f;
		this.mOrbit.mMinXRotation = -30f;
		this.mOrbit.mMaxXRotation = 30f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(mRobot.isControllable)
			this.GetInput();
	}
	
	protected virtual void LateUpdate(){
		if(mRobot.isControllable)
			this.Turn();
	}
	
	/// <summary>
	/// Is this method we get the input of the player
	/// </summary>
	protected void GetInput() {
		this.mMouseVertical = Input.GetAxisRaw(this.mInput.mMouseVertical);
	}
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	protected void Turn() {     
		this.mOrbit.mXRotation += -this.mMouseVertical * mOrbit.mVorbitSmooth;
		this.mOrbit.mXRotation = Mathf.Clamp(this.mOrbit.mXRotation, this.mOrbit.mMinXRotation, this.mOrbit.mMaxXRotation);
		this.currentXrotation = Mathf.SmoothDamp(this.currentXrotation, this.mOrbit.mXRotation, ref refRotateVel, dampVel);
		this.transform.localRotation = Quaternion.Euler(this.mOrbit.mXRotation, 0, 0);
	}
}
