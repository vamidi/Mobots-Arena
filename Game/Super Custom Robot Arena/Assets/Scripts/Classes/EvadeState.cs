using UnityEngine;
using System.Collections;

public class EvadeState : State<Enemy> {
	
	private static EvadeState s_evadeState;

	public static EvadeState Instance(){
		if(s_evadeState == null)
			s_evadeState = new EvadeState();

		return s_evadeState;
	}

	// Use this for initialization
	public override void Start (Enemy mEnemy) {

	}

	// Update is called once per frame
	public override void Update (Enemy mEnemy) {

	}

	public override void FixedUpdate (Enemy mEnemy) {
		this.Move(mEnemy);
	}
	
	// Exit is called once the state is exitted
	public override void Exit (Enemy mEnemy) {

	}

	protected EvadeState () { }
	
	protected override void Move (Enemy mEnemy) {
		mEnemy.Agent.speed = mEnemy.mSpeed.mChaseSpeed;
		if(mEnemy.mPlayer != null){
			if(mEnemy.Agent.destination != mEnemy.mPlayer.position)
				mEnemy.Agent.SetDestination(mEnemy.mPlayer.position);
		}
	}
	
	protected override void Turn (Enemy mEnemy) { }
}
