using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

	/****************************** PUBLIC PROPERTIES *********************/
	
	/// <summary>
	/// To see if the player is controllable
	/// </summary>
	public bool isControllable = true;  
	/// <summary>
	/// Name of the robot
	/// </summary>
	public string mName = "Henk de tank";
	/// <summary>
	/// The armor of the robot
	/// </summary>
	public float mArmor = 100f;
	/// <summary>
	/// Deadzone foor the input
	/// </summary>
	public float mInputDelay = 0.1f;                                               
	/// <summary>
	/// The velocity of the robot
	/// how fast he me turn.
	/// </summary>
	public float mRotateVel = 100f;                                                     
	/// <summary>
	/// The torso of the robot
	/// </summary>
	public Transform mTorsoTransform;                                                                                             
	
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
	private Quaternion mTargetRot, mTargetRotTorso;                                     
	/// <summary>
	/// Rigidbody of the robot
	/// </summary>
	[SerializeField]                    
	private Rigidbody mRigidbody;
	/// <summary>
	/// The m velocity.
	/// </summary>
	private Vector3 mVelocity = Vector3.zero;
	
	private float mMaxSlope = 60f;
	private bool mGrounded = false;
	/// <summary>
	/// Forward, RotateInput calculations
	/// </summary>
	[SerializeField]
	private float mForwardInput, mRotateInput, mJumpInput;                                    
	/// <summary>
	/// The mass of the robot
	/// </summary>
	[SerializeField]
	private int mMass;                                                             

	/// <summary>
	/// Vertical velocity
	/// </summary>
	private float mVerticalVel;
	/// <summary>
	/// Input variables
	/// </summary>
	private float mVOrbitInput, mHOrbitInput, mOrbitSnapInput;

	/// <summary>
	/// Gameobject of the parts
	/// </summary>
	[SerializeField]
	private GameObject goHead, goLarm, goRarm, goCar;                                   

	/// <summary>
	/// Classes of the parts
	/// </summary>
	[SerializeField]
	private Part[] mParts= new Part[4];                                      
		
	/****************************** PUBLIC METHODS *********************/
	
	/// <summary>
	/// Returns the rotation.
	/// </summary>
	/// <value>The target rotation.</value>
	public Quaternion TargetRotation {
		get { return mTargetRot;  }
	}

	/// <summary>
	/// Change the part of the robot.
	/// </summary>
	/// <param name="part">Part.</param>
	/// <param name="robotName">Robot name.</param>
	/// <param name="newObj">New object.</param>
	/// <param name="callBack">Call back.</param>
	public void SetRobot(PART part, string robotName, GameObject newObj, mAssignValues callBack){
		switch (part) {
			case PART.HEAD:
				if (newObj.name != goHead.name) {
					Transform parent = goHead.transform.parent;
					GameObject holder = (GameObject)Instantiate (newObj, goHead.transform.position, goHead.transform.rotation);
					holder.name = newObj.name;
					holder.tag = this.mTags.mHeadTag;
					holder.AddComponent<Head>();
					mParts [0] = holder.GetComponent<Head> ();
					holder.transform.parent = parent;
					Destroy (goHead);
					goHead = holder;
					
					goLarm.transform.localPosition = GameObject.Find("larm_spawn").transform.localPosition;
					goRarm.transform.localPosition = GameObject.Find("rarm_spawn").transform.localPosition;
				}
				break;
			case PART.LARM:
				if (newObj.name != goHead.name) {
					Transform parent = goLarm.transform.parent;
					GameObject holder = (GameObject)Instantiate (newObj, goLarm.transform.position, goLarm.transform.rotation);
					//holder.transform.localPosition = GameObject.Find("larm_spawn").transform.localPosition;
					holder.name = newObj.name;
					holder.tag = this.mTags.mLarmTag;
					holder.AddComponent<Larm>();
					mParts [1] = holder.GetComponent<Larm> ();
					holder.transform.parent = parent;
					Destroy (goLarm);
					goLarm = holder;
				}
				break;
			case PART.RARM:
				if (newObj.name != goHead.name) {
					Transform parent = goRarm.transform.parent;
					GameObject holder = (GameObject)Instantiate (newObj, goRarm.transform.position, goRarm.transform.rotation);
					holder.name = newObj.name;
					holder.tag = this.mTags.mRamTag;
					holder.AddComponent<Rarm>();
					mParts [2] = holder.GetComponent<Rarm> ();
					holder.transform.parent = parent;
					Destroy (goRarm);
					goRarm = holder;
				}
				break;
			case PART.CAR:
				if (newObj.name != goHead.name) {
					Transform parent = goCar.transform.parent;
					GameObject holder = (GameObject)Instantiate (newObj, goCar.transform.position, goCar.transform.rotation);
					holder.name = newObj.name;
					holder.tag = this.mTags.mCarTag;
					holder.AddComponent<Car>();
					mParts [3] = holder.GetComponent<Car> ();
					holder.transform.parent = parent;
					Destroy (goCar);
					goCar = holder;
				}
				break;
		}

		callBack (part, robotName);
	}

	/// <summary>
	/// Sets the correct values to the right part
	/// </summary>
	/// <param name="part">Part.</param>
	/// <param name="method">Method.</param>
	/// <param name="value">Value.</param>
	public void SetValue(PART part, string method = "", object value = null) {

		if (method == "" || value == null)
			return;

		switch (part) {
		case PART.HEAD:
			mParts [0].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			break;
		case PART.LARM:
			mParts [1].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			break;
		case PART.RARM:
			mParts [2].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			break;
		case PART.CAR:
			mParts [3].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			break;
		}
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
	
	void Awake() {
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
	void Start () {
	
		this.mTargetRot = this.transform.rotation;
		this.mRigidbody = this.GetComponent<Rigidbody>();

		if (!mRigidbody)
			Debug.LogError("Character needs Rigidbody");

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
		
		mForwardInput = mRotateInput = mJumpInput = 0;	
		
		this.mMass =  this.mParts [0].mRobotWegith + this.mParts [1].mRobotWegith + this.mParts [2].mRobotWegith + this.mParts [3].mRobotWegith;
		((Car)this.mParts[3]).SetSpeed(1825);
		
	}

	// Update is called once per frame
	void Update() {
		
//		Screen.lockCursor = (this.isControllable);
		
		if(this.isControllable){
			this.GetInput();
			this.OrbitRobot();
			
		}
	}

	// FixedUpdate is called 
	void FixedUpdate() {
		if(this.isControllable){
			this.Move();
			this.Turn();
			this.Jump();
			
			this.mRigidbody.velocity = this.transform.TransformDirection(this.mVelocity);
		}
	}

	// LateUpdate is called after each frame
	void LateUpdate() {
		if(this.isControllable)
			this.MoveToTarget();
	}

	/****************************** INPUT METHODS *********************/

	/// <summary>
	/// Is this method we get the input of the player
	/// </summary>
	void GetInput() {

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

	/****************************** ROTATION METHODS *********************/

	/// <summary>
	/// This method is for to calculate the
	/// orbiting of the torso
	/// </summary>
	void OrbitRobot() {
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
	void MoveToTarget() {
		if (this.mTorsoTransform) {
			// this.mTargetRotTorso = Quaternion.Euler(0, -this.mOrbit.mYRotation + Camera.main.transform.eulerAngles.y, 0);
			this.mTorsoTransform.rotation = Quaternion.Lerp(this.mTorsoTransform.rotation, Camera.main.transform.rotation, Time.deltaTime * this.mPosition.mLookSmooth);
		}
	}
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	void Turn() {
		float angle = mRotateVel * mRotateInput * Time.deltaTime;
		this.mTargetRot *= Quaternion.AngleAxis(angle, Vector3.up);
		transform.rotation = this.mTargetRot;
	}

	/****************************** MOVEMENT METHODS *********************/
	
	/// <summary>
	/// Movement of the robot
	/// </summary>
	void Move() {
		if(Mathf.Abs(this.mForwardInput) > this.mInputDelay) {
			// Move the player
			this.mVelocity.z = this.mForwardInput * ((Car)this.mParts[3]).GetSpeed();
		} else {
			this.mVelocity.z = 0;
		}
	}
	
	/// <summary>
	/// Jump this instance.
	/// </summary>
	void Jump(){
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
				
	void OnCollisionStay(Collision col) {
		foreach(ContactPoint contact in col.contacts){
			if(Vector3.Angle(contact.normal, Vector3.up) < this.mMaxSlope){
				this.mGrounded = true;
			}
		}
	}
	
	void OnCollisionExit(Collision col){
		this.mGrounded = false;
	}
}
