﻿using UnityEngine;
using UnityEngine.UI;

namespace Mobots.Robots {

	// [RequireComponent(typeof(HealthBar))]
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

		public override void Initialize() {
			if (!mCurrentShieldBar)
				mCurrentShieldBar = GameObject.FindGameObjectWithTag("ArmorUI").GetComponent<Image>();
			
			// TODO Reimplement this !!
			//if (this.mHealthBar)
			//	this.mHealthBar.Initialize();

			this.mMaxHealth = this.mArmorHealth;
		}

		public void UpdateShieldBar() {
			float ratio = Map(mArmorHealth, 0, mMaxArmorHealth, 0, 1);
			if (mCurrentShieldBar && mCurrentShieldBar.fillAmount != ratio) {
				mCurrentShieldBar.fillAmount = Mathf.Lerp(mCurrentShieldBar.fillAmount, ratio,
					Time.deltaTime * this.mColorLerpSpeed);
			}
//		if(this.mRatioText)
//			mRatioText.text = (ratio * 100 ).ToString("0") + "%";
		}

		/// <summary>
		/// Damage the specified part
		/// </summary>
		/// <param name="d">Full damage</param>
		public override void Damage(float d) {

			// Damgeperround = 20;
			// Shieldstrenght = 30;
			// DamageDone = ((100-Shieldstrenght)/100) * Damageperround
			// Damagedone = ((100-30)/100) * 20
			// Damagedone = 0.7 * 20
			// Damagedone = 14

			if (!this.isFlashing)
				StartCoroutine(Flash());

			float damageOnHealth = ((100f - this.Strenght) / 100f) * d;
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
		public void SetArmor(float armor) {
			this.mArmorHealth = this.mMaxArmorHealth = armor;
		}

		/// <summary>
		/// Sets the armor strength.
		/// </summary>
		/// <param name="strength">Strength.</param>
		public void SetStrength(float strength) {
			this.mArmorStrength = strength;
		}

		/// <summary>
		/// Heals the armor.
		/// </summary>
		/// <param name="h">Health.</param>
		public override void ArmorHeal(double h) {
			this.mHealth += (float) (this.mMaxHealth * h); // ex 265 * .1 == 10% = 39,75
		}

		protected override void Awake() {
			base.Awake();
			mPart = PART.Head;
		}

		// Use this for initialization
		protected override void Start() {
			base.Start();
			this.mArmorHealth = this.mMaxArmorHealth;
		}

		// Update is called once per frame
		protected override void Update() {
			base.Update();
			if (this.mArmorHealth < 0) {
				this.mArmorHealth = 0;
			}

			if (this.mArmorHealth > this.mMaxArmorHealth) {
				this.mArmorHealth = this.mMaxArmorHealth;
			}

			this.UpdateShieldBar();
		}

		private float Map(float value, float inMin, float inMax, float outMin, float outMax) {
			return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		}
	}
}