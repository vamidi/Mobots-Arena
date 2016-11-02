using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable<float> {
	
	public float mHealth = 100f;
	public float mMaxHealth = 100f;
	
	private Enemy mEnemy;

	public void Damage(float d){
//		Head tempHead = (Head) this.mEnemy.GetPart(0);
//		float damageOnHealth = ( (100f - tempHead.Strenght) / 100f ) * d;
		float damageOnHealth = ( (100f - 15) / 100f ) * d; // placeholder

		this.mHealth -= damageOnHealth;
//		tempHead.ArmorHealth -= d;
	}
	
	// Use this for initialization
	void Start () {
		this.mEnemy = this.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void UpdateHealth(){
		if(this.mHealth <= 0){
			this.gameObject.SetActive(false);
		}
	}
}
