using UnityEngine;
using System.Collections;

public class SearchState : State<Enemy> {
	
	private static SearchState s_searchState;
	
	private Vector3 mGotoPos = Vector3.zero;

	public static SearchState Instance(){
		if(s_searchState == null)
			s_searchState = new SearchState();

		return s_searchState;
	}
	
	// Use this for initialization
	public override void Start (Enemy mEnemy) {
		this.mPriority = PRIORITY.MEDIUM;
	}
	
	// Update is called once per frame
	public override void Update (Enemy mEnemy) {
		if(this.mPriority > mEnemy.GetFSM().GetGlobalState().GetPriority()){
			mEnemy.mPlayerInSight = false;
			Player p = mEnemy.mPlayer.GetComponent<Player>(); 
			if(this.mGotoPos == Vector3.zero){
				this.mGotoPos = p.transform.position;
				mEnemy.Agent.SetDestination(this.mGotoPos);
			}
			 
			if(mEnemy.GetFieldOfView().FindTarget() != null && mEnemy.mPlayerInSight == false && mEnemy.mPlayer){
				// Move the player
				this.mGotoPos = Vector3.zero;
				mEnemy.mPlayerInSight = true;
				mEnemy.GetFSM().ChangeState(AttackState.Instance());
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

	protected SearchState () { }

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

