using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackState : State<Enemy> {

	private static AttackState s_attackState;
	
	private bool inDodging = false;
	private float mIntialRotationSpeed;
	private Vector3 newPos = Vector3.zero;
	
	public static AttackState Instance(){
		if(s_attackState == null)
			s_attackState = new AttackState();

		return s_attackState;
	}

	// Use this for initialization
	public override void Start (Enemy mEnemy) {
		this.mPriority = PRIORITY.MEDIUM;
		this.mCurrentTries = 0;
		this.mIntialRotationSpeed = mEnemy.Agent.angularSpeed;
	}

	// Update is called once per frame
	public override void Update (Enemy mEnemy) {
		if(this.mPriority > mEnemy.GetFSM().GetGlobalState().GetPriority()){
			// Move the player
			mEnemy.StartCoroutine(Dodging(mEnemy));
			
			if(mEnemy.GetFieldOfView().FindTarget() == null && mEnemy.mPlayerInSight && mEnemy.mPlayer){
				mEnemy.mPlayerInSight = false;
			}else if(mEnemy.GetFieldOfView().FindTarget() != null && mEnemy.mPlayerInSight == false && mEnemy.mPlayer){
				mEnemy.mPlayerInSight = true;
			}
			
			if(mEnemy.mPlayer){
				if(Vector3.Distance(mEnemy.transform.position, mEnemy.mPlayer.position) > mEnemy.GetFieldOfView().mViewRadius){
					mEnemy.GetFSM().ChangeState(ChaseState.Instance());
				}
			}
		}
	}
	
	public override void FixedUpdate (Enemy mEnemy) {
		if(this.mPriority > mEnemy.GetFSM().GetGlobalState().GetPriority()){
			this.CoverBehaviour(mEnemy);
			this.Move(mEnemy);
		}
	}
	
	public override void LateUpdate (Enemy mEnemy){
		if(this.mPriority > mEnemy.GetFSM().GetGlobalState().GetPriority()){
			this.Turn(mEnemy);			
		}
	}

	// Exit is called once the state is exitted
	public override void Exit (Enemy mEnemy) { 
		mEnemy.Agent.speed = mEnemy.mSpeed.mChaseSpeed;
		mEnemy.Agent.angularSpeed = this.mIntialRotationSpeed;
		mEnemy.Agent.SetDestination(mEnemy.transform.position);
	}

	protected AttackState () { }
	
	protected override void Move (Enemy mEnemy) {
//		mEnemy.Agent.speed = 0;
		mEnemy.Agent.angularSpeed = 0;
	}
	
	protected IEnumerator Dodging (Enemy mEnemy) {
		if(!inDodging && mEnemy.mPlayer){
			this.inDodging = true;
			Vector3 heading = Vector3.zero;
			float dot;
			int pos = Random.Range(0, mEnemy.mEvadePoints.Length - 1);
			newPos = mEnemy.mEvadePoints[pos].transform.position;
			// see if the point is nearby the player
			heading = mEnemy.mPlayer.position - newPos;
			dot = Vector3.Dot(heading, mEnemy.transform.forward);
			if(dot > 0){
				if(pos + 1 > mEnemy.mEvadePoints.Length - 1){
					pos--;
					if(pos == -1)
						pos = 0;
				}else{
					pos++;
				}
				newPos = mEnemy.mEvadePoints[pos].transform.position;
			}
			mEnemy.Agent.SetDestination(newPos);
			yield return new WaitForSeconds(Random.Range(0f, 1.5f));
			pos = Random.Range(0, mEnemy.mEvadePoints.Length - 1);
			newPos = mEnemy.mEvadePoints[pos].transform.position;
			// see if the point is nearby the player
			heading = mEnemy.mPlayer.position - newPos;
			dot = Vector3.Dot(heading, mEnemy.transform.forward);
			if(dot > 0){
				if(pos + 1 > mEnemy.mEvadePoints.Length - 1){
					pos--;
					if(pos == -1)
						pos = 0;
				}else{
					pos++;
				}
				newPos = mEnemy.mEvadePoints[pos].transform.position;
			}
			mEnemy.Agent.SetDestination(newPos);
			yield return new WaitForSeconds(Random.Range(1f, 1.5f));
			this.inDodging = false;
		}
	}
	
	protected override void Turn (Enemy mEnemy) {
		Vector3 direction = Vector3.zero; 
		Vector3 viewAngleA, viewAngleB; 
		if(mEnemy.mPlayer){
			direction = mEnemy.mPlayer.position - mEnemy.transform.position;
			mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * mEnemy.mRotateVel);	

			if(Vector3.Angle( mEnemy.transform.forward, direction ) < 5f){
				((EnemyLarm)mEnemy.GetPart(1)).Shoot();
				((EnemyRarm)mEnemy.GetPart(2)).Shoot();
			}
		
			viewAngleA = mEnemy.GetFieldOfView().DirectionFromAngle(-15f, false); 
			viewAngleB = mEnemy.GetFieldOfView().DirectionFromAngle(15f, false); 
			if(Vector3.Distance(mEnemy.mPlayer.transform.position, viewAngleA) > 15f ||
				Vector3.Distance(mEnemy.mPlayer.transform.position, viewAngleB) > 0 ){
				direction = mEnemy.mPlayer.position - mEnemy.transform.position;
				mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * mEnemy.mRotateVel);	
			}
		}
	}
}
