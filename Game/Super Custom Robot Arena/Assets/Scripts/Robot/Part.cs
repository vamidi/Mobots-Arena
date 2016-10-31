using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// The parts of the robot
/// </summary>
public enum PART {
	HEAD, CAR, LARM, RARM, UNASSIGNED
}

public class Part : MonoBehaviour {
	
	/// <summary>
	/// The weight of the part
	/// </summary>
	public int mRobotWegith = 75;	
	
	protected Robot mRobot = null;
	
	/// <summary>
	/// This is needed to tell the robot which part this is.
	/// </summary>
	[SerializeField]
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
	/// The health of the armor
	/// </summary>
	[SerializeField]
	private float mArmorHealth = 100f;
	
	/// <summary>
	/// The strength that the armor has (0 to 100%)
	/// </summary>
	[SerializeField]
	private float mArmorStrength = 5f;

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

	public void SetWeight(int weight){
		this.mRobotWegith = weight;
	}
	
	public void SetArmor(float armor){
		this.mArmorHealth = armor;
	}
	
	public void SetStrength(float strength){
		this.mArmorStrength = strength;
	}
	
	void Awake(){
		this.mRobot = GameObject.FindObjectOfType<Robot>();
	}
}