using UnityEngine;

namespace Mobots.Robots {
	// [RequireComponent(typeof(HealthBar))]
	public class Car : Part {

		/// <summary>
		/// The speed of the robot
		/// </summary>
		[SerializeField]
		private float mSpeed;

		/// <summary>
		/// The jump strengtj pf the robot
		/// </summary>
		private float mJumpForce = 5f;

		public override void Initialize() {
			// if (this.mHealthBar)
				// this.mHealthBar.Initialize();
		}

		protected override void Awake() {
			base.Awake();
			mPart = PART.Car;
		}

		public float GetSpeed() {
			return mSpeed;
		}

		public float GetJumpPower() {
			return this.mJumpForce;
		}

		public void SetSpeed(float speed) {
			speed = 2800f;
			if (mRobot != null)
				mSpeed = speed / mRobot.GetRobotMass();
		}

		public void SetJumpStrength(float strength) {
			if (mRobot != null)
				mJumpForce = strength / mRobot.GetRobotMass();
		}
	}
}