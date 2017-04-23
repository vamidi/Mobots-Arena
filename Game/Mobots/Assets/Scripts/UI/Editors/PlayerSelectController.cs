using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// own namespace
using MBA.UI;
// 
using Boomlagoon.JSON;

public class PlayerSelectController : MonoBehaviour {
	
	public GameObject mPlayerSlot;
	public GameObject mContent;
	public int mOffset;
	
	private GameManager manager;
	
	private RobotEditor mRobotEditor;
	private bool changing = false;
	private string head, larm, rarm, car;
	private PART mPart = PART.HEAD;
	
	public void SelectSlot(string slot){
		
		string file = "";
		if(GameUtilities.CheckFileExists("Slots/", "slot_#" + slot + ".txt")){
			file = GameUtilities.ReadFile("Slots/", "slot_#" + slot + ".txt");	
		}
				
		JSONObject j = JSONObject.Parse(file).GetObject("robot");

		this.head = j.GetString("head");
		this.larm = j.GetString("left");
		this.rarm = j.GetString("right");
		this.car = j.GetString("car");
	
		if(this.changing == false)
			StartCoroutine(this.SaveRobot());
	}
	
	public void ContinueWithSlot(){
		if(this.changing == false)
			StartCoroutine(this.MakeRobot());		
	}
	
	
	void Awake() {
		this.mRobotEditor = GameObject.FindGameObjectWithTag("Robot").GetComponent<RobotEditor>();
		this.mRobotEditor.transform.position = new Vector3(this.mRobotEditor.transform.position.x, -0.6f, this.mRobotEditor.transform.position.z );
//		Vector3 v = this.mRobotEditor.gameObject.transform.position;
//		v.y = 100f;
//		GameObject.FindGameObjectWithTag("Robot").transform.position = v;
		GameObject.Find("Cylinder").GetComponent<Renderer>().enabled = true;
		this.manager = GameObject.FindObjectOfType<GameManager>();
	}

	// Use this for initialization
	void Start () {
		if(Camera.main.GetComponent<SimpleRotation>() != null){
			Destroy(Camera.main.gameObject.GetComponent<SimpleRotation>());
			Camera.main.gameObject.transform.rotation = Quaternion.identity;
			this.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		
		List<string>robotsnames = new List<string>();

		for(int i = 0; i < 3; i++){
			robotsnames.Add("slot_" + (i+1) + "");
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
			if(this.mPlayerSlot){
				GameObject b = Instantiate(this.mPlayerSlot as GameObject);
				b.transform.SetParent(this.mContent.transform, false);		
				RectTransform rect = b.GetComponent<RectTransform>();
				b.GetComponent<DynamicListener>().mMessageParameter = (i+1).ToString();
				b.GetComponentInChildren<Text>().text = robotsnames[i];
				if(i != 0)
					rect.anchoredPosition = positions[i];

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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

		if (this.mRobotEditor != null && holder != null && this.mPart != PART.UNASSIGNED )
			// Change the robot part with the new object (assign is a callback)
			this.mRobotEditor.SetRobot (this.mPart, holder);

	}
	
	/// <summary>
	/// Method to change the stats of a part.
	/// </summary>
	/// <param name="part">Part.</param>
	/// <param name="robotName">Robot name.</param>
	private void ChangeStats(string robotName = "", Player p = null){

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
	
	private IEnumerator SaveRobot(){
		this.changing = true;
		this.mPart = PART.HEAD;
		this.EquipRobot(this.head);
		this.mPart = PART.LARM;
		this.EquipRobot(this.larm);
		this.mPart = PART.RARM;
		this.EquipRobot(this.rarm);
		this.mPart = PART.CAR;
		this.EquipRobot(this.car);
		yield return new WaitForSeconds(.5f);
		this.changing = false;
	}
	
	private IEnumerator MakeRobot(){
		Player holder = null;
		holder = this.mRobotEditor.gameObject.AddComponent<Player>();
		// Debug.Log("here");
		holder.GetPartObj(0).AddComponent<Head>();
		holder.GetPartObj(1).AddComponent<Larm>();
		GameObject bullet = (GameObject) GameUtilities.ReadResourceFile("Bullets/Bullet");
		holder.GetPartObj(1).GetComponent<Larm>().mBullet = bullet;
		holder.GetPartObj(2).AddComponent<Rarm>();
		holder.GetPartObj(2).GetComponent<Rarm>().mBullet = bullet;
		holder.GetPartObj(3).AddComponent<Car>();
		yield return new WaitForSeconds(.5f);
		holder.Initialize();		
		holder.isControllable = false;
		this.mPart = PART.HEAD;
		this.ChangeStats(this.head, holder);
		this.mPart = PART.LARM;
		this.ChangeStats(this.larm, holder);
		this.mPart = PART.RARM;
		this.ChangeStats(this.rarm, holder);
		this.mPart = PART.CAR;
		this.ChangeStats(this.car, holder);				
		this.mPart = PART.HEAD;
		GameUtilities.FindDeepChild(holder.transform, "Arms").gameObject.AddComponent<ArmController>();
		yield return new WaitForSeconds(.5f);
		this.ChangePage();
	}

	private void ChangePage() {
		GameObject.FindGameObjectWithTag("Menu").SendMessage("SetNextPage", "Enemy");			
	}
}
