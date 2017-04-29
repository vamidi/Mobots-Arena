using System;
using UnityEngine;

namespace Mobots.Robot {
	[Serializable]
	public class Rarm : Arm {
		public override void Shoot () {
			base.Shoot();

			// right btn click
			if (this.mFire && this.mCanFire && Time.time > this.mNextFire) {
				mNextFire = Time.time + mRoundsPerSecond;
				mCurrentRecoilPos -= mRecoilAmount;
				if (this.mBullet) {
					GameObject bullet = Instantiate (this.mBullet, this.mGunEnd.position, this.mGunEnd.rotation);
					Bullet b = bullet.GetComponent<Bullet>();
					b.mDamage = mDamagePerRound;
					b.parent = gameObject;
				}

				StartCoroutine(ShotEffect());

				Vector3 rayOrg = this.mGunEnd.position; //this.mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,  0));
				RaycastHit hit;

				if (mLaserLine) {
					mLaserLine.SetPosition(0, this.mGunEnd.position);

					if (Physics.Raycast(rayOrg, this.mGunEnd.transform.forward, out hit, this.mRange)) {
						this.mLaserLine.SetPosition(1, hit.point);
					} else {
						this.mLaserLine.SetPosition(1, rayOrg + (mGunEnd.transform.forward * this.mRange));
					}
				}
			}
		}

		protected override void GetInput() {
			base.GetInput();
			this.mFire = Input.GetButtonDown(this.mInput.mFire2);
		}
	}
}
