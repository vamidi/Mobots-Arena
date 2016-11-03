using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// The parts of the robot
/// </summary>
public enum PART {
	HEAD, CAR, LARM, RARM, UNASSIGNED
}

public abstract class Part : MonoBehaviour, IDamageable<float>, IHealable<int> {
	
	/// <summary>
	/// The weight of the part
	/// </summary>
	public int mRobotWegith = 75;	
	
	protected Player mRobot = null;
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
	/// The max health of the robot
	/// </summary>
	[SerializeField]
	protected float mMaxHealth = 100f;
	/// <summary>
	/// The health of the robot
	/// </summary>
	[SerializeField]
	protected float mHealth = 100f;
	/// <summary>
	/// The weapon of the robot
	/// </summary>
	protected GameObject weapon = null;
	
	protected HealthBar mHealthBar = null;
	
	/// <summary>
	/// Returns the part
	/// </summary>
	/// <returns>The part.</returns>
	public PART GetPart(){
		return this.mPart;
	}

	/// <summary>
	/// Damage the specified part
	/// </summary>
	/// <param name="d">Full damage</param>
	public virtual void Damage(float d){
		
		// Damgeperround = 20;
		// Shieldstrenght = 30;
		// DamageDone = ((100-Shieldstrenght)/100) * Damageperround
		// Damagedone = ((100-30)/100) * 20
		// Damagedone = 0.7 * 20
		// Damagedone = 14
		
		// Get the Head part
		Head tempHead = (Head) this.mRobot.GetPart(0);
		float damageOnHealth;
		
		if(tempHead.ArmorHealth <= 0){
			damageOnHealth = d;
		}else {
			damageOnHealth = ( (100f - tempHead.Strenght) / 100f ) * d;
		}
				
		this.mHealth -= damageOnHealth;
		tempHead.ArmorHealth -= d;
		
		// Update the healthbars
		this.mHealthBar.UpdateHealthBar();
		tempHead.UpdateShieldBar();
		
	}
	
	/// <summary>
	/// Heal the specified part.
	/// </summary>
	/// <param name="h">Health.</param>
	public void Heal(int h){
		this.mHealth += (this.mMaxHealth / h); // ex. h = 10% -> health += 100f / 10% = 10		
		this.mHealthBar.UpdateHealthBar();
		
		// Get the Head part
		Head tempHead = (Head) this.mRobot.GetPart(0);
		tempHead.UpdateShieldBar();
	}
	
	/// <summary>
	/// Heals the armor.
	/// </summary>
	/// <param name="h">Health.</param>
	public virtual void ArmorHeal(int h){ 
		Head tempHead = (Head) this.mRobot.GetPart(0);
		tempHead.ArmorHeal(h);
		tempHead.UpdateShieldBar();
	}

	public float GetMaxHealth(){
		return this.mMaxHealth;
	}
	
	public float GetHealth(){
		return this.mHealth;
	}
	
	/// <summary>
	/// Sets the health.
	/// </summary>
	/// <param name="health">Health.</param>
	public void SetHealth(float health){
		this.mHealth = health;
	}

	/// <summary>
	/// Sets the weight.
	/// </summary>
	/// <param name="weight">Weight.</param>
	public void SetWeight(int weight){
		this.mRobotWegith = weight;
	}
	
	// Called before the Start function
	protected void Awake(){
		this.mRobot = GameObject.FindObjectOfType<Player>();
	}	
	
	// Use this for initialization
	protected virtual void Start(){
		this.mHealth = this.mMaxHealth = 100f;
		this.mHealthBar = this.GetComponent<HealthBar>();

	}
	
	// Update is called once per frame
	protected virtual void Update(){
		if( this.mHealth < 0 ){
			this.mHealth = 0;
			this.mHealthBar.UpdateHealthBar();
		}
		
		if(this.mHealth > 100){
			this.mHealth = 100f;
			this.mHealthBar.UpdateHealthBar();
		}
	}
	
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="col">Col.</param>
	void OnTriggerEnter(Collider col){
		if(col.tag == "Bullet") {
			this.gameObject.SendMessage("Damage", 20f, SendMessageOptions.DontRequireReceiver);
		}
	}
}