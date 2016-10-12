using UnityEngine;
using System.Collections;

public class Car : Part {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.mPart = PART.CAR;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
