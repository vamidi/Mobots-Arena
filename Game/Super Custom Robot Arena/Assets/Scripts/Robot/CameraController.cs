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

	public string mMouseHorizontal = "Mouse X";
	public string mMouseVertical = "Mouse Y";
}

public class CameraController : MonoBehaviour {

	public Transform mTarget;
	public Robot mRobot;
	public GameObject mTorso;

	public PositionSettings mPosition = new PositionSettings();
	public OrbitSettings mOrbit = new OrbitSettings();
	public InputSettings mInput = new InputSettings();

	private Vector3 mTargetPos = Vector3.zero;
	private Vector3 mDestination = Vector3.zero;
	private float mMouseInputVertical, mMouseInputHorizontal, mZoomInput, mOrbitSnapInput;

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

	void initRotation() {
		mTargetPos = mTarget.position + mPosition.mTargetPosOffset;
		mDestination = Quaternion.Euler(mOrbit.mXRotation, mOrbit.mYRotation + mTarget.eulerAngles.y, 0) * -Vector3.forward * mPosition.mDistanceFromTarget;
		mDestination += mTarget.position;
		transform.position = mDestination;
	}

	// Update is called once per frame
	void Update () {
		if(mRobot != null && mRobot.isControllable){
			GetInput();
			OrbitTarget();
			ZoomInOnTarget();
		}
	}

	void LateUpdate() {
		if(mRobot != null && mRobot.isControllable){
			// moving
			MoveToTarget();
			// rotating
			LookAtTarget();
		}
	}

	void GetInput() {
		mOrbitSnapInput = Input.GetAxisRaw(mInput.mOrbitHorizontalSnap);
		mZoomInput = Input.GetAxisRaw(mInput.mZoom);
		// arms movement
		this.mMouseInputVertical = Input.GetAxisRaw(this.mInput.mMouseVertical);
		this.mMouseInputHorizontal = Input.GetAxisRaw(this.mInput.mMouseHorizontal);
	}

	void MoveToTarget() {
		mTargetPos = mTarget.position + mPosition.mTargetPosOffset;
		mDestination = Quaternion.Euler(mOrbit.mXRotation, mOrbit.mYRotation + mTarget.eulerAngles.y, 0) * -Vector3.forward * mPosition.mDistanceFromTarget;
		mDestination += mTarget.position;
		transform.position = mDestination;
	}

	void LookAtTarget() {
		Quaternion targetRotation = Quaternion.LookRotation(mTargetPos - transform.position);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, mPosition.mLookSmooth * Time.deltaTime);

	}

	void OrbitTarget() {
		if(mOrbitSnapInput > 0) {
			mOrbit.mYRotation = -180f;
		}

		mOrbit.mXRotation += -mMouseInputVertical * mOrbit.mVorbitSmooth * Time.deltaTime;
		mOrbit.mYRotation += mMouseInputHorizontal * mOrbit.mHorbitSmooth * Time.deltaTime;

		
		// cap the orbiting
		if(mOrbit.mXRotation > mOrbit.mMaxXRotation) {
			mOrbit.mXRotation = mOrbit.mMaxXRotation;
		}

		if (mOrbit.mXRotation < mOrbit.mMinXRotation) {
			mOrbit.mXRotation = mOrbit.mMinXRotation;
		}

	}

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
