using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SCRA.Humanoids;

public class EvadeState : State<Enemy> {
	
	public float mDistanceToTarget = 3.5f;
	
	private static EvadeState s_evadeState;
	private static CHOICES mChoice = CHOICES.FindHealth;
	
	private Vector3 mCurrentLookPos = Vector3.zero;

	public static EvadeState Instance(CHOICES c){
		mChoice = c;
		if(s_evadeState == null)
			s_evadeState = new EvadeState();

		return s_evadeState;
	}

	// Use this for initialization
	public override void Start (Enemy mEnemy) {
		this.mPriority = PRIORITY.HIGH;
		this.mCurrentTries = 0;
	}

	// Update is called once per frame
	public override void Update (Enemy mEnemy) {
		
		switch(mChoice){
			case CHOICES.FindHealth:
				this.SeekHealth(mEnemy);
				break;
			case CHOICES.SeekCover:
				this.SeekCover(mEnemy);
				break;
		}
	}

	public override void FixedUpdate (Enemy mEnemy) { 
		this.CheckCover(mEnemy);
		this.CoverBehaviour(mEnemy);
		this.Move(mEnemy); 
	}
	
	public override void LateUpdate (Enemy mEnemy) {
		this.Turn(mEnemy);
	}
	
	// Exit is called once the state is exitted
	public override void Exit (Enemy mEnemy) {
		this.mIgnorePositions.Clear();
	}

	protected EvadeState () { }
	
	protected override void Move (Enemy mEnemy) { }
	
	protected override void Turn (Enemy mEnemy) { }
	
	private void SeekHealth(Enemy mEnemy){
		
		if(this.mCurrentLookPos != Vector3.zero && Vector3.Distance(mEnemy.transform.position, this.mCurrentLookPos) < this.mDistanceToTarget){
			this.mCurrentLookPos = Vector3.zero;
			mEnemy.GetFSM().SetGlobalState(GlobalState.Instance());
		}
		
		if(this.mCurrentLookPos != Vector3.zero && mEnemy.Agent.destination == this.mCurrentLookPos)
			return;		
		
		List<Transform>points = mEnemy.GetFieldOfView().FindNearestCapsule();
		List<Capsule>capsules = new List<Capsule>();
		if(points.Count > 0){
			foreach(Transform point in points){
				Capsule cap = point.GetComponent<Capsule>();
				if(cap.mKind == KIND.HEALTH){
					this.mCurrentLookPos = cap.transform.position;
					capsules.Add(cap);
					break;
				}
			}
		
			mEnemy.Agent.speed = mEnemy.mSpeed.mChaseSpeed;
			mEnemy.Agent.SetDestination(this.mCurrentLookPos);
		}
		
		if(capsules.Count == 0){
			mChoice = CHOICES.SeekCover;
		}
	}

	private void SeekCover(Enemy mEnemy){
		if(mEnemy.mCurrentCoverBase != null && mEnemy.Agent.destination != mEnemy.mCurrentCoverBase.mPositionObject.position){
			mEnemy.Agent.speed = mEnemy.mSpeed.mChaseSpeed;
			mEnemy.Agent.SetDestination(mEnemy.mCurrentCoverBase.mPositionObject.position);
			this.mPriority = PRIORITY.HIGH;
		}

		if(mEnemy.mCurrentCoverBase != null && Vector3.Distance(mEnemy.transform.position, mEnemy.mCurrentCoverBase.mPositionObject.position) < 3.5f){
			// set the enemy back to attack mode
			this.mPriority = PRIORITY.LOW;
			mEnemy.GetFSM().ChangeState(AttackState.Instance());
		}
	}
}
