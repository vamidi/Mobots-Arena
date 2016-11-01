using UnityEngine;
using System.Collections;

public class Head : Part {
	
	/// <summary>
	/// The health of the armor
	/// </summary>
	[SerializeField]
	private float mArmorHealth = 100f;

	/// <summary>
	/// The strength that the armor has (0 to 100%)
	/// </summary>
	[SerializeField]
	private float mArmorStrength = 15f;
	
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
		this.mArmorHealth += (this.mArmorHealth / h);
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mPart = PART.HEAD;
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
	}
}
