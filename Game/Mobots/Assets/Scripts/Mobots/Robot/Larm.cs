using UnityEngine;

namespace Mobots.Robot {
	[System.Serializable]
	public class Larm : Arm {

		public override void Shoot() {
			base.Shoot();
			// left btn click
			if (this.mFire && this.mCanFire && Time.time > this.mNextFire) {
				this.mNextFire = Time.time + this.mRoundsPerSecond;

				this.mCurrentRecoilPos -= this.mRecoilAmount;

				if (this.mBullet) {
					GameObject bullet = (GameObject)Instantiate (this.mBullet, this.mGunEnd.position, this.mGunEnd.rotation);
					Bullet b = bullet.GetComponent<Bullet>();
					b.mDamage = mDamagePerRound;
					b.parent = gameObject;
				}

				StartCoroutine(this.ShotEffect());

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
			if(mRobot.isControllable) {
				base.GetInput();
				this.mFire = Input.GetButtonDown(this.mInput.mFire);
			}
		}
    }
}
