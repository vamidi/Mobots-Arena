using UnityEngine;
using System.Collections;
using System;

public class Larm : Arm { 

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.mPart = PART.LARM;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	protected override void LateUpdate () {
		base.LateUpdate();
	}
	
}
