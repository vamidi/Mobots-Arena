using UnityEngine;
using System.Collections;

[System.Serializable]
public class PositionSettings {
	public Vector3 mTargetPosOffset = new Vector3(0, 3.4f, 0);
	public float mLookSmooth = 500f;
	public float mDistanceFromTarget = -10f;
	public float mZoomSmooth = 10f;
	public float mMaxZoom = -2f;
	public float mMinZoom = -15f;
}

[System.Serializable]
public class OrbitSettings {
	public float mXRotation = -34f;
	public float mYRotation = 180f;
	public float mMaxXRotation = 25f;
	public float mMinXRotation = -85f;
	public float mVorbitSmooth = 150f;
	public float mHorbitSmooth = 150f;
}

[System.Serializable]
public class InputSettings {
	public string mOrbitHorizontalSnap = "OrbitHorizontalSnap";
	public string mZoom = "Mouse ScrollWheel";

	public string mVertical = "Vertical";
	public string mHorizontal = "Horizontal";
	public string mFire = "Fire1";
	public string mJump = "Jump";

	public string mMouseHorizontal = "Mouse X";
	public string mMouseVertical = "Mouse Y";
}

[System.Serializable]
public class PhysicSettings {
	public float mJumpVel = 25f;
	public float mDownAcc = 1.25f;
	public float mDistToGround = 0.1f;
}

public class CameraController : MonoBehaviour {

	/// <summary>
	/// The target for the camera
	/// </summary>
	public Transform mTarget;
	/// <summary>
	/// The Robot script
	/// </summary>
	public Robot mRobot;
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
	/// The target position
	/// </summary>
	private Vector3 mTargetPos = Vector3.zero;
	/// <summary>
	/// The destination of the camera
	/// </summary>
	private Vector3 mDestination = Vector3.zero;
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

	// Use this for initialization
	void Start () {
		SetCameraTarget(GameObject.FindGameObjectWithTag("Robot").transform);

		initRotation();

		if (!mTarget)
			Debug.LogError("You dont have a target for your camera");

		mRobot = (this.mTarget.GetComponent<Robot>()) ? this.mTarget.GetComponent<Robot>() : null;

		if(!mRobot)
			Debug.LogWarning("You dont have a robot script for your camera");
		
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

	// Update is called once per frame
	void Update () {
		if(mRobot != null && mRobot.isControllable){
			// Input
			GetInput();
			// Orbit the camera
			OrbitTarget();
			// Zoom function camera
			ZoomInOnTarget();
			// moving
			MoveToTarget();
			// rotating
			LookAtTarget();
		}
	}

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
		this.mDestination = Quaternion.Euler(this.mOrbit.mXRotation, this.mOrbit.mYRotation + this.mTarget.eulerAngles.y, 0) * -Vector3.forward * this.mPosition.mDistanceFromTarget;
		this.mDestination += this.mTarget.position;
		this.transform.position = this.mDestination;
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
			mOrbit.mYRotation = -180f;
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
}
