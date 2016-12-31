using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class EnemySelectController : MonoBehaviour {
	
	public GameObject mContent;
	public GameObject mEnemySlot;
	public float mOffset = 50f;
	public GameManager manager;
	
	public void SelectEnemy(string enemyName){
		Debug.Log(enemyName);
	}

	void Awake() {
		Vector3 v = GameObject.FindGameObjectWithTag("Robot").transform.position;
		v.y = 100f;
		GameObject.FindGameObjectWithTag("Robot").transform.position = v;
		GameObject.Find("Cylinder").GetComponent<Renderer>().enabled = false;
		this.manager = GameObject.FindObjectOfType<GameManager>();
	}
	
	// Use this for initialization
	void Start () {
		
		List<string>robotsnames = new List<string>();
		
		for(int i = 0; i < manager.robots.Length; i++){
			robotsnames.Add(manager.robots[i].Obj.GetString("robotname"));
		}

		float rows = Mathf.Floor(robotsnames.Count / 3);
		int columns = 3;
		List<Vector3> positions = new List<Vector3>();
		for(int row = 0; row <= rows; row++) {
			for(int column = 0; column < columns; column++) {
				// float r = column * rows + row;				
				Vector3 targetPos = new Vector3(145f, -90f, 0);
				targetPos.x = targetPos.x + (column * 300f) + (column * this.mOffset);
				targetPos.y = targetPos.y - (155f * row) - (row * this.mOffset);
				targetPos.z = 0;
				positions.Add(targetPos);

			}
		}		

		for(int i = 0; i < robotsnames.Count; i++){
			GameObject b = Instantiate(this.mEnemySlot as GameObject);
			if(this.mEnemySlot){
				b.transform.SetParent(this.mContent.transform, false);		
				RectTransform rect = b.GetComponent<RectTransform>();
				b.GetComponent<DynamicListener>().mMessageParameter = robotsnames[i];
				b.GetComponentInChildren<Text>().text = robotsnames[i];
				if(i != 0)
					rect.anchoredPosition = positions[i];

			}
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
