using UnityEngine;
using System.Collections;

public class StateMachine {

	private Enemy mEnemy;
	private State<Enemy> mPreviousState;
	private State<Enemy> mCurrentState;
	private State<Enemy> mGlobalState;

	// Use these methods to initialize the FSM
	public void SetPreviousState(State<Enemy> s){ this.mPreviousState = s; }
	public void SetCurrentState(State<Enemy> s){ this.mCurrentState = s; }
	public void SetGlobalState(State<Enemy> s){ this.mGlobalState = s; }

	// Accessors
	public State<Enemy> GetCurrentState () { return this.mCurrentState; }
	public State<Enemy> GetGlobalState () { return this.mGlobalState; }
	public State<Enemy> GetPreviousState () { return this.mPreviousState; }

	// Update is called once per frame
	public void Update () {

		if (this.mGlobalState != null)
			this.mGlobalState.Update (this.mEnemy);

		if (this.mCurrentState != null) {
			this.mCurrentState.Update (this.mEnemy);
		}
	}

	public void FixedUpdate(){
		if(this.mCurrentState != null)
			this.mCurrentState.FixedUpdate (this.mEnemy);
	}

	// Change to a new state
	public void ChangeState(State<Enemy> newState){
		/// <summary>
		/// Make sure both states are valid before attempting to
		/// call their methods
		/// </summary>
		//		Debug.Assert(this.mState && newState);

		// Set the current state to the previous state
		this.mPreviousState = this.mCurrentState;

		// Call the exit method of the existing state
		this.mCurrentState.Exit(this.mEnemy);

		// Change state to the new state
		this.mCurrentState = newState;

		// Call the entry method of the new state
		this.mCurrentState.Start(this.mEnemy);
	}

	public StateMachine(Enemy enemy){
		this.mPreviousState = this.mCurrentState = this.mGlobalState = null;
		this.mEnemy = enemy;
	}

	//change state back to the previous state
	private void RevertToPreviousState() {
		this.ChangeState(this.mPreviousState);
	}

	//returns true if the current state’s type is equal to the type of the
	//class passed as a parameter.
	private bool isInState(State<Enemy> st) { return (this.mCurrentState == st); }

}
