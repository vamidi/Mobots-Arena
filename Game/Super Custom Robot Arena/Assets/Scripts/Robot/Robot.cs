using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Robot : MonoBehaviour { // IDamageable<float> 

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
	/// Tag for the head
	/// </summary>
	public string mHeadTag = "";                                                        
	/// <summary>
	/// Tag for the left arm
	/// </summary>
	public string mLarmTag = "";                                                   
	/// <summary>
	/// Tag for the right arm
	/// </summary>
	public string mRamTag = "";                                            
	/// <summary>
	/// Tag for the car
	/// </summary>
	public string mCarTag = "";                                                         
	/// <summary>
	/// Tag for the floor
	/// </summary>
	public string mGroundTag = "";	
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

	/// <summary>
	/// Position settings
	/// </summary>
	public PositionSettings mPosition = new PositionSettings();                        
	/// <summary>
	/// Class for the orbit settings
	/// </summary>
	public OrbitSettings mOrbit = new OrbitSettings();                                  

	/****************************** PRIVATE PROPERTIES *********************/
	
	/// <summary>
	/// The class for the input settings
	/// </summary>
	[SerializeField]
	private InputSettings mInput = new InputSettings();     
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
	/// Forward, RotateInput calculations
	/// </summary>
	[SerializeField]
	private float mForwardInput, mRotateInput;                                    
	/// <summary>
	/// The mass of the robot
	/// </summary>
	[SerializeField]
	private int mMass = 10;                                                             

	/// <summary>
	/// Vertical velocity
	/// </summary>
	private float mVerticalVel;
	/// <summary>
	/// The gravity
	/// </summary>
	private float mGravity = -14.0f;
	/// <summary>
	/// To see if the player is grounded
	/// </summary>
	private bool isGrounded = false;
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
	private Part[] mParts= new Part[4];                                                 // Classes of the parts
		
	/****************************** PUBLIC METHODS *********************/
	
	public Quaternion TargetRotation {
		get { return mTargetRot;  }
	}

	public void SetRobot(PART part, string robotName, GameObject newObj, mAssignValues callBack){
		switch (part) {
			case PART.HEAD:
				if (newObj.name != goHead.name) {
					Transform parent = goHead.transform.parent;
					GameObject holder = (GameObject)Instantiate (newObj, goHead.transform.position, goHead.transform.rotation);
					holder.name = newObj.name;
					holder.tag = mHeadTag;
					holder.AddComponent<Head>();
					mParts [0] = holder.GetComponent<Head> ();
					holder.transform.parent = parent;
					Destroy (goHead);
					goHead = holder;
					
					goLarm.transform.position = GameObject.Find("larm_spawn").transform.position;
					goRarm.transform.position = GameObject.Find("rarm_spawn").transform.position;
				}
				break;
			case PART.LARM:
				if (newObj.name != goHead.name) {
					Transform parent = goLarm.transform.parent;
					GameObject holder = (GameObject)Instantiate (newObj, GameObject.Find("larm_spawn").transform.position, goLarm.transform.rotation);
					holder.name = newObj.name;
					holder.tag = mLarmTag;
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
					GameObject holder = (GameObject)Instantiate (newObj, GameObject.Find("rarm_spawn").transform.position, goRarm.transform.rotation);
					holder.name = newObj.name;
					holder.tag = mRamTag;
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
					holder.tag = mCarTag;
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
	
	/****************************** UNITY METHODS *********************/
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		DontDestroyOnLoad(this.goHead);
		DontDestroyOnLoad(this.goLarm);
		DontDestroyOnLoad(this.goRarm);
		DontDestroyOnLoad(this.goCar);
		
		this.mTargetRot = this.transform.rotation;
		this.mRigidbody = this.GetComponent<Rigidbody>();

		if (!mRigidbody)
			Debug.LogError("Character needs Rigidbody");
		
		foreach( Transform child in this.transform){
			if (child.gameObject.tag == this.mCarTag) {
				this.goCar = child.gameObject;
			}
			if(child.childCount > 0) {
				foreach( Transform nodeChild in child){
					if (nodeChild.gameObject.tag == this.mHeadTag) {
						this.goHead = nodeChild.gameObject;
					}
					foreach (Transform innerChild in nodeChild) {
						if (innerChild.gameObject.tag == this.mLarmTag) {
							this.goLarm = innerChild.gameObject;
						}else if(innerChild.gameObject.tag == this.mRamTag){
							this.goRarm = innerChild.gameObject;
						}
					}
				}
			}
		}

		this.mParts [0] = this.goHead.GetComponent<Head> ();
		this.mParts [1] = this.goLarm.GetComponent<Larm> ();
		this.mParts [2] = this.goRarm.GetComponent<Rarm> ();
		this.mParts [3] = this.goCar.GetComponent<Car> ();

		if (!this.mParts[0] || this.mParts [0].GetPart () != PART.HEAD)
			Debug.LogError ("The part is not a head part");

		if (!this.mParts[1] || this.mParts [1].GetPart () != PART.LARM)
			Debug.LogError ("The part is not a left arm part");

		if (!this.mParts[2] || this.mParts [2].GetPart () != PART.RARM)
			Debug.LogError ("The part is not a right arm part");

		if (!this.mParts[3] || this.mParts [3].GetPart () != PART.CAR)
			Debug.LogError ("The part is not a car part");
		
		mForwardInput = mRotateInput = 0;		
		
	}

	// Update is called once per frame
	void Update() {
		
		if(this.isControllable){
			this.GetInput();
			this.OrbitRobot();
			this.Turn();
			
			if (Input.GetMouseButtonDown(0)) {
				this.goLarm.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
				this.goRarm.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// FixedUpdate is called 
	void FixedUpdate() {
		if(this.isControllable)
			this.Move();
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
			//			this.mTargetRotTorso = Quaternion.Euler(0, -this.mOrbit.mYRotation + Camera.main.transform.eulerAngles.y, 0);
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
		Vector3 g;
		if(this.isGrounded) {
			this.mVerticalVel = -this.mGravity * Time.deltaTime; 
			if(Input.GetButton("Jump") && this.isGrounded){
//				Debug.Log("Jump");
				this.mVerticalVel = ((Car)mParts[3]).GetJumpPower(); 
			}
		}else{
			this.mVerticalVel += mGravity * Time.deltaTime;
		}
		
		g = new Vector3(0, this.mVerticalVel, 0);

		
		if(Mathf.Abs(this.mForwardInput) > this.mInputDelay) {
			// Move the player
			this.mRigidbody.velocity = transform.forward * this.mForwardInput * ((Car)this.mParts[3]).GetSpeed();
		} else {
			this.mRigidbody.velocity = Vector3.zero;
		}
		
		this.mRigidbody.velocity += g;

	}
	
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == mGroundTag) {
			isGrounded = true;
		}
	}

	void OnCollisionExit(Collision col){
		if(col.gameObject.tag == mGroundTag) {
			isGrounded = false;
		}
	}
}
