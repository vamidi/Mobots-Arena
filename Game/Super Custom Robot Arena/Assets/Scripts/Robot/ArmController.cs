using UnityEngine;
using System.Collections;
// Own namespace
using SCRA.Humanoids;

public class ArmController : MonoBehaviour {
	
	private Player mRobot;
	private Transform mLeftArm, mRightArm;
	[SerializeField]
	private InputSettings mInput = new InputSettings();
	[SerializeField]
	private OrbitSettings mOrbit = new OrbitSettings();
	private float mMouseVertical, currentXrotation;
	private float dampVel = 0.1f;
	
	public float GetXRotation () {
		return this.mOrbit.mXRotation;
	}

	// Use this for initialization
	void Start () {
		this.mRobot = this.transform.root.GetComponent<Player>();
		if(this.mRobot){
			this.mLeftArm = this.mRobot.GetPart(1).transform;
			this.mRightArm = this.mRobot.GetPart(2).transform;
		}
			
		this.mOrbit.mVorbitSmooth = 5f;
		this.mOrbit.mMinXRotation = -30f;
		this.mOrbit.mMaxXRotation = 30f;
	}
	
	// Update is called once per frame
	void Update () {
		if(mRobot.isControllable){
			this.GetInput();
		}
	}
		
	protected virtual void LateUpdate(){
		if(mRobot.isControllable){
			this.Turn();
		}		
	}
	
	/// <summary>
	/// Is this method we get the input of the player
	/// </summary>
	void GetInput() {
		this.mMouseVertical = Input.GetAxisRaw(this.mInput.mMouseVertical);
	}
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	void Turn() {     
		this.mOrbit.mXRotation += -this.mMouseVertical * mOrbit.mVorbitSmooth;
		this.mOrbit.mXRotation = Mathf.Clamp(this.mOrbit.mXRotation, this.mOrbit.mMinXRotation, this.mOrbit.mMaxXRotation);
		this.currentXrotation = Mathf.Lerp(this.currentXrotation, this.mOrbit.mXRotation, dampVel);
		this.mLeftArm.localRotation = Quaternion.Euler(Camera.main.transform.rotation.x + this.mOrbit.mXRotation, mOrbit.mYRotation, 0);
		this.mRightArm.localRotation = Quaternion.Euler(Camera.main.transform.rotation.x + this.mOrbit.mXRotation, -mOrbit.mYRotation, 0);
	}
}
