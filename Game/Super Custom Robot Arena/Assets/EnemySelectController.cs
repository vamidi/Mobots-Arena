using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Boomlagoon.JSON;
using MBA.UI;


public class EnemySelectController : MonoBehaviour {
	
	public GameObject mContent;
	public GameObject mEnemySlot;
	public float mOffset = 50f;
	public GameManager manager;
	public RobotEditor mEditor;
	
	private bool changing = false;
	private PART mPart = PART.HEAD;
	
	public void SelectEnemy(string enemyName){
		manager.enemyName = enemyName;
		if(this.changing == false)
			StartCoroutine(this.SaveRobot());
	}

	void Awake() {
		Vector3 v = GameObject.FindGameObjectWithTag("Robot").transform.position;
		v.y = 100f;
		GameObject.FindGameObjectWithTag("Robot").transform.position = v;
		GameObject.Find("Cylinder").GetComponent<Renderer>().enabled = false;
		this.manager = GameObject.FindObjectOfType<GameManager>();
		this.mEditor = manager.enemy.GetComponent<RobotEditor>();
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
	
	/// <summary>
	/// Method to change the stats of a part.
	/// </summary>
	/// <param name="part">Part.</param>
	/// <param name="robotName">Robot name.</param>
	private void ChangeStats(string robotName = "", Enemy p = null){

		if (robotName == "")
			return;

		// json object from the robots dictionary
		JSONObject json = manager.robotDictionary[robotName].GetObject("parts");
		switch (this.mPart) {
			case PART.HEAD:
				//				head = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
				//				json = JSONObject.Parse(head);
				json = json.GetObject("head").GetObject("stats");
				p.SetValue (this.mPart, "SetHealth", (float)json.GetNumber("hitpoints"));
				p.SetValue (this.mPart, "SetArmor", (float)json.GetNumber("shieldhitpoints"));
				p.SetValue (this.mPart, "SetStrength", (float)json.GetNumber("shieldstrength"));
				p.SetValue (this.mPart, "SetWeight", (int)json.GetNumber("weight"));
				break;
			case PART.LARM:
				//			    arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
				//			    json = JSONObject.Parse(arm);
				json = json.GetObject("left").GetObject("stats");
				p.SetValue (this.mPart, "SetHealth", (float)json.GetNumber("hitpoints"));
				p.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
				p.SetValue (this.mPart, "SetDamagePerRound", (float)json.GetNumber ("damageperround"));
				p.SetValue (this.mPart, "SetRoundsPerSecond", (float)json.GetNumber ("roundspersecond"));
				p.SetValue (this.mPart, "SetAccuracy", (float)json.GetNumber ("accuracy"));
				break;
			case PART.RARM:
				//			    arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
				//			    json = JSONObject.Parse(arm);
				json = json.GetObject("right").GetObject("stats");
				p.SetValue (this.mPart, "SetHealth", (float)json.GetNumber ("hitpoints"));
				p.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
				p.SetValue (this.mPart, "SetDamagePerRound", (float)json.GetNumber ("damageperround"));
				p.SetValue (this.mPart, "SetRoundsPerSecond", (float)json.GetNumber ("roundspersecond"));
				p.SetValue (this.mPart, "SetAccuracy", (float)json.GetNumber("accuracy"));
				break;
			case PART.CAR:
				//			    car = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
				//			    json = JSONObject.Parse(car);
				json = json.GetObject("car").GetObject("stats");
				p.SetValue (this.mPart, "SetHealth", (float)json.GetNumber ("hitpoints"));
				p.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
				p.SetValue (this.mPart, "SetSpeed", (float)json.GetNumber ("speed"));
				p.SetValue (this.mPart, "SetJumpStrength", (float)json.GetNumber ("jumpstrength"));
				break;
		}
	}
	
	public void EquipRobot (string robotName = ""){
		// holder for the part
		GameObject holder = null;

		switch (this.mPart) {
			case PART.HEAD:
				holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_head", typeof(GameObject));	
				break;
			case PART.LARM:
				holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_larm", typeof(GameObject));		
				break;
			case PART.RARM:
				holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_rarm", typeof(GameObject));		
				break;
			case PART.CAR:
				holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_car", typeof(GameObject));		
				break;
		}

		if (this.mEditor != null && holder != null && this.mPart != PART.UNASSIGNED )
			// Change the robot part with the new object (assign is a callback)
			this.mEditor.SetRobot (this.mPart, holder);
	
	}
	
	private IEnumerator SaveRobot(){
		this.changing = true;
		this.mPart = PART.HEAD;
		this.EquipRobot(manager.enemyName);
		this.mPart = PART.LARM;
		this.EquipRobot(manager.enemyName);
		this.mPart = PART.RARM;
		this.EquipRobot(manager.enemyName);
		this.mPart = PART.CAR;
		this.EquipRobot(manager.enemyName);
		yield return new WaitForSeconds(.5f);
		Enemy holder = null;
		if(manager.enemy.GetComponent<Enemy>() == null) {
			Debug.Log("here");
			holder = manager.enemy.AddComponent<Enemy>();
			holder.isControllable = false;
			holder.GetPartObj(0).AddComponent<EnemyHead>();
			holder.GetPartObj(1).AddComponent<EnemyLarm>();
			holder.GetPartObj(2).AddComponent<EnemyRarm>();
			holder.GetPartObj(3).AddComponent<EnemyCar>();
			yield return new WaitForSeconds(.5f);
			holder.Initialize();
		}else {
			Debug.Log("here");
			Destroy(manager.enemy.GetComponent<Enemy>());
			holder = manager.enemy.AddComponent<Enemy>();
			holder.isControllable = false;
			holder.GetPartObj(0).AddComponent<EnemyHead>();
			holder.GetPartObj(1).AddComponent<EnemyLarm>();
			holder.GetPartObj(2).AddComponent<EnemyRarm>();
			holder.GetPartObj(3).AddComponent<EnemyCar>();
			yield return new WaitForSeconds(.5f);
			holder.Initialize();
		}
		this.mPart = PART.HEAD;
		this.ChangeStats(manager.enemyName, holder);
		this.mPart = PART.LARM;
		this.ChangeStats(manager.enemyName, holder);
		this.mPart = PART.RARM;
		this.ChangeStats(manager.enemyName, holder);
		this.mPart = PART.CAR;
		this.ChangeStats(manager.enemyName, holder);				
		this.mPart = PART.HEAD;
		this.changing = false;
		yield return new WaitForSeconds(1f);
		GameObject.FindGameObjectWithTag("Menu").SendMessage("SetNextPage", "Level");
		yield return null;
	}
}
