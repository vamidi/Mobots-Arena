using UnityEngine;

namespace Mobots.Robot {
	[System.Serializable]
	public class Car : Part {
		/// <summary>
		/// The speed of the robot
		/// </summary>
		[SerializeField]
		private float mSpeed;

		/// <summary>
		/// The jump strengtj pf the robot
		/// </summary>
		private float mJumpStrength = 5f;

		public void SetSpeed(float speed) {
			if (mRobot != null) {
				mSpeed = speed / mRobot.Mass;
			}
		}

		public void SetJumpStrength(float strength) {
			if(mRobot != null)
				this.mJumpStrength = strength/mRobot.Mass;
		}

		public float Speed {
			get { return mSpeed; }
			set {
				if (mRobot == null)
					SearchParent();
				mSpeed = value / mRobot.Mass;
			}
		}

		public float JumpStrength {
			get { return mJumpStrength; }
			set { mJumpStrength = value; }
		}
	}
}