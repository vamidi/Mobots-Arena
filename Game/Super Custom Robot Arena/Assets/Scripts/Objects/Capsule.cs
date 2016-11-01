using UnityEngine;
using System.Collections;
using System;

public enum SIZE {
	SMALLER, SMALL, MEDIUM, BIG
}

public enum KIND {
	HEALTH, SHIELD
}

public class Capsule : MonoBehaviour {
	
	/// <summary>
	/// The size of the capsule.
	/// </summary>
	public SIZE mSize = SIZE.SMALLER;
	/// <summary>
	/// What kind is the capsule
	/// </summary>
	public KIND mKind = KIND.HEALTH;
	/// <summary>
	/// The amount of damage or recovery
	/// </summary>
	private int mAmount;
	
	// Use this for initialization
	void Start () {
		switch(this.mSize){
			case SIZE.SMALLER:
				this.mAmount = 10;
				break;
			case SIZE.SMALL:
				this.mAmount = 15;
				break;
			case SIZE.MEDIUM:
				this.mAmount = 25;
				break;
			case SIZE.BIG:
				this.mAmount = 50;
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="col">Col.</param>
	void OnTriggerEnter(Collider col){
		if(col.tag == "Head" || col.tag == "left" || col.tag == "right" || col.tag == "Car"){
			switch(this.mKind){
				case KIND.HEALTH:
					col.SendMessage("Heal", mAmount, SendMessageOptions.DontRequireReceiver);
					break;
				case KIND.SHIELD:
					col.SendMessage("ArmorHeal", mAmount, SendMessageOptions.DontRequireReceiver);
					break;
				default:
					col.SendMessage("Heal", mAmount, SendMessageOptions.DontRequireReceiver);
					break;		
			}
		}
	}
}
