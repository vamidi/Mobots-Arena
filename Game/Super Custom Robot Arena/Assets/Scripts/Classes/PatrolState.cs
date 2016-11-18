using UnityEngine;
using System.Collections;

public class PatrolState : State<Enemy> {
	
	private static PatrolState s_patrolState;
	
	public static PatrolState Instance(){
		if(s_patrolState == null)
			s_patrolState = new PatrolState();
		
		return s_patrolState;
	}

	// Use this for initialization
	public override void Start (Enemy mEnemy) {
	
	}
	
	// Update is called once per frame
	public override void Update (Enemy mEnemy) {
		if(mEnemy.mWaypoints != null && mEnemy.mWaypoints.Length > 0){
			if(Vector3.Distance(mEnemy.mWaypoints[mEnemy.mCurrentWP].transform.position, mEnemy.transform.position) < mEnemy.mAccWP){
				mEnemy.mCurrentWP++;
				if(mEnemy.mCurrentWP >= mEnemy.mWaypoints.Length){
					mEnemy.mCurrentWP = 0;
				}
			}
		}

		if(mEnemy.mPlayer)
			mEnemy.ChangeState(ChaseState.Instance());
	}
	
	public override void FixedUpdate (Enemy mEnemy) {
		Move(mEnemy);
	}
	
	// Exit is called once the state is exitted
	public override void Exit (Enemy mEnemy) {
	
	}
	
	protected PatrolState () { }

	protected override void Move(Enemy mEnemy){
		/*if(this.mPriority == PRIORITY.SEARCH){
			if(this.fov.mVisibleTargets.Count > 0){
				foreach(Transform target in this.fov.mVisibleTargets){
					if(target.GetComponent<Capsule>().mKind == KIND.HEALTH){
						this.mAgent.SetDestination(target.position);
						break;
					}
				}
			}
		}else */
		if(mEnemy.Agent.destination != mEnemy.mWaypoints[mEnemy.mCurrentWP].transform.position)
			mEnemy.Agent.SetDestination(mEnemy.mWaypoints[mEnemy.mCurrentWP].transform.position);		
	}
	
	protected override void Turn (Enemy mEnemy) { }
}
