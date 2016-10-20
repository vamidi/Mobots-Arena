using UnityEngine;
using System.Collections;

public class Larm : Part {

	/// <summary>
	/// The damage that the robot deals to 
	/// the other player
	/// </summary>
	[SerializeField]
	private float mDamagePerRound = 5f;

	/// <summary>
	/// Rounds per second.
	/// </summary>
	[SerializeField]
	private float mRoundsPerSecond = 1f;

	/// <summary>
	/// The Robot accuracy.
	/// </summary>
	[SerializeField]
	private float mAccuracy = 3f;

	public void SetDamagePerRound(float damage){
		this.mDamagePerRound = damage;
	}

	public void SetRoundsPerSecond(float seconds){
		this.mRoundsPerSecond = seconds;
	}

	public void SetAccuracy(float accuracy){
		this.mAccuracy = accuracy;
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.mPart = PART.LARM;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
