using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Boomlagoon.JSON;
using MBA.UI;


public class EnemySelectController : MonoBehaviour {
	
	public GameObject mContent;
	public GameObject mEnemySlot;
	public float mOffset = 50f, mButtonOffsetX = 200f, mButtonOffsetY = 155f;
	public GameManager manager;
	public RobotEditor mEditor;
	
	private Vector3 mPosition, mInitPosition;
	private bool changing = false, init = false;
	private PART mPart = PART.HEAD;
	
	public void SelectEnemy(string enemyName){
		manager.enemyName = enemyName;
		if(this.changing == false)
			StartCoroutine(this.SaveRobot());
	}
	
	public void CreateRobot(){
		if(this.changing == false && this.init)
			StartCoroutine(this.MakeRobot());		
	}

	void Awake() {
		Vector3 v = GameObject.FindGameObjectWithTag("Robot").transform.position;
		v.y = 100f;
		GameObject.FindGameObjectWithTag("Robot").transform.position = v;
		GameObject.Find("Cylinder").GetComponent<Renderer>().enabled = false;
		this.manager = GameObject.FindObjectOfType<GameManager>();
		this.mEditor = manager.enemy.GetComponent<RobotEditor>();
		GameObject mCanvas = GameUtilities.FindDeepChild(this.mEditor.transform, "Canvas").gameObject;
		if(mCanvas)
			mCanvas.SetActive(false);
		this.mInitPosition = this.mEditor.transform.position;
		
		this.mPosition = this.transform.localPosition;
	}
	
	// Use this for initialization
	void Start () {
		
		List<string>robotsnames = new List<string>();
		
		for(int i = 0; i < manager.robots.Length; i++){
			robotsnames.Add(manager.robots[i].Obj.GetString("robotname"));
		}

		for(int i = 0; i < 3; i++){
			robotsnames.Add("slot_#" + (i+1) + "");
		}
		
		float rows = Mathf.Floor(robotsnames.Count / 3);
		int columns = 3;
		List<Vector3> positions = new List<Vector3>();
		for(int row = 0; row <= rows; row++) {
			for(int column = 0; column < columns; column++) {
				// float r = column * rows + row;				
				Vector3 targetPos = new Vector3(145f, -90f, 0);
				targetPos.x = targetPos.x + (column * this.mButtonOffsetX) + (column * this.mOffset);
				targetPos.y = targetPos.y - (this.mButtonOffsetY * row) - (row * this.mOffset);
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
		this.changing = this.init = true;
		if(this.manager.enemyName.Contains("slot")){
			string file = "";
			if(GameUtilities.CheckFileExists("Slots/", this.manager.enemyName + ".txt")){
				file = GameUtilities.ReadFile("Slots/", this.manager.enemyName + ".txt");	
			}
			
			JSONObject j = JSONObject.Parse(file).GetObject("robot");
			
			this.manager.enemyHead = j.GetString("head");
			this.manager.enemyLarm = j.GetString("left");
			this.manager.enemyRarm = j.GetString("right");
			this.manager.enemyCar = j.GetString("car");
			
			this.mPart = PART.HEAD;
			this.EquipRobot(this.manager.enemyHead);
			this.mPart = PART.LARM;
			this.EquipRobot(this.manager.enemyLarm);
			this.mPart = PART.RARM;
			this.EquipRobot(this.manager.enemyRarm);
			this.mPart = PART.CAR;
			this.EquipRobot(this.manager.enemyCar);
		}else{
			this.mPart = PART.HEAD;
			this.EquipRobot(manager.enemyName);
			this.mPart = PART.LARM;
			this.EquipRobot(manager.enemyName);
			this.mPart = PART.RARM;
			this.EquipRobot(manager.enemyName);
			this.mPart = PART.CAR;
			this.EquipRobot(manager.enemyName);
		}
		yield return new WaitForSeconds(.5f);
		this.changing = false;
		this.transform.localPosition = this.mPosition;
		yield return new WaitForSeconds(1f);
		this.mEditor.transform.position = new Vector3(4f, -.5f, 10.5f );
		yield return null;
	}
	
	private IEnumerator MakeRobot(){
		this.changing = true;
		this.mEditor.transform.position = this.mInitPosition;
		Enemy holder = null;
		if(manager.enemy.GetComponent<Enemy>() == null) {
			//			Debug.Log("here");
			holder = manager.enemy.AddComponent<Enemy>();
			yield return new WaitForSeconds(.5f);
			holder.isControllable = false;
			holder.GetPartObj(0).AddComponent<EnemyHead>();
			GameObject bullet = (GameObject) GameUtilities.ReadResourceFile("Bullets/Bullet");
			holder.GetPartObj(1).AddComponent<EnemyLarm>();
			holder.GetPartObj(1).GetComponent<EnemyLarm>().mBullet = bullet;
			holder.GetPartObj(2).AddComponent<EnemyRarm>();
			holder.GetPartObj(2).GetComponent<EnemyRarm>().mBullet = bullet;
			holder.GetPartObj(3).AddComponent<EnemyCar>();
			yield return new WaitForSeconds(.5f);
			holder.Initialize();
		}else{
			Destroy(manager.enemy.GetComponent<Enemy>());
			holder = manager.enemy.AddComponent<Enemy>();
			yield return new WaitForSeconds(.5f);
			holder.isControllable = false;
			holder.GetPartObj(0).AddComponent<EnemyHead>();
			GameObject bullet = (GameObject) GameUtilities.ReadResourceFile("Bullets/Bullet");
			holder.GetPartObj(1).AddComponent<EnemyLarm>();
			holder.GetPartObj(1).GetComponent<EnemyLarm>().mBullet = bullet;
			holder.GetPartObj(2).AddComponent<EnemyRarm>();
			holder.GetPartObj(2).GetComponent<EnemyRarm>().mBullet = bullet;
			holder.GetPartObj(3).AddComponent<EnemyCar>();
			yield return new WaitForSeconds(.5f);
			holder.Initialize();
		}
		if(this.manager.enemyName.Contains("slot")){
			this.mPart = PART.HEAD;
			this.ChangeStats(this.manager.enemyHead, holder);
			this.mPart = PART.LARM;
			this.ChangeStats(this.manager.enemyLarm, holder);
			this.mPart = PART.RARM;
			this.ChangeStats(this.manager.enemyRarm, holder);
			this.mPart = PART.CAR;
			this.ChangeStats(this.manager.enemyCar, holder);				
			this.mPart = PART.HEAD;
		}else{		
			this.mPart = PART.HEAD;
			this.ChangeStats(manager.enemyName, holder);
			this.mPart = PART.LARM;
			this.ChangeStats(manager.enemyName, holder);
			this.mPart = PART.RARM;
			this.ChangeStats(manager.enemyName, holder);
			this.mPart = PART.CAR;
			this.ChangeStats(manager.enemyName, holder);				
			this.mPart = PART.HEAD;
		}
		yield return new WaitForSeconds(1.2f);
		this.changing = false;
		GameObject.FindGameObjectWithTag("Menu").SendMessage("SetNextPage", "Level");
		yield return null;
	}
}
