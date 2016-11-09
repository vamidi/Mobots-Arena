using UnityEngine;
using System.Collections;

public class EnemyRarm : Arm {

	public override void Shoot(){
		this.mNextFire = Time.time + this.mRoundsPerSecond;
		
		this.mCurrentRecoilPos -= this.mRecoilAmount;

//			this.mOrbit.mXRotation += (UnityEngine.Random.value - 0.5f) * Mathf.Lerp(0f, 5f, 1f);
//			this.mOrbit.mYRotation += (UnityEngine.Random.value - 0.5f) * Mathf.Lerp(0f, 5f, 1f);
		
		if(this.mBullet){
			var direction = this.transform.root.GetComponent<Turret>().target.position - this.mGunEnd.position;
			direction.y = this.transform.root.GetComponent<Turret>().target.position.y;
			Instantiate(this.mBullet, this.mGunEnd.position, Quaternion.LookRotation(direction));
		}
		
		
		StartCoroutine(this.ShotEffect());

		Vector3 rayOrg = this.mGunEnd.position; //this.mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,  0));
		RaycastHit hit;

		mLaserLine.SetPosition(0, this.mGunEnd.position);

		if(Physics.Raycast(rayOrg, this.transform.root.GetComponent<Turret>().target.position, out hit, this.mRange)) {
			this.mLaserLine.SetPosition(1, hit.point);				
		}else {
			this.mLaserLine.SetPosition(1, rayOrg + (mGunEnd.transform.forward * this.mRange));
		}
	}
	
	/// <summary>
	/// Damage the specified part
	/// </summary>
	/// <param name="d">Full damage</param>
	public override void Damage(float d){


		// Damgeperround = 20;
		// Shieldstrenght = 30;
		// DamageDone = ((100-Shieldstrenght)/100) * Damageperround
		// Damagedone = ((100-30)/100) * 20
		// Damagedone = 0.7 * 20
		// Damagedone = 14

		// Get the Head part
		EnemyHead tempHead = (EnemyHead) this.mRobot.GetPart(0);
		float damageOnHealth;

		if(tempHead.ArmorHealth <= 0){
			damageOnHealth = d;
		}else {
			damageOnHealth = ( (100f - tempHead.Strenght) / 100f ) * d;
		}

		this.mHealth -= damageOnHealth;
		tempHead.ArmorHealth -= d;

		// Update the healthbars
		if(this.mHealthBar)
			this.mHealthBar.UpdateHealthBar();
		tempHead.UpdateShieldBar();

	}
	
	// Called before the Start function
	protected override void Awake(){
		this.mRobot = this.transform.root.GetComponent<Enemy>();
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mPart = PART.RARM;
	}
	
	// Update is called once per frame
	protected override void Update () {
		if( this.mHealth < 0 ){
			this.mHealth = 0f;
			if(this.mHealthBar)
				this.mHealthBar.UpdateHealthBar();
		}

		if(this.mHealth > 100){
			this.mHealth = 100f;
			if(this.mHealthBar)
				this.mHealthBar.UpdateHealthBar();
		}
	}

	protected override void LateUpdate () {
//		base.LateUpdate();
	}
	
	protected override void GetInput(){

	}
}
