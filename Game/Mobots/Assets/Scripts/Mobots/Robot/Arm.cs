using System.Collections;
using UnityEngine;

namespace Mobots.Robot {
	[System.Serializable]
	public class Arm : Part, Interfaces.IShootable {
		public Transform mGunEnd;
		public Transform mMuzzle;
		public GameObject mBullet;
		public bool mCanFire;
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

		public void SetDamagePerRound(float damage) {
			mDamagePerRound = damage;
		}

		public void SetAccuracy(float accuracy) {
			mAccuracy = accuracy;
		}

		public virtual void SetRoundsPerSecond(float seconds) {
			mRoundsPerSecond = seconds;
		}

		public virtual void Shoot() {
			if(this.mCanFire && this.mFire) {
				Vector3 rayOrg = this.mGunEnd.position; // this.mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,  0));
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
			if(this.mGunSound) {
				this.mGunAudioSource.clip = this.mGunSound;
				this.mGunAudioSource.loop = false;
			}
			mNextFire = Time.time + mRoundsPerSecond;
			mCanFire = true;
		}

		// Update is called once per frame
		protected override void Update() {
			base.Update();
			if(mRobot.isControllable) {
				GetInput();
				Shoot();
			}
		}

		/// <summary>
		/// Is this method we get the input of the player
		/// </summary>
		protected virtual void GetInput () { }

		protected IEnumerator ShotEffect () {
			if(mGunSound)
				mGunAudioSource.Play();

			if(mParticleSystem)
				mParticleSystem.Play();

			if(mLaserLine)
				mLaserLine.enabled = true;

			yield return shotDuration;

			if(mLaserLine)
				mLaserLine.enabled = false;
		}


	}
}