using UnityEngine;
using System.Collections;

namespace SCRA {
	
	namespace Humanoids {

		public abstract class Robot : MonoBehaviour {
		
			/****************************** PUBLIC PROPERTIES *********************/ 
			/// <summary>
			/// Name of the robot
			/// </summary>
			public string mName = "Henk de tank";
			/// <summary>
			/// The armor of the robot
			/// </summary>
			public float mArmor = 100f;                                                                                                
			/// <summary>
			/// The torso of the robot
			/// </summary>
			public Transform mTorsoTransform;                                                                                             
		
			/****************************** PRIVATE PROPERTIES *********************/
		                       
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
		
			protected float mMaxSlope = 60f;
			protected bool mGrounded = false;                                   
			/// <summary>
			/// The mass of the robot
			/// </summary>
			[SerializeField]
			protected int mMass;                                                             
		
			/// <summary>
			/// Gameobject of the parts
			/// </summary>
			[SerializeField]
			protected GameObject goHead, goLarm, goRarm, goCar;                                   
		
			/// <summary>
			/// Classes of the parts
			/// </summary>
			[SerializeField]
			protected Part[] mParts= new Part[4];                                      
		
			/****************************** PUBLIC METHODS *********************/
		
			/// <summary>
			/// Returns the rotation.
			/// </summary>
			/// <value>The target rotation.</value>
			public Quaternion TargetRotation {
				get { return mTargetRot;  }
			}
			
		
			/// <summary>
			/// Gets the mass of the robot/
			/// </summary>
			/// <returns>The robot mass.</returns>
			public int GetRobotMass(){
				return this.mMass;
			}
		
			public Part GetPart(int index){
				return mParts[index];
			}
		
			/****************************** UNITY METHODS *********************/
		
			protected virtual void Awake() {
				DontDestroyOnLoad(this.gameObject);
				DontDestroyOnLoad(this.goHead);
				DontDestroyOnLoad(this.goLarm);
				DontDestroyOnLoad(this.goRarm);
				DontDestroyOnLoad(this.goCar);
		
				foreach( Transform child in this.transform){
					if (child.gameObject.tag == this.mTags.mCarTag) {
						this.goCar = child.gameObject;
					}
					if(child.childCount > 0) {
						foreach( Transform nodeChild in child){
							if (nodeChild.gameObject.tag == this.mTags.mHeadTag) {
								this.goHead = nodeChild.gameObject;
							}
							foreach (Transform innerChild in nodeChild) {
								if (innerChild.gameObject.tag == this.mTags.mLarmTag) {
									this.goLarm = innerChild.gameObject;
								}else if(innerChild.gameObject.tag == this.mTags.mRamTag){
									this.goRarm = innerChild.gameObject;
								}
							}
						}
					}
				}
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
					if(Vector3.Angle(contact.normal, Vector3.up) < this.mMaxSlope){
						this.mGrounded = true;
					}
				}
			}

			protected virtual void OnCollisionExit(Collision col){
				this.mGrounded = false;
			}
			
			/****************************** ROTATION METHODS *********************/
		
			/// <summary>
			/// This method is for to calculate the
			/// orbiting of the torso
			/// </summary>
			protected virtual void OrbitRobot() {
				/* if (this.mOrbitSnapInput > 0) {
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
				}*/
			}
		
			/// <summary>
			/// Applying the rotation to the torso
			/// </summary>
			protected virtual void MoveToTarget() {
				/*if (this.mTorsoTransform) {
					// this.mTargetRotTorso = Quaternion.Euler(0, -this.mOrbit.mYRotation + Camera.main.transform.eulerAngles.y, 0);
					this.mTorsoTransform.rotation = Quaternion.Lerp(this.mTorsoTransform.rotation, Camera.main.transform.rotation, Time.deltaTime * this.mPosition.mLookSmooth);
				}*/
			}
		
			/// <summary>
			/// This method is for to turn the robot
			/// </summary>
			protected virtual void Turn() {
				/* float angle = mRotateVel * mRotateInput * Time.deltaTime;
				this.mTargetRot *= Quaternion.AngleAxis(angle, Vector3.up);
				transform.rotation = this.mTargetRot; */
			}
		
			/****************************** MOVEMENT METHODS *********************/
		
			/// <summary>
			/// Movement of the robot
			/// </summary>
			protected virtual void Move() {
				/*if(Mathf.Abs(this.mForwardInput) > this.mInputDelay) {
					// Move the player
					this.mVelocity.z = this.mForwardInput * ((Car)this.mParts[3]).GetSpeed();
				} else {
					this.mVelocity.z = 0;
				}*/
			}
		
			/// <summary>
			/// Jump this instance.
			/// </summary>
			protected virtual void Jump(){
				/* //		Debug.Log(this.mGrounded);
				if(Mathf.Abs(mJumpInput) > 0 && mGrounded ){
					this.mVelocity.y = mPhysics.mJumpVel;
				}else if(mJumpInput == 0 && mGrounded){
					mVelocity.y = 0;
				}else{
					Vector3 vel = mVelocity;
					vel.y -=mPhysics.mDownAcc * Time.deltaTime;
					this.mVelocity = vel;
				}*/
			}
		}
	}
}