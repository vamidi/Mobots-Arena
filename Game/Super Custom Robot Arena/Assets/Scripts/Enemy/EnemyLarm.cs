﻿using UnityEngine;
using System.Collections;
using System;

public class EnemyLarm : Arm { 
	
	public override void Shoot(){
		// left btn click
		if (this.mFire && Time.time > this.mNextFire) {
			this.mNextFire = Time.time + this.mRoundsPerSecond;
			
			this.mCurrentRecoilPos -= this.mRecoilAmount;
			
//			this.mOrbit.mXRotation += (UnityEngine.Random.value - 0.5f) * Mathf.Lerp(0f, 5f, 1f);
//			this.mOrbit.mYRotation += (UnityEngine.Random.value - 0.5f) * Mathf.Lerp(0f, 5f, 1f);

			if(this.mBullet) 
				Instantiate(this.mBullet, this.mGunEnd.position, this.mGunEnd.rotation);
			
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
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mPart = PART.LARM;
	}
	
	// Update is called once per frame
	protected override void Update () {
//		base.Update();
	}
	
	protected override void LateUpdate () {
//		base.LateUpdate();
	}
	
	protected override void GetInput(){
	}
	
}
