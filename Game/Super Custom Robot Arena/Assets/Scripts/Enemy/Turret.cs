using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Turret : MonoBehaviour {

	public Image mCurrentHealthBar;
	public Text mRatioText;
	public float mHealth;
	public float mMaxHealth;
	public float mOldHealth;
	public float mColorLerpSpeed = .2f;
	public Part larm, rarm;
	public STATES mState = STATES.PATROL;
	public FieldOfView fov;
	public float mRotateVel;
	public bool hasTarget;
	public Transform target;
	public float time,lTime;
	public float resetTime;
	
	public float mMass;
	public float mResetMass;
	public Part[] mParts = new Part[4];
	public Color[] mColorArr = new Color[2];
	
	public GameObject goHead, goLarm, goRarm, goCar;
	public TagSettings mTags = new TagSettings();
	
	public Part GetPart(int index){
		return this.mParts[index];
	}
	
	void Awake(){

		foreach( Transform child in this.transform){
			if (child.gameObject.tag == this.mTags.mCarTag) {
				this.goCar = child.gameObject;
			}
			if(child.childCount > 0) {
				foreach( Transform nodeChild in child){
					if (nodeChild.gameObject.tag == this.mTags.mHeadTag) {
						this.goHead = nodeChild.gameObject;
					}
					foreach (Transform innerChild in nodeChild) {
						if (innerChild.gameObject.tag == this.mTags.mLarmTag) {
							this.goLarm = innerChild.gameObject;
						}else if(innerChild.gameObject.tag == this.mTags.mRamTag){
							this.goRarm = innerChild.gameObject;
						}
					}
				}
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		this.time = this.lTime = resetTime;
		this.fov = this.GetComponent<FieldOfView>();
		
		this.mParts [0] = this.goHead.GetComponent<EnemyHead> ();
		this.mParts [1] = this.goLarm.GetComponent<EnemyLarm> ();
		this.mParts [2] = this.goRarm.GetComponent<EnemyRarm> ();
		this.mParts [3] = this.goCar.GetComponent<EnemyCar> ();

		if (this.mParts [0].GetPart () != PART.HEAD)
			Debug.LogError ("The part is not a head part");

		if (this.mParts [1].GetPart () != PART.LARM)
			Debug.LogError ("The part is not a left arm part");

		if (this.mParts [2].GetPart () != PART.RARM)
			Debug.LogError ("The part is not a right arm part");

		if (this.mParts [3].GetPart () != PART.CAR)
			Debug.LogError ("The part is not a car part");

		this.mResetMass = this.mMass = this.mParts [0].mRobotWegith + this.mParts [1].mRobotWegith + this.mParts [2].mRobotWegith + this.mParts [3].mRobotWegith;
//		((EnemyCar)this.mParts[3]).SetSpeed(1825);
		
		this.mColorArr[0] = new Color(1f, .007f, .007f);
		this.mColorArr[1] = new Color(.17f, .96f, 0f);
		
		for(int i = 0; i < this.mParts.Length; i++){
//			Debug.Log("Part: " + this.mParts[i].GetPart() + " Health " + this.mParts[i].GetHealth());
			this.mMaxHealth += mParts[i].GetMaxHealth();
		}
		
		this.mOldHealth = this.mHealth = this.mMaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		this.DebugEnemy();
		this.UpdateHealthBar();
	}
	
	void FixedUpdate(){
		this.Turn();
	}
	
	private void UpdateHealthBar(){
		this.mHealth = 0;
		for(int i = 0; i < this.mParts.Length; i++){
			this.mHealth += mParts[i].GetHealth();
		}
		if(this.mOldHealth != this.mHealth){
//			Debug.Log(this.mHealth);
			this.mOldHealth = this.mHealth;
		}
		float ratio = Map( this.mHealth, 0, this.mMaxHealth, 0, 1);
		if(this.mCurrentHealthBar && this.mCurrentHealthBar.fillAmount != ratio){
			this.mCurrentHealthBar.fillAmount = Mathf.Lerp(this.mCurrentHealthBar.fillAmount, ratio, Time.deltaTime * this.mColorLerpSpeed);
			this.mCurrentHealthBar.color = Color.Lerp(this.mColorArr[0], this.mColorArr[1], ratio);
		}

		if(mRatioText)
			mRatioText.text = (ratio * 100 ).ToString("0") + "%";			
		
	}
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	private void Turn() {
		// Rotate 
		Vector3 direction = Vector3.zero; 
		float distanceToTarget = 0;
		Vector3 viewAngleA, viewAngleB; 
		switch(mState){
			case STATES.PATROL:
				if(target){
					direction = this.target.position - this.transform.position;
					this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.mRotateVel);	
							
					if(Vector3.Angle( this.transform.forward, direction ) < 5f){
						this.mState = STATES.ATTACK;
					}
				}		
			break;
			case STATES.ATTACK:
				time -= Time.deltaTime;
				lTime -= Time.deltaTime;
				if(time <= 0){
					((EnemyLarm)larm).Shoot();
					this.time = Random.Range(.5f, resetTime);
					
				}
				
				if(lTime <= 0){
					((EnemyRarm)rarm).Shoot();
					this.lTime = Random.Range(0.5f, resetTime);
				}
								
				viewAngleA = fov.DirectionFromAngle(-15f, false); 
				viewAngleB = fov.DirectionFromAngle(15f, false); 
//				Debug.Log(Vector3.Distance(target.transform.position, viewAngleA) > 15f );
//				Debug.Log(Vector3.Distance(target.transform.position, viewAngleB) > 5f );
				if(Vector3.Distance(target.transform.position, viewAngleA) > 15f ||
					Vector3.Distance(target.transform.position, viewAngleB) > 0 ){
					direction = this.target.position - this.transform.position;
					this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.mRotateVel);	
				}
				
				distanceToTarget = Vector3.Distance(this.transform.position, target.position);
				if(distanceToTarget > fov.mViewRadius){
					this.mState = STATES.CHASE;
				}
			break;
			case STATES.CHASE:
				distanceToTarget = Vector3.Distance(this.transform.position, target.position);
				if(distanceToTarget < 25f){
					this.mState = STATES.PATROL;
				}
			break;
		}
	}
	
	private void DebugEnemy(){
		Vector3 viewAngleA = fov.DirectionFromAngle(-15f, false); 
		Vector3 viewAngleB = fov.DirectionFromAngle(15f, false); 
		
		Debug.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.mViewRadius, Color.yellow);
		Debug.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.mViewRadius, Color.yellow);
		
	}
	
	private float Map(float value, float inMin, float inMax, float outMin, float outMax){
		return ( value - inMin ) * ( outMax - outMin) / ( inMax - inMin ) + outMin;
	}
}
