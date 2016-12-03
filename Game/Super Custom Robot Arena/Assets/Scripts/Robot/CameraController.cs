using UnityEngine;
using System.Collections;

[System.Serializable]
public class PositionSettings {
	public Vector3 mTargetPosOffset = new Vector3(0, 3.4f, 0);
	public float mLookSmooth = 500f;
	public float mDistanceFromTarget = -7f;
	public float mZoomSmooth = 10f;
	public float mMaxZoom = -2f;
	public float mMinZoom = -15f;
	public bool mSmoothFollow = true;
	public float mSmooth = 0.05f;
	
	[HideInInspector]
	public float newDistance = -6f; // set by the zoom input
	[HideInInspector]
	public float mAdjustmentDistance = -6f;
}

[System.Serializable]
public class DebugSettings {
	public bool mDrawDesiredCollisionLines = true;
	public bool mDrawAdjustedCollisionlines = true;
}

[System.Serializable]
public class OrbitSettings {
	public float mXRotation = -34f;
	public float mYRotation = 180f;
	public float mMaxXRotation = -34f;
	public float mMinXRotation = -65f;
	public float mVorbitSmooth = 50f;
	public float mHorbitSmooth = 150f;
}

[System.Serializable]
public class InputSettings {
	public string mOrbitHorizontalSnap = "OrbitHorizontalSnap";
	public string mZoom = "Mouse ScrollWheel";

	public string mVertical = "Vertical";
	public string mHorizontal = "Horizontal";
	public string mFire = "Fire1";
	public string mFire2 = "Fire2";
	public string mJump = "Jump";

	public string mMouseHorizontal = "Mouse X";
	public string mMouseVertical = "Mouse Y";
}

[System.Serializable]
public class PhysicSettings {
	public float mJumpVel = 14f;
	public float mDownAcc = 40f;
	/// <summary>
	/// The ground.
	/// </summary>
	public LayerMask Ground;
}

[System.Serializable]
public class TagSettings {
	/// <summary>
	/// Tag for the head
	/// </summary>
	public string mHeadTag = "Head";                                                        
	/// <summary>
	/// Tag for the left arm
	/// </summary>
	public string mLarmTag = "Left";                                                   
	/// <summary>
	/// Tag for the right arm
	/// </summary>
	public string mRamTag = "Right";                                            
	/// <summary>
	/// Tag for the car
	/// </summary>
	public string mCarTag = "Car";                                                         
	/// <summary>
	/// Tag for the floor
	/// </summary>
	public string mGroundTag = "Ground";
	/// <summary>
	/// The robot tag.
	/// </summary>
	public string mRobotTag = "Robot";
	/// <summary>
	/// The enemy tag.
	/// </summary>
	public string mEnemyTag = "Enemy";
	/// <summary>
	/// The head UI tag
	/// </summary>
	public string mHeadUI = "HeadUI";
	/// <summary>
	/// The right UI tag
	/// </summary>
	public string mRightUI = "RightUI";
	/// <summary>
	/// The left UI tag
	/// </summary>
	public string mLeftUI = "LeftUI";
	/// <summary>
	/// Thecar UI tag
	/// </summary>
	public string mCarUI = "CarUI";
}

public class CameraController : MonoBehaviour {

	/// <summary>
	/// The target for the camera
	/// </summary>
	public Transform mTarget;
	/// <summary>
	/// The Robot script
	/// </summary>
	public Player mRobot;
	/// <summary>
	/// The position settings
	/// </summary>
	public PositionSettings mPosition = new PositionSettings();
	/// <summary>
	/// The orbit settings
	/// </summary>
	public OrbitSettings mOrbit = new OrbitSettings();
	/// <summary>
	/// The input settings
	/// </summary>
	public InputSettings mInput = new InputSettings();
	/// <summary>
	/// The m debug.
	/// </summary>
	public DebugSettings mDebug = new DebugSettings();
	/// <summary>
	/// The m collision.
	/// </summary>
	public CollisionHandler mCollision = new CollisionHandler();

	/// <summary>
	/// The target position
	/// </summary>
	private Vector3 mTargetPos = Vector3.zero;
	/// <summary>
	/// The destination of the camera
	/// </summary>
	private Vector3 mDestination = Vector3.zero;
	/// <summary>
	/// For when we are colliding
	/// the adjusted destination
	/// </summary>
	private Vector3 mAdjustedDestination = Vector3.zero;
	/// <summary>
	/// This is for when we are smoothing the camera
	/// to the player target ( closer movement )
	/// </summary>
	private Vector3 mCamVel = Vector3.zero;
	/// <summary>
	/// Input variables
	/// </summary>
	private float mMouseInputVertical, mMouseInputHorizontal, mZoomInput, mOrbitSnapInput;

	/// <summary>
	/// Sets the camera target.
	/// </summary>
	/// <param name="t">T.</param>
	public void SetCameraTarget(Transform t) {
		mTarget = t;
	}

	/// <summary>
	/// Inits the rotation.
	/// </summary>
	void initRotation() {
		mTargetPos = mTarget.position + mPosition.mTargetPosOffset;
		mDestination = Quaternion.Euler(mOrbit.mXRotation, mOrbit.mYRotation + mTarget.eulerAngles.y, 0) * -Vector3.forward * mPosition.mDistanceFromTarget;
		mDestination += mTarget.position;
		transform.position = mDestination;
	}
	
	#region UNITYMETHODS
	
	// Use this for initialization
	void Start () {
		this.SetCameraTarget(GameObject.FindGameObjectWithTag("Robot").transform);
		if (!mTarget)
			Debug.LogError("You dont have a target for your camera");
		this.initRotation();
		
		mRobot = (this.mTarget.GetComponent<Player>()) ? this.mTarget.GetComponent<Player>() : null;
		if(!mRobot)
			Debug.LogWarning("You dont have a robot script for your camera");
		
		this.mCollision.Initialize(this.GetComponent<Camera>());
		this.mCollision.UpdateCameraClipPoints(this.transform.position, this.transform.rotation, ref this.mCollision.mAdjustedCameraClipPoints);		
		this.mCollision.UpdateCameraClipPoints(this.mDestination, this.transform.rotation, ref this.mCollision.mDesiredCameraClipPoints);
	}

	// Update is called once per frame
	void Update () {
		if(this.mRobot != null && this.mRobot.isControllable){
			// Input
			this.GetInput();
			// Zoom function camera
			this.ZoomInOnTarget();

		}
	}
	
	void FixedUpdate(){
		if(mRobot != null && mRobot.isControllable){
			// Orbit the camera
			OrbitTarget();
			// moving
			MoveToTarget();
			// rotating
			LookAtTarget();
			
			this.mCollision.UpdateCameraClipPoints(this.transform.position, this.transform.rotation, ref this.mCollision.mAdjustedCameraClipPoints);		
			this.mCollision.UpdateCameraClipPoints(this.mDestination, this.transform.rotation, ref this.mCollision.mDesiredCameraClipPoints);
			
			// Draw the debuglines
			for(int i = 0; i < 5; i++) {
				if(mDebug.mDrawDesiredCollisionLines){
					Debug.DrawLine(this.mTargetPos, this.mCollision.mDesiredCameraClipPoints[i], Color.white);
				} 
				if(mDebug.mDrawAdjustedCollisionlines){
					Debug.DrawLine(this.mTargetPos, this.mCollision.mAdjustedCameraClipPoints[i], Color.green);
				}				 
			}
			
			// Use raycast here
			this.mCollision.CheckColliding(this.mTargetPos);
			this.mPosition.mAdjustmentDistance = this.mCollision.GetAdjustedDistanceWithRayFrom(this.mTargetPos);
		}
	}

	#endregion
	
	/// <summary>
	/// Gets the input.
	/// </summary>
	void GetInput() {
		this.mOrbitSnapInput = Input.GetAxisRaw(this.mInput.mOrbitHorizontalSnap);
		this.mZoomInput = Input.GetAxisRaw(this.mInput.mZoom);
		// arms movement
		this.mMouseInputVertical = Input.GetAxisRaw(this.mInput.mMouseVertical);
		this.mMouseInputHorizontal = Input.GetAxisRaw(this.mInput.mMouseHorizontal);
	}

	/// <summary>
	/// Moves to target.
	/// </summary>
	void MoveToTarget() {
		this.mTargetPos = this.mTarget.position + this.mPosition.mTargetPosOffset;
		this.mDestination = Quaternion.Euler(this.mOrbit.mXRotation, this.mOrbit.mYRotation, 0) * -Vector3.forward * this.mPosition.mDistanceFromTarget;
		this.mDestination += this.mTarget.position;
		
		if(this.mCollision.mColliding){
			this.mAdjustedDestination = Quaternion.Euler(this.mOrbit.mXRotation, this.mOrbit.mYRotation, 0) * Vector3.forward * this.mPosition.mAdjustmentDistance;
			this.mAdjustedDestination += this.mTargetPos;
			if(this.mPosition.mSmoothFollow){
				// Use smoothdamp function
				this.transform.position = Vector3.SmoothDamp(this.transform.position, this.mAdjustedDestination, ref this.mCamVel, this.mPosition.mSmooth);
			}else{
				this.transform.position = this.mAdjustedDestination;
			}
		}else{
			if(this.mPosition.mSmoothFollow){
				// Use smoothdamp function
				this.transform.position = Vector3.SmoothDamp(this.transform.position, this.mDestination, ref this.mCamVel, this.mPosition.mSmooth);
			}else{
				this.transform.position = this.mDestination;
			}			
		}
	}

	/// <summary>
	/// Looks at target.
	/// </summary>
	void LookAtTarget() {
		Quaternion targetRotation = Quaternion.LookRotation(mTargetPos - transform.position);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, mPosition.mLookSmooth * Time.deltaTime);

	}

	/// <summary>
	/// Orbits to the target.
	/// </summary>
	void OrbitTarget() {
		if(mOrbitSnapInput > 0) {
			mOrbit.mYRotation = 0f;
		}

		mOrbit.mXRotation += mMouseInputVertical * mOrbit.mVorbitSmooth * Time.deltaTime;
		mOrbit.mYRotation += mMouseInputHorizontal * mOrbit.mHorbitSmooth * Time.deltaTime;

		// get localrotation of the y and store it in the arms
		// cap the orbiting
		if(mOrbit.mXRotation > mOrbit.mMaxXRotation) {
			mOrbit.mXRotation = mOrbit.mMaxXRotation;
		}

		if (mOrbit.mXRotation < mOrbit.mMinXRotation) {
			mOrbit.mXRotation = mOrbit.mMinXRotation;
		}

	}

	/// <summary>
	/// Zooms in on target.
	/// </summary>
	void ZoomInOnTarget() {
		mPosition.mDistanceFromTarget += mZoomInput * mPosition.mZoomSmooth * Time.deltaTime;

		if(mPosition.mDistanceFromTarget > mPosition.mMaxZoom) {
			mPosition.mDistanceFromTarget = mPosition.mMaxZoom;
		}

		if (mPosition.mDistanceFromTarget < mPosition.mMinZoom) {
			mPosition.mDistanceFromTarget = mPosition.mMinZoom;
		}
	}

	[System.Serializable]
	public class CollisionHandler {
		public LayerMask mCollisionLayer;
		[HideInInspector]
 		public bool mColliding = false;
		[HideInInspector]
		public Vector3[] mAdjustedCameraClipPoints; 
		[HideInInspector]
		public Vector3[] mDesiredCameraClipPoints;
		Camera mCamera;
		
		public void Initialize(Camera cam){
			this.mCamera = cam;
			this.mAdjustedCameraClipPoints = this.mDesiredCameraClipPoints = new Vector3[5];
			
		}
		
		/// <summary>
		/// Filling the clippoint array
		/// because the camera is going to move
		/// and it needs te be update every frame
		/// </summary>
		/// <param name="camPosition">Cam position.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="intArray">Int array.</param>
		public void UpdateCameraClipPoints (Vector3 camPosition, Quaternion atRotation, ref Vector3[] intoArray) {
			if(!this.mCamera)
				return;
			
			// clear the contents of intoarray
			intoArray = new Vector3[5];
			float z = this.mCamera.nearClipPlane;
			// play with the 3.41f;
			float x = Mathf.Tan(this.mCamera.fieldOfView / 3.41f) * z;
			float y = x / this.mCamera.aspect;
			
			// find the points in the camera clip planes
			// top left
			intoArray[0] = (atRotation * new Vector3(-x, y, z)) + camPosition; // Added and rotated the point relative to the camera
			// top right
			intoArray[1] = (atRotation * new Vector3(x, y, z)) + camPosition; // Added and rotated the point relative to the camera
			// bottom left
			intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + camPosition; // Added and rotated the point relative to the camera
			// bottom right
			intoArray[3] = (atRotation * new Vector3(x, -y, z)) + camPosition; // Added and rotated the point relative to the camera
			// Camera position
			intoArray[4] = camPosition - this.mCamera.transform.forward;
		}
		
		/// <summary>
		/// Return the distance that our camera needs
		/// to be from our target
		/// and its finds a new position and returns it
		/// </summary>
		/// <returns>The adjusted distance with ray from.</returns>
		public float GetAdjustedDistanceWithRayFrom (Vector3 from) {
			float distance = -1;
			for(int i = 0; i < this.mDesiredCameraClipPoints.Length; i++){
				// direction towards 
				Ray ray = new Ray(from, this.mDesiredCameraClipPoints[i] - from);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit)){
					if(distance == -1)
						distance = hit.distance;
					else {
						if(hit.distance < distance)
							distance = hit.distance;
					}	
				}
			}
			if(distance == -1)
				return 0f;
			else
				return distance;
		}
		
		public void CheckColliding (Vector3 targetPosition) {
			if(this.CollisionDetectedAtClipPoints(this.mDesiredCameraClipPoints, targetPosition)){
				this.mColliding = true;
			}else{
				this.mColliding = false;
			}			
		}
		
		bool CollisionDetectedAtClipPoints (Vector3[] clipPoints, Vector3 fromPosition) {
			for(int i = 0; i < clipPoints.Length; i++){
				// cast a ray in the fromposition towards the clippoint
				Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
				float distance = Vector3.Distance(clipPoints[i], fromPosition);
				// if it run into our collisionlayer then true
				if(Physics.Raycast(ray, distance, this.mCollisionLayer)){
					return true;
				}
			}
			return false;
		}
	}
}
