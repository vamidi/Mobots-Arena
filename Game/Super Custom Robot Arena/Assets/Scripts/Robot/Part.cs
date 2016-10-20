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
	[SerializeField]
	protected float mHealth = 100f;

	/// <summary>
	/// The armor of the robot
	/// </summary>
	[SerializeField]
	protected float mArmor = 100f;

	/// <summary>
	/// The strength that the armor has (0 to 100%)
	/// </summary>
	[SerializeField]
	protected float mArmorStrength = 5f;

	[SerializeField]
	protected int mRobotWegith = 50;

	/// <summary>
	/// The weapon of the robot
	/// </summary>
	protected GameObject weapon = null;


	public PART GetPart(){
		return this.mPart;
	}

	public void SetHealth(float health){
		this.mHealth = health;
	}

	public void SetArmor(float armor){
		this.mArmor = armor;
	}

	public void SetStrength(float strength){
		this.mArmorStrength = strength;
	}

	public void SetWeight(int weight){
		this.mRobotWegith = weight;
	}

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
}
