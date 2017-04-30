using System.Collections.Generic;
using UnityEngine;

namespace Mobots.Robot {
	public class Player : Robot {
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
		private PositionSettings mPosition = new PositionSettings();

		private bool isDeleting = false;
		[SerializeField]
		private List<GameObject>mPlayerPositions = new List<GameObject>();

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
		public override void Initialize() {
			DontDestroyOnLoad(this.gameObject);
			this.mParts [0] = this.goHead.GetComponent<Head> ();
			this.mParts [1] = this.goLarm.GetComponent<Larm> ();
			this.mParts [2] = this.goRarm.GetComponent<Rarm> ();
			this.mParts [3] = this.goCar.GetComponent<Car> ();

			if (this.mParts [0].PartType != PartType.Head)
				Debug.LogError ("The part is not a head part");

			if (this.mParts [1].PartType != PartType.Larm)
				Debug.LogError ("The part is not a left arm part");

			if (this.mParts [2].PartType != PartType.Rarm)
				Debug.LogError ("The part is not a right arm part");

			if (this.mParts [3].PartType != PartType.Car)
				Debug.LogError ("The part is not a car part");

			if(this.mTorsoTransform == null)
				this.mTorsoTransform = GameUtilities.FindDeepChild(this.transform, "Body");
		}

		#region UNITYMETHODS
		/****************************** UNITY METHODS *********************/

		protected override void Awake() {
			// DontDestroyOnLoad(this.gameObject);
			DontDestroyOnLoad(this.goHead);
			DontDestroyOnLoad(this.goLarm);
			DontDestroyOnLoad(this.goRarm);
			DontDestroyOnLoad(this.goCar);
			base.Awake();
		}

		protected override void Start() {
			base.Start();

			this.mForwardInput = this.mRotateInput = this.mJumpInput = 0;

//			this.mResetTimer = this.mTimer;
//			this.mResetWeightTimer = this.mWeightTimer;
//			this.mStartTimer = false;
//			this.mStart = false;
//			StartCoroutine(this.SavePlayerPosition(2.5f));

			this.initializeMass = this.mMass =  this.mParts [0].Weight + this.mParts [1].Weight + this.mParts [2].Weight + this.mParts [3].Weight;
			if(mParts[3] != null)
				mParts[3].SetStats();
			Cursor.visible = false;

		}

		// Update is called once per frame
		protected override void Update() {
			if(this.mParts[0] == null || this.mParts[1] == null || this.mParts[2] == null || this.mParts[3] == null)
				this.Initialize();

			if(this.isControllable) {
				this.GetInput();
				this.OrbitRobot();
//				this.ActivateTimer();
			}
		}

		// FixedUpdate is called
		protected override void FixedUpdate() {
			if(isControllable) {
				Move();
				Turn();
				Jump();

				mRigidbody.velocity = transform.TransformDirection(mVelocity);
			}
		}

		// LateUpdate is called after each frame
		protected override void LateUpdate() {
			if(this.isControllable)
				this.MoveToTarget();
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
				mVelocity.z = this.mForwardInput * ((Car) this.mParts[3]).Speed;
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
			}else if(mJumpInput == 0 && mGrounded) {
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
	}
}