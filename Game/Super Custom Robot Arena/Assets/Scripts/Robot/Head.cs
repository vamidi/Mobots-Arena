using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Head : Part {
	
	public Image mCurrentShieldBar;
	public Text mRatioText;
	
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
		float ratio = this.mArmorHealth / this.mMaxArmorHealth;
		
		if(this.mCurrentShieldBar)
			this.mCurrentShieldBar.rectTransform.localScale = new Vector3(ratio , 1, 1);
		
		if(this.mRatioText)
			mRatioText.text = (ratio * 100 ).ToString("0") + "%";
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

		float damageOnHealth = ( (100f - this.Strenght) / 100f ) * d;

		this.mHealth -= damageOnHealth;
		this.ArmorHealth -= d;

		if(this.mHealthBar)
			this.mHealthBar.UpdateHealthBar();

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
	public override void ArmorHeal(int h){
		this.mArmorHealth += (this.mMaxHealth / h);
		this.UpdateShieldBar();
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
			this.UpdateShieldBar();
		}
		
		if(this.mArmorHealth > 100){
			this.mArmorHealth = 100f;
			this.UpdateShieldBar();
		}
	}
}
