using UnityEngine;
using System.Collections;

public class Arm : Part, IShootable {
	
	public Transform mGunEnd;
	public GameObject mBullet;
	
	/// <summary>
	/// The damage that the robot deals to 
	/// the other player
	/// </summary>
	protected float mDamagePerRound = 5f;
	/// <summary>
	/// Rounds per second.
	/// How often they can fire
	/// </summary>
	protected float mRoundsPerSecond = .1f;
	protected float mNextFire;
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
	protected WaitForSeconds shotDuration = new WaitForSeconds(.07f);
	protected AudioSource mGunSound;
	protected LineRenderer mLaserLine;
	protected bool mFire = false;
	
	protected float mRecoilAmount;
	protected float mRecoilRecoverTime;
	protected float mCurrentRecoilPos;
	protected float mCurrentRecoilVel;
	

	public void SetDamagePerRound(float damage){
		this.mDamagePerRound = damage;
	}

	public void SetRoundsPerSecond(float seconds){
		this.mRoundsPerSecond = seconds;
	}

	public void SetAccuracy(float accuracy){
		this.mAccuracy = accuracy;
	}

	public virtual void Shoot() {

	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mOrbit.mVorbitSmooth = 5f;
		this.mOrbit.mMinXRotation = -30f;
		this.mOrbit.mMaxXRotation = 30f;
		this.mLaserLine = this.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(((Player)mRobot).isControllable){
			this.GetInput();
			this.Shoot();
		}
	}
	
	protected virtual void LateUpdate(){
		if(((Player)mRobot).isControllable){
			this.Turn();
			this.Move();
		}
			
	}
	
	/// <summary>
	/// Is this method we get the input of the player
	/// </summary>
	protected virtual void GetInput() {
		this.mMouseVertical = Input.GetAxisRaw(this.mInput.mMouseVertical);
	}
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	protected void Turn() {     
		this.mOrbit.mXRotation += -this.mMouseVertical * mOrbit.mVorbitSmooth;
		this.mOrbit.mXRotation = Mathf.Clamp(this.mOrbit.mXRotation, this.mOrbit.mMinXRotation, this.mOrbit.mMaxXRotation);
		this.currentXrotation = Mathf.Lerp(this.currentXrotation, this.mOrbit.mXRotation, dampVel);
		this.transform.localRotation = Quaternion.Euler(this.mOrbit.mXRotation, mOrbit.mYRotation, 0);
	}
	
	protected void Move(){
//		this.mCurrentRecoilPos -= Mathf.SmoothDamp(this.mCurrentRecoilPos, 0, ref this.mCurrentRecoilVel, this.mRecoilRecoverTime);
//		transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.mCurrentRecoilPos);
	}

	protected IEnumerator ShotEffect(){
		this.mLaserLine.enabled = true;
		yield return shotDuration;
		this.mLaserLine.enabled = false;
	}
}
