using UnityEngine;
using System.Collections;

public class Rarm : Arm {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.mPart = PART.RARM;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	protected override void LateUpdate () {
		base.LateUpdate();
	}
}
