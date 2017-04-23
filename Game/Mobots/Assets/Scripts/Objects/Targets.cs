using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour, IDamageable<float> {

	public float mHealth = 100f;
	public float mMaxHealth = 100f;

	public void Damage(float damage){
		mHealth -= damage;
	}

	// Use this for initialization
	void Start () {
		this.mHealth = this.mMaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (mHealth <= 0) {
			this.gameObject.SetActive (false);
		}
	}
}
