using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public Part larm, rarm;
	public STATES mState = STATES.PATROL;
	public FieldOfView fov;
	public float mRotateVel;
	public bool hasTarget;
	public Transform target;
	public float time,lTime;
	public float resetTime;
	
	// Use this for initialization
	void Start () {
		this.time = this.lTime = resetTime;
		this.fov = this.GetComponent<FieldOfView>();
	}
	
	// Update is called once per frame
	void Update () {
		this.DebugEnemy();
	}
	
	void FixedUpdate(){
		this.Turn();
	}
	
	/// <summary>
	/// This method is for to turn the robot
	/// </summary>
	protected void Turn() {
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
	
	void DebugEnemy(){
		Vector3 viewAngleA = fov.DirectionFromAngle(-15f, false); 
		Vector3 viewAngleB = fov.DirectionFromAngle(15f, false); 
		
		Debug.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.mViewRadius, Color.yellow);
		Debug.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.mViewRadius, Color.yellow);
		
	}
}
