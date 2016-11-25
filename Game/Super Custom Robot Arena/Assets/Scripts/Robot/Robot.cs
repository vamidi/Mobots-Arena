using UnityEngine;
using System.Collections;

namespace SCRA {
	
	namespace Humanoids {
		
		[System.Serializable]
		public class Speed {
			public float mPatrolSpeed = 6f;
			
			public float mChaseSpeed = 6f;			
		}
		
		[RequireComponent(typeof(Rigidbody))]
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
			
			/****************************** PRIVATE PROPERTIES *********************/
			
			/// <summary>
			/// To see if the player/enemy is alive
			/// </summary>
			protected bool mIsAlive = true;
			
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
			protected float mMass;                                                             
			protected float mResetMass;
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
			public float GetRobotMass(){
				return this.mMass;
			}
		
			public Part GetPart(int index){
				return mParts[index];
			}
		
			public bool IsAlive(){
				return this.mIsAlive;
			}
			
			#region UNITYMETHODS
			
			/****************************** UNITY METHODS *********************/
		
			protected virtual void Awake() {		
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
			
			#endregion
			
			#region ROTATIONMETHODS
			
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
}