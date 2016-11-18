using UnityEngine;
using System.Collections;

public abstract class State<Enemy> {
	
	// Use this for initialization
	public abstract void Start (Enemy mEnemy);
	
	// Update is called once per frame
    public abstract void Update (Enemy mEnemy);
	
	public abstract void FixedUpdate (Enemy mEnemy);
	
	// Exit is called once the state is exitted
	public abstract void Exit (Enemy mEnemy);
	
	protected abstract void Move (Enemy mEnemy);
	
	protected abstract void Turn (Enemy mEnemy);
}
