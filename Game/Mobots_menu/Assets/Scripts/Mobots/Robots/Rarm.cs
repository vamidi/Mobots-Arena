using UnityEngine;

namespace Mobots.Robots {

	// [RequireComponent(typeof(HealthBar))]
	public class Rarm : Arm {

		public override void Initialize() {
			// if (this.mHealthBar)
				// this.mHealthBar.Initialize();
			mCanFire = true;
			base.Initialize();
		}

		public override void Shoot() {
			base.Shoot();

			// right btn click
			if (mFire && mCanFire && Time.time > mNextFire) {
				mNextFire = Time.time + mRoundsPerSecond;

				mCurrentRecoilPos -= mRecoilAmount;

//			this.mOrbit.mXRotation += (UnityEngine.Random.value - 0.5f) * Mathf.Lerp(0f, 5f, 1f);
//			this.mOrbit.mYRotation += (UnityEngine.Random.value - 0.5f) * Mathf.Lerp(0f, 5f, 1f);

				if (mBullet) {
					GameObject bullet = (GameObject) Instantiate(mBullet, mGunEnd.position, mGunEnd.rotation);
					// bullet.GetComponent<Bullet>().mDamage = this.mDamagePerRound;
				}

				StartCoroutine(ShotEffect());

				Vector3 rayOrg = mGunEnd.position; // this.mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,  0));
				RaycastHit hit;

				mLaserLine.SetPosition(0, mGunEnd.position);

				if (Physics.Raycast(rayOrg, mGunEnd.transform.forward, out hit, mRange)) {
					mLaserLine.SetPosition(1, hit.point);
				} else {
					mLaserLine.SetPosition(1, rayOrg + (mGunEnd.transform.forward * mRange));
				}
			}
		}

		protected override void Awake() {
			base.Awake();
			mPart = PART.Rarm;
		}

		protected override void GetInput() {
			base.GetInput();
			mFire = Input.GetButtonDown(mInput.mFire2);
		}
	}
}