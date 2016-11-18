using UnityEngine;
using System.Collections;

public abstract class State<T> {
	
	// Use this for initialization
	public abstract void Start (T mEnemy);
	
	// Update is called once per frame
    public abstract void Update (T mEnemy);
	
	public abstract void FixedUpdate (T mEnemy);

	// Exit is called once the state is exitted
	public abstract void Exit (T mEnemy);

	protected abstract void Move (T mEnemy);
	
	protected abstract void Turn (T mEnemy);
}
