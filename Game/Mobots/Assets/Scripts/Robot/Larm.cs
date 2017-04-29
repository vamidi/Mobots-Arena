using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(HealthBar))]
public class Larm : Arm { 
	
	public override void Initialize() {
		if(this.mHealthBar)
			this.mHealthBar.Initialize();
		this.mCanFire = true;
		base.Initialize();
	}
	
	public override void Shoot() {
		base.Shoot();
		
		// left btn click
		if (this.mFire && this.mCanFire && Time.time > this.mNextFire) {
			this.mNextFire = Time.time + this.mRoundsPerSecond;
			
			this.mCurrentRecoilPos -= this.mRecoilAmount;
			
//			this.mOrbit.mXRotation += (UnityEngine.Random.value - 0.5f) * Mathf.Lerp(0f, 5f, 1f);
//			this.mOrbit.mYRotation += (UnityEngine.Random.value - 0.5f) * Mathf.Lerp(0f, 5f, 1f);

			if (this.mBullet) {
				GameObject bullet = (GameObject)Instantiate (this.mBullet, this.mGunEnd.position, this.mGunEnd.rotation);
				bullet.GetComponent<Bullet> ().mDamage = this.mDamagePerRound;
			}
			
			StartCoroutine(this.ShotEffect());

			Vector3 rayOrg = this.mGunEnd.position; //this.mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,  0));
			RaycastHit hit;

			mLaserLine.SetPosition(0, this.mGunEnd.position);

			if(Physics.Raycast(rayOrg, this.mGunEnd.transform.forward, out hit, this.mRange)) {
				this.mLaserLine.SetPosition(1, hit.point);
			}else {
				this.mLaserLine.SetPosition(1, rayOrg + (mGunEnd.transform.forward * this.mRange));
			}
		}
	}
	
	protected override void Awake(){
		base.Awake();
		this.mPart = PART.LARM;
	}
	
	protected override void GetInput(){
		if(((Player)mRobot).isControllable){
			base.GetInput();
			this.mFire = Input.GetButtonDown(this.mInput.mFire);
		}
	}
}
