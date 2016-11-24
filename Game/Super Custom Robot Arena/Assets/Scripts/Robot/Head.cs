using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Head : Part {
	
	public Image mCurrentShieldBar;
	public float mColorLerpSpeed = 2f;
	
	/// <summary>
	/// The health of the armor
	/// </summary>
	[SerializeField]
	private float mArmorHealth = 100f;
	[SerializeField]
	private float mMaxArmorHealth = 100f;

	/// <summary>
	/// The strength that the armor has (0 to 100%)
	/// </summary>
	[SerializeField]
	private float mArmorStrength = 15f;
	
	public void UpdateShieldBar(){
		float ratio = Map( this.mArmorHealth, 0, this.mMaxArmorHealth, 0, 1);		
		if(this.mCurrentShieldBar && this.mCurrentShieldBar.fillAmount != ratio){
			this.mCurrentShieldBar.fillAmount = Mathf.Lerp(this.mCurrentShieldBar.fillAmount, ratio, Time.deltaTime * this.mColorLerpSpeed);
		}
//		if(this.mRatioText)
//			mRatioText.text = (ratio * 100 ).ToString("0") + "%";
	}
	
	/// <summary>
	/// Damage the specified part
	/// </summary>
	/// <param name="d">Full damage</param>
	public override void Damage(float d){

		// Damgeperround = 20;
		// Shieldstrenght = 30;
		// DamageDone = ((100-Shieldstrenght)/100) * Damageperround
		// Damagedone = ((100-30)/100) * 20
		// Damagedone = 0.7 * 20
		// Damagedone = 14

		if(!this.isFlashing)
			StartCoroutine(Flash());		
		
		float damageOnHealth = ( (100f - this.Strenght) / 100f ) * d;

		this.mHealth -= damageOnHealth;
		this.ArmorHealth -= d;

	}
	
	/// <summary>
	/// Gets the armor strenght.
	/// </summary>
	/// <value>The strenght.</value>
	public float Strenght {
		get { return this.mArmorStrength; }
	}
	
	/// <summary>
	/// Gets the armor health.
	/// </summary>
	/// <value>The health.</value>
	public float ArmorHealth {
		get { return this.mArmorHealth; }
		set { this.mArmorHealth = value; }
	}
	
	/// <summary>
	/// Sets the armor health.
	/// </summary>
	/// <param name="armor">Armor.</param>
	public void SetArmor(float armor){
		this.mArmorHealth = armor;
	}

	/// <summary>
	/// Sets the armor strength.
	/// </summary>
	/// <param name="strength">Strength.</param>
	public void SetStrength(float strength){
		this.mArmorStrength = strength;
	}
	
	/// <summary>
	/// Heals the armor.
	/// </summary>
	/// <param name="h">Health.</param>
	public override void ArmorHeal(double h){
		this.mHealth += (float)(this.mMaxHealth * h); // ex 265 * .1 == 10% = 39,75
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mPart = PART.HEAD;
		this.mArmorHealth = this.mMaxHealth;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if( this.mArmorHealth < 0 ){
			this.mArmorHealth = 0f;
		}
		
		if(this.mArmorHealth > 100){
			this.mArmorHealth = 100f;
		}
		
		this.UpdateShieldBar(); 
	}
	
	private float Map(float value, float inMin, float inMax, float outMin, float outMax){
		return ( value - inMin ) * ( outMax - outMin) / ( inMax - inMin ) + outMin;
	}
	
}
