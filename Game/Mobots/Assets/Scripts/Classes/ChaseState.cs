using UnityEngine;
using System.Collections;

public class ChaseState : State<Enemy> {

	private static ChaseState s_chaseState;

	public static ChaseState Instance(){
		if(s_chaseState == null)
			s_chaseState = new ChaseState();

		return s_chaseState;
	}

	// Use this for initialization
	public override void Start (Enemy mEnemy) {
		this.mPriority = PRIORITY.MEDIUM;
	}

	// Update is called once per frame
	public override void Update (Enemy mEnemy) {
		if(this.mPriority > mEnemy.GetFSM().GetGlobalState().GetPriority()){
			if(mEnemy.mPlayer != null && Vector3.Distance(mEnemy.transform.position, mEnemy.mPlayer.position) < mEnemy.GetFieldOfView().mViewRadius ){
				mEnemy.researchArea = mEnemy.mResetArea;
				mEnemy.GetFSM().ChangeState(AttackState.Instance());
			}

			// do the timer to see if the player is behind a wall
			mEnemy.researchArea -= Time.deltaTime;
			if(mEnemy.researchArea <= 0){
				mEnemy.researchArea = mEnemy.mResetArea;
				mEnemy.mPlayer = mEnemy.GetFieldOfView().FindTarget();
				if(!mEnemy.mPlayer)
					mEnemy.GetFSM().ChangeState(PatrolState.Instance());
			}
		}
	}
	
	public override void FixedUpdate (Enemy mEnemy) {
		if(this.mPriority > mEnemy.GetFSM().GetGlobalState().GetPriority()){
			this.Move(mEnemy);
		}
	}

	public override void LateUpdate(Enemy mEnemy){
		if(this.mPriority > mEnemy.GetFSM().GetGlobalState().GetPriority()){
			this.Turn(mEnemy);
		}
	}
	
	// Exit is called once the state is exitted
	public override void Exit (Enemy mEnemy) { }

	protected ChaseState () { }
	
	protected override void Move (Enemy mEnemy) {
		mEnemy.Agent.speed = mEnemy.mSpeed.mChaseSpeed;
		mEnemy.Agent.SetDestination(mEnemy.mPlayer.position);
	}
	
	protected override void Turn (Enemy mEnemy) {
		Vector3 direction = Vector3.zero; 
		direction = mEnemy.mPlayer.position - mEnemy.transform.position;
		mEnemy.transform.rotation = Quaternion.Lerp(mEnemy.transform.rotation, Quaternion.LookRotation(direction), mEnemy.mRotateVel);
	}
}
