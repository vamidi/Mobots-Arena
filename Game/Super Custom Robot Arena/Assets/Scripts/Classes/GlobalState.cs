using UnityEngine;
using System.Collections;
using SCRA.Humanoids;

public class GlobalState : State<Enemy> {

	private static GlobalState s_globalState;

	public static GlobalState Instance(){
		if(s_globalState == null)
			s_globalState = new GlobalState();

		return s_globalState;
	}

	// Use this for initialization
	public override void Start (Enemy mEnemy) {
		this.mPriority = PRIORITY.LOW;
	}

	// Update is called once per frame
	public override void Update (Enemy mEnemy) {
		float healthPercentage = (mEnemy.GetMaxHealth() / 100 ) * 50;
		if(mEnemy.GetHealth() < healthPercentage) {
			mEnemy.GetFSM().SetGlobalState(EvadeState.Instance(CHOICES.FindHealth));
		}
	}

	public override void FixedUpdate (Enemy mEnemy) {
		this.CheckCover(mEnemy);
		this.Move(mEnemy);
	}
	
	public override void LateUpdate (Enemy mEnemy) {
		this.Turn(mEnemy);
	}

	// Exit is called once the state is exitted
	public override void Exit (Enemy mEnemy) { }

	protected GlobalState () { }

	protected override void Move (Enemy mEnemy) { }

	protected override void Turn (Enemy mEnemy) { }
}
