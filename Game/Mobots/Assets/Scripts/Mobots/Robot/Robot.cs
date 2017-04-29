using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobots.Robot {
	[System.Serializable]
	public class Speed {
		public float mPatrolSpeed = 6f;

		public float mChaseSpeed = 6f;
	}

	[RequireComponent(typeof(Rigidbody))]
	public abstract class Robot : MonoBehaviour {
		/****************************** PUBLIC PROPERTIES *********************/
		/// <summary>
		/// The torso of the robot
		/// </summary>
		public Transform mTorsoTransform;
		/// <summary>
		/// The velocity of the robot
		/// how fast he me turn.
		/// </summary>
		public float mRotateVel = 100f;
		/// <summary>
		/// Class for the orbit settings
		/// </summary>
		[SerializeField]
		public OrbitSettings mOrbit = new OrbitSettings();
		/// <summary>
		/// The m physics.
		/// </summary>
		[SerializeField]
		public PhysicSettings mPhysics = new PhysicSettings();
		/// <summary>
		/// The Tags
		/// </summary>
		public TagSettings mTags = new TagSettings();
		/// <summary>
		/// To see if the player is controllable
		/// </summary>
		public bool isControllable, mDebug = true;

		/****************************** PROTECTED PROPERTIES *********************/

		/// <summary>
		/// Target rotaion variables
		/// </summary>
		protected Quaternion mTargetRot, mTargetRotTorso;
		/// <summary>
		/// Rigidbody of the robot
		/// </summary>
		[SerializeField]
		protected Rigidbody mRigidbody;
		/// <summary>
		/// The m velocity.
		/// </summary>
		protected Vector3 mVelocity = Vector3.zero;

		protected int mMass = 0;
		protected int initializeMass = 0;
		/// <summary>
		/// The max slope the player/enemy can walk on
		/// </summary>
		protected float mMaxSlope = 60f;
		protected bool mGrounded;
		/// <summary>
		/// Gameobject of the parts
		/// </summary>
		[SerializeField]
		protected GameObject goHead, goLarm, goRarm, goCar;

		/// <summary>
		/// Array of the parts (only by part name)
		/// </summary>
		[SerializeField]
		protected Part[] mParts = new Part[4];

		/****************************** PUBLIC PROPERTIES *********************/

		public virtual void Initialize() { }

		public Part GetPart(int index) {
			return (mParts[index] != null) ? mParts[index] : null;
		}

		public int Mass {
			get { return mMass; }
			set { mMass = value; }
		}

		/// <summary>
		/// Sets the correct values to the right part
		/// </summary>
		/// <param name="part">Part.</param>
		/// <param name="method">Method.</param>
		/// <param name="value">Value.</param>
		public void SetValue(PartType part, string method = "", object value = null)  {
			if (method == "" || value == null)
				return;

			switch (part) {
				case PartType.Head:
					mParts [0].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
					break;
				case PartType.Larm:
					mParts [1].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
					break;
				case PartType.Rarm:
					mParts [2].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
					break;
				case PartType.Car:
					mParts [3].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
					break;
			}
		}

		/****************************** PRIVATE PROPERTIES *********************/

		#region UNITYMETHODS

		/****************************** UNITY METHODS *********************/

		protected virtual void Awake() {
			foreach( Transform child in this.transform){
				if (child.gameObject.CompareTag(mTags.mCarTag)) {
					this.goCar = child.gameObject;
				}
				if(child.childCount > 0) {
					foreach( Transform nodeChild in child){
						if (nodeChild.gameObject.CompareTag(mTags.mHeadTag)) {
							this.goHead = nodeChild.gameObject;
						}
						foreach (Transform innerChild in nodeChild) {
							if (innerChild.gameObject.CompareTag(mTags.mLarmTag)) {
								this.goLarm = innerChild.gameObject;
							}else if(innerChild.gameObject.CompareTag(mTags.mRarmTag)) {
								this.goRarm = innerChild.gameObject;
							}
						}
					}
				}
			}
			Initialize();
		}

		// Use this for initialization
		protected virtual void Start () {

			this.mTargetRot = this.transform.rotation;
			this.mRigidbody = this.GetComponent<Rigidbody>();

			if (!mRigidbody)
				Debug.LogError("Character needs Rigidbody");

		}

		// Update is called once per frame
		protected virtual void Update() { }

		// FixedUpdate is called
		protected virtual void FixedUpdate() {
			this.Move();
			this.Turn();
			this.Jump();

			this.mRigidbody.velocity = this.transform.TransformDirection(this.mVelocity);
		}

		// LateUpdate is called after each frame
		protected virtual void LateUpdate() {
			this.MoveToTarget();
		}

		protected virtual void OnCollisionStay(Collision col) {
			foreach(ContactPoint contact in col.contacts){
				if(Vector3.Angle(contact.normal, Vector3.up) < this.mMaxSlope) {
					this.mGrounded = true;
				}
			}
		}

		protected virtual void OnCollisionExit(Collision col) {
			this.mGrounded = false;
		}

		#endregion

		#region ROTATIONMETHODS

		/****************************** ROTATION METHODS *********************/

		/// <summary>
		/// This method is for to calculate the
		/// orbiting of the torso
		/// </summary>
		protected virtual void OrbitRobot() { }

		/// <summary>
		/// Applying the rotation to the torso
		/// </summary>
		protected virtual void MoveToTarget() { }

		/// <summary>
		/// This method is for to turn the robot
		/// </summary>
		protected virtual void Turn() { }

		#endregion

		#region MOVEMENTMETHODS

		/****************************** MOVEMENT METHODS *********************/

		/// <summary>
		/// Movement of the robot
		/// </summary>
		protected abstract void Move();

		/// <summary>
		/// Jump this instance.
		/// </summary>
		protected abstract void Jump();

		#endregion
	}
}