using UnityEngine;
using System.Collections;

public class AttackState : State<Enemy> {

	private static AttackState s_attackState;

	public static AttackState Instance(){
		if(s_attackState == null)
			s_attackState = new AttackState();

		return s_attackState;
	}

	// Use this for initialization
	public override void Start (Enemy mEnemy) { }

	// Update is called once per frame
	public override void Update (Enemy mEnemy) {
		((EnemyLarm)mEnemy.GetPart(1)).Shoot();
		((EnemyRarm)mEnemy.GetPart(2)).Shoot();
		if(mEnemy.mPlayer != null && Vector3.Distance(mEnemy.transform.position, mEnemy.mPlayer.position) > mEnemy.GetFieldOfView().mViewRadius){
			mEnemy.GetFSM().ChangeState(ChaseState.Instance());
		}
	}
	
	public override void FixedUpdate (Enemy mEnemy) {
		this.Move(mEnemy);
		this.Turn(mEnemy);
	}

	// Exit is called once the state is exitted
	public override void Exit (Enemy mEnemy) { }

	protected AttackState () { }
	
	protected override void Move (Enemy mEnemy) { 
		mEnemy.Agent.speed = 0;
	}
	
	protected override void Turn (Enemy mEnemy) {
		Vector3 direction = Vector3.zero; 
		Vector3 viewAngleA, viewAngleB; 
		if(mEnemy.mPlayer){
			direction = mEnemy.mPlayer.position - mEnemy.transform.position;
			mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * mEnemy.mRotateVel);	

			if(Vector3.Angle( mEnemy.transform.forward, direction ) < 5f){
				mEnemy.GetFSM().ChangeState(AttackState.Instance());
			}
		}

		viewAngleA = mEnemy.GetFieldOfView().DirectionFromAngle(-15f, false); 
		viewAngleB = mEnemy.GetFieldOfView().DirectionFromAngle(15f, false); 
		//				Debug.Log(Vector3.Distance(target.transform.position, viewAngleA) > 15f );
		//				Debug.Log(Vector3.Distance(target.transform.position, viewAngleB) > 5f );
		if(Vector3.Distance(mEnemy.mPlayer.transform.position, viewAngleA) > 15f ||
			Vector3.Distance(mEnemy.mPlayer.transform.position, viewAngleB) > 0 ){
			direction = mEnemy.mPlayer.position - mEnemy.transform.position;
			mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * mEnemy.mRotateVel);	
		}
	}
}
