using UnityEngine;
using System.Collections;

public class Larm : Part {

	/// <summary>
	/// The damage that the robot deals to 
	/// the other player
	/// </summary>
	private float mDamagePerRound = 5f;

	/// <summary>
	/// Rounds per second.
	/// </summary>
	private float mRoundPerSecond = 1f;

	/// <summary>
	/// The Robot accuracy.
	/// </summary>
	private float mAccuracy = 3f;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.mPart = PART.LARM;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
