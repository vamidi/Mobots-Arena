﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	
	public GameObject mEnemyObj;
	public GameObject mCurrentEnemy;
	public GameObject mExplosionPrefab;
	public GameObject[] mSpawnponts;
	
	private Enemy mEnemyScript;
	

	// Use this for initialization
	void Start () {
		if(mCurrentEnemy)
			mEnemyScript = mCurrentEnemy.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!mEnemyScript.IsAlive()){
			if(this.mExplosionPrefab)
				Instantiate(mExplosionPrefab, mCurrentEnemy.transform.position, Quaternion.identity);
			
			Destroy(mCurrentEnemy.gameObject);
			
			if(mSpawnponts != null && mSpawnponts.Length > 0 && mEnemyObj){
				int r = Random.Range(0, mSpawnponts.Length - 1);
				GameObject newEnemy = (GameObject) Instantiate(mEnemyObj, mSpawnponts[r].transform.position, Quaternion.identity);
				this.mCurrentEnemy = newEnemy;
				this.mEnemyScript = this.mCurrentEnemy.GetComponent<Enemy>();
			}
		}
	}
}
