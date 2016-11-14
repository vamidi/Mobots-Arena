using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

using SCRA.Humanoids;

/// <summary>
/// The parts of the robot
/// </summary>
public enum PART {
	HEAD, CAR, LARM, RARM, UNASSIGNED
}

[RequireComponent(typeof(HealthBar))]
public abstract class Part : MonoBehaviour, IDamageable<float>, IHealable<int> {
	
	/// <summary>
	/// The max health of the robot
	/// </summary>
	public float mMaxHealth = 100f;
	/// <summary>
	/// The weight of the part
	/// </summary>
	public int mRobotWegith = 75;	
	public Color mDownColor = new Color(153f, 153f, 153f, 1f);
	protected Robot mRobot = null;
	/// <summary>
	/// This is needed to tell the robot which part this is.
	/// </summary>
	[SerializeField]
 	protected PART mPart = PART.HEAD;
	/// <summary>
	/// The model of the robot
	/// </summary>
	protected Material mMaterial = null;
	protected Material mFlashMaterial = null;
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
	/// The weapon of the robot
	/// </summary>
	protected GameObject weapon = null;
	
	protected HealthBar mHealthBar = null;
	
	public bool isFlashing = false;
	
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

		if(!this.isFlashing)
			StartCoroutine(Flash());
		
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
	}
	
	/// <summary>
	/// Heal the specified part.
	/// </summary>
	/// <param name="h">Health.</param>
	public void Heal(int h){
		this.mHealth += (this.mMaxHealth / h); // ex. h = 10% -> health += 100f / 10% = 10
	}

	public virtual void IncreaseDamage(int x){ /* 1.2x, 1.4x, 1.6x, 1.8x, 2.0x of all damage done */ }

	/// <summary>
	/// Heals the armor.
	/// </summary>
	/// <param name="h">Health.</param>
	public virtual void ArmorHeal(int h){ 
		Head tempHead = (Head) this.mRobot.GetPart(0);
		tempHead.ArmorHeal(h);
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
	protected virtual void Awake(){
		this.mRobot = this.transform.root.GetComponent<Player>();
	}	
	
	// Use this for initialization
	protected virtual void Start(){
		this.mHealth = this.mMaxHealth;
		this.mHealthBar = this.GetComponent<HealthBar>();
		this.mMaterial = this.GetComponent<Renderer>().material;
		this.mFlashMaterial = new Material(Shader.Find("Mobile/Particles/Additive"));
		float gray = 153f/255f;
		this.mDownColor = new Color(gray, gray, gray, 1f);
	}
	
	// Update is called once per frame
	protected virtual void Update(){
		if( this.mHealth < 0 ){
			this.mHealth = 0;
			this.GetComponent<Renderer>().material.color = this.mDownColor;
		}
		
		if(this.mHealth > 100){
			this.mHealth = 100f;
		}
	}
	
	protected IEnumerator Flash(){
		this.isFlashing = true;
		yield return new  WaitForSeconds(0.1F);
		gameObject.GetComponent<Renderer>().material = this.mFlashMaterial;
		yield return new  WaitForSeconds(0.1F);
		gameObject.GetComponent<Renderer>().material = this.mMaterial;
		yield return new  WaitForSeconds(0.1F);
		gameObject.GetComponent<Renderer>().material = this.mFlashMaterial;
		yield return new  WaitForSeconds(0.1F);
		gameObject.GetComponent<Renderer>().material = this.mMaterial;
		yield return new  WaitForSeconds(0.1F);
		gameObject.GetComponent<Renderer>().material = this.mFlashMaterial;
		yield return new  WaitForSeconds(0.1F);
		gameObject.GetComponent<Renderer>().material = this.mMaterial;
		yield return new  WaitForSeconds(0.1F);
		this.isFlashing = false;
	}
}