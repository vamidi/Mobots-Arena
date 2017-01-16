using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Arm : Part, IShootable {
	
	public Transform mGunEnd;
	public Transform mMuzzle;
	public GameObject mBullet;
	public bool mCanFire = false;
	public LayerMask mObstacle;
	
	[SerializeField]
	protected InputSettings mInput = new InputSettings();
	/// <summary>
	/// The damage that the robot deals to 
	/// the other player
	/// </summary>
	[SerializeField]
	protected float mDamagePerRound = 5f;
	protected float mResetDamage;
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
	protected float mSmallRange = 5f;
	/// <summary>
	/// The Robot accuracy.
	/// </summary>
	protected float mAccuracy = 3f;
	protected WaitForSeconds shotDuration = new WaitForSeconds(.09f);
	protected ParticleSystem mParticleSystem;
	protected AudioClip mGunSound;
	protected AudioSource mGunAudioSource;
	protected LineRenderer mLaserLine;
	protected bool mFire = false;
	
	protected float mRecoilAmount;
	protected float mRecoilRecoverTime;
	protected float mCurrentRecoilPos;
	protected float mCurrentRecoilVel;
	
	public override void Initialize() {
		this.mNextFire = Time.time + this.mRoundsPerSecond;
	}
	
	public void ResetDamage(){
		this.mDamagePerRound = this.mResetDamage;
	}

	public void SetDamagePerRound(float damage){
		this.mDamagePerRound = damage;
	}

	public virtual void SetRoundsPerSecond(float seconds){
		this.mRoundsPerSecond = seconds;
	}

	public void SetAccuracy(float accuracy){
		this.mAccuracy = accuracy;
	}

	public override void IncreaseDamage(double x){
		// 1.2x, 1.4x, 1.6x, 1.8x, 2.0x of all damage done
		// 100% => 120%
		this.mDamagePerRound = this.mDamagePerRound / 100 * (float)x;
	}

	public virtual void Shoot() {
		if(this.mCanFire && this.mFire){
			Vector3 rayOrg = this.mGunEnd.position; //this.mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,  0));
			RaycastHit hit;

			if(Physics.Raycast(rayOrg, this.mGunEnd.transform.forward, out hit, this.mSmallRange, this.mObstacle)) {
				Debug.Log("Wall");
			}
		}
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mLaserLine = this.GetComponent<LineRenderer>();
		this.mResetDamage = this.mDamagePerRound;
		this.mGunEnd = this.GetComponentsInChildren<Transform>()[1];
		if(this.GetComponentsInChildren<Transform>().Length > 2){
			this.mMuzzle = this.GetComponentsInChildren<Transform>()[2];
			if(this.mMuzzle)
				this.mMuzzle.position = mGunEnd.position;
			this.mParticleSystem = this.mMuzzle.GetComponent<ParticleSystem>();
		}
		this.mGunAudioSource = this.gameObject.AddComponent<AudioSource>();
		this.mGunAudioSource.playOnAwake = false;
		this.mGunSound = (AudioClip) GameUtilities.ReadResourceFile("placeholdersound");
		if(this.mGunSound){
			this.mGunAudioSource.clip = this.mGunSound;
			this.mGunAudioSource.loop = false;
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(((Player)mRobot).isControllable){
			this.GetInput();
			this.Shoot();
		}
	}
	
	protected virtual void LateUpdate () {
		if(((Player)mRobot).isControllable){
			this.Turn();
			this.Move();
		}		
	}
	
	/// <summary>
	/// Is this method we get the input of the player
	/// </summary>
	protected virtual void GetInput () { }
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	protected void Turn () { }
	
	protected void Move () { }

	protected IEnumerator ShotEffect () {
		if(this.mGunSound)
			this.mGunAudioSource.Play();
		
		if(this.mParticleSystem)
			this.mParticleSystem.Play();
		
		if(this.mLaserLine)
			this.mLaserLine.enabled = true;
		
		yield return shotDuration;
		
		if(this.mLaserLine)
			this.mLaserLine.enabled = false;
	}
	
	protected void OnDrawGizmosSelected() {
//		Gizmos.color = Color.yellow;
//		Vector3 direction = this.mGunEnd.transform.forward * this.mRange;
//		Gizmos.DrawRay(this.mGunEnd.position, direction);
	}
}
