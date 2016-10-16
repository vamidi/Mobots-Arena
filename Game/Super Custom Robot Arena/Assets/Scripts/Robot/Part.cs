using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// The parts of the robot
/// </summary>
public enum PART {
	HEAD, CAR, LARM, RARM
}

public class Part : MonoBehaviour {

	/// <summary>
	/// This is needed to tell the robot which part this is.
	/// </summary>
	protected PART mPart = PART.HEAD;

	/// <summary>
	/// The model of the robot
	/// </summary>
	protected GameObject fbx = null;

	/// <summary>
	/// The texture of the robot
	/// </summary>
	protected Texture mTexture = null;

	/// <summary>
	/// The health of the robot
	/// </summary>
	protected float mHealth = 100f;

	/// <summary>
	/// The armor of the robot
	/// </summary>
	protected float mArmor = 100f;

	/// <summary>
	/// The strength that the armor has (0 to 100%)
	/// </summary>
	protected float mArmorStrength = 5;

	protected int mRobotWegith = 50;

	/// <summary>
	/// The weapon of the robot
	/// </summary>
	protected GameObject weapon = null;


	public PART getPart(){
		return this.mPart;
	}

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
}
