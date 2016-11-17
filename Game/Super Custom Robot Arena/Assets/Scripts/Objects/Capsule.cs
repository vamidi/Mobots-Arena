using UnityEngine;
using System;
using System.Collections;

public enum SIZE {
	SMALLER, SMALL, MEDIUM, BIG, ULTRA
}

public enum KIND {
	HEALTH, SHIELD, WEIGHT, DAMAGE
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
	private int mMultiplier;
	private int mWeightAmount;
	// 5%, 10%, 15% of the full weight of the robot

	// Use this for initialization
	void Start () {
		switch(this.mSize){
			case SIZE.SMALLER:
				this.mAmount = 10;
				this.mMultiplier = 120;
				this.mWeightAmount = 5;
				break;
			case SIZE.SMALL:
				this.mAmount = 15;
				this.mMultiplier = 140;
				this.mWeightAmount = 5;
				break;
			case SIZE.MEDIUM:
				this.mAmount = 25;
				this.mMultiplier = 160;
				this.mWeightAmount = 10;
				break;
		case SIZE.BIG:
				this.mAmount = 50;
				this.mMultiplier = 180;
				this.mWeightAmount = 15;
				break;
			case SIZE.ULTRA:
				this.mAmount = 50;
				this.mMultiplier = 200;
				this.mWeightAmount = 15;
				break;
		}
	}
	
	// Update is called once per frame
	void Update () { }
		
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="col">Col.</param>
	void OnTriggerEnter(Collider col){
		if(col.tag == "Head" || col.tag == "Left" || col.tag == "Right" || col.tag == "Car"){
			switch(this.mKind){
				case KIND.HEALTH:
					GetLowestHealthOfPart(col).SendMessage("Heal", this.mAmount, SendMessageOptions.DontRequireReceiver);
					break;
				case KIND.SHIELD:			
					col.SendMessage ("ArmorHeal", this.mAmount, SendMessageOptions.DontRequireReceiver);
					break;
			case KIND.WEIGHT:
					col.transform.root.SendMessage ("IncreaseMovement", this.mWeightAmount, SendMessageOptions.DontRequireReceiver);
					break;
			case KIND.DAMAGE:
					col.transform.root.SendMessage ("IncreaseDamage", this.mMultiplier, SendMessageOptions.DontRequireReceiver);
					break;
				default:
					GetLowestHealthOfPart(col).SendMessage("Heal", this.mAmount, SendMessageOptions.DontRequireReceiver);
					break;		
			}			
			
			this.gameObject.SetActive(false);
		}
	}
	
	private Part GetLowestHealthOfPart(Collider col){
		SCRA.Humanoids.Robot r = col.transform.GetComponent<Part>().GetParent();
		Part p = null;
		float amount = 99999;
		for(int i = 0; i < 4; i++){
			if(r.GetPart(i).GetHealth() < amount){
				amount = r.GetPart(i).GetHealth();
				p = r.GetPart(i);
			}
		}
		
		return p;
	}
}
