using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobots.Robot {
	[System.Serializable]
	public class Head : Part {
		/// <summary>
		///
		/// </summary>
		private float mShieldHitPoints = 100f;

		private float mMaxShieldHitPoints = 100f;
		/// <summary>
		///
		/// </summary>
		private float mShieldStrength = 1f;

		/// <summary>
		/// Sets the armor health.
		/// </summary>
		/// <param name="armor">Armor.</param>
		public void SetArmor(float armor) {
			mShieldHitPoints = mMaxShieldHitPoints = armor;
		}

		/// <summary>
		/// Sets the armor strength.
		/// </summary>
		/// <param name="strength">Strength.</param>
		public void SetStrength(float strength){
			this.mShieldStrength = strength;
		}

		public float ShieldHitPoints {
			get { return mShieldHitPoints; }
			set { mShieldHitPoints = value; }
		}

		public float ShieldStrength {
			get { return mShieldStrength; }
			set { mShieldStrength = value; }
		}
	}
}