using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Boomlagoon.JSON;

public delegate void mAssignValues (PART part, string name);

/// <summary>
/// Editor class.
/// 
/// </summary>
public class Editor : MonoBehaviour {

	public GameObject HomeContent;

	/// <summary>
	/// The prefab to load the buttons of the robot parts
	/// </summary>
	public GameObject mButtonPrefab;

	/// <summary>
	/// The content of the menu
	/// </summary>
	private Dictionary<string, GameObject> mContents = new Dictionary<string, GameObject>();
	/// <summary>
	/// The values of the parts
	/// </summary>
	private List<RobotBtn> buttons = new List<RobotBtn>();
	/// <summary>
	/// Content manager delegate
	/// </summary>
	private delegate void mContentManager();
	/// <summary>
	/// Content manager
	/// </summary>
	private mContentManager mManager; 
	/// <summary>
	/// The manager to assign values to
	/// a particular robot part
	/// </summary>
	private mAssignValues mAssign;
	/// <summary>
	/// The current location
	/// </summary>
	private string mLocation = "Home";
	/// <summary>
	/// The previous location
	/// </summary>
	private string prevLocation = "";
	/// <summary>
	/// The new location
	/// </summary>
	private string newLocation = "";
	/// <summary>
	/// The robot class
	/// </summary>
	private Robot mRobot = null;

	/// <summary>
	/// Catch the click event of the buttons
	/// </summary>
	/// <param name="location">Location.</param>
	public void onClick(string location){
		prevLocation = mLocation;
		mLocation = location;
	}

	/// <summary>
	/// Change the robot part by name
	/// </summary>
	/// <param name="robotName">Robot name.</param>
	public void ChangeRobotByName(string robotName){
		GameObject holder = null;
		PART part = PART.HEAD;
		switch (mLocation.ToLower ()) {
		case "heads":
			holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_head", typeof(GameObject));	
			part = PART.HEAD;
			break;
		case "leftarms":
			holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_larm", typeof(GameObject));		
			part = PART.LARM;
			break;
		case "rightarms":
			holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_rarm", typeof(GameObject));		
			part = PART.RARM;
			break;
		case "cars":
			holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_car", typeof(GameObject));		
			part = PART.CAR;
			break;
		}
			
		if (holder != null && this.mRobot != null && part != null) {
			this.mRobot.SetRobot (part, robotName, holder, this.mAssign);

		}
	}

	// Use this for initialization
	void Start () {
		this.mRobot = GameObject.FindGameObjectWithTag ("Robot").GetComponent<Robot>();

		this.mContents.Add("HomeScreen", GameObject.Find("HomeContent"));
		this.mContents.Add("PartsContent", GameObject.Find("PartsContent"));
		this.mContents.Add("Heads", GameObject.Find("HeadsContent"));
		this.mContents.Add("LeftArms", GameObject.Find("LeftArmsContent"));
		this.mContents.Add("RightArms", GameObject.Find("RightArmsContent"));
		this.mContents.Add("Cars", GameObject.Find("CarsContent"));

		foreach(KeyValuePair<string, GameObject> entry in mContents) {
			if (entry.Value != null && entry.Key != "HomeScreen") {
				//Debug.Log (entry);
				entry.Value.SetActive (false);
			}
		}
			
		this.mManager = new mContentManager (Home);
		this.mAssign = new mAssignValues (ChangeStats);
        this.GetRobots();

	}
	
	// Update is called once per frame
        void Update () {
            if(this.mManager != null)
			    this.mManager ();
	} 

	private void GetRobots(){
		string robots = GameUtilities.ReadFile ("Robots/robots");
		JSONArray jArray = JSONObject.Parse(robots).GetArray("robots");
		Vector3 initHeadVector = new Vector3(91f, -43, 0);
		Vector3 initLeftVector = new Vector3(91f, -43, 0);
		Vector3 initRightVector = new Vector3(91f, -43, 0);
		Vector3 initCarVector = new Vector3(91f, -43, 0);

		Debug.Log(jArray);
		foreach(JSONValue o in jArray){
			// Do a check for the head
			JSONObject check = o.Obj;
			GameObject button = null;
			if(check.GetObject("parts").GetObject("head").GetBoolean("owned")){
			 	button = (GameObject)Instantiate(mButtonPrefab, initHeadVector, Quaternion.identity);
				button.transform.SetParent(this.mContents["Heads"].transform, false);
				RobotBtn btn = button.GetComponent<RobotBtn>();
				btn.SetName(check.GetString("robotname"));
				initHeadVector += new Vector3(0, -34f, 0);
			}

			if(check.GetObject("parts").GetObject("left").GetBoolean("owned")){
			 	button = (GameObject)Instantiate(mButtonPrefab, initLeftVector, Quaternion.identity);
				button.transform.SetParent(this.mContents["LeftArms"].transform, false);
				RobotBtn btn = button.GetComponent<RobotBtn>();
				btn.SetName(check.GetString("robotname"));
				initLeftVector += new Vector3(0, -34f, 0);
			}
			
			if(check.GetObject("parts").GetObject("right").GetBoolean("owned")){
			 	button = (GameObject)Instantiate(mButtonPrefab, initRightVector, Quaternion.identity);
				button.transform.SetParent(this.mContents["RightArms"].transform, false);
				RobotBtn btn = button.GetComponent<RobotBtn>();
				btn.SetName(check.GetString("robotname"));
				initRightVector += new Vector3(0, -34f, 0);
			}

			if(check.GetObject("parts").GetObject("car").GetBoolean("owned")){
			 	button = (GameObject)Instantiate(mButtonPrefab, initCarVector, Quaternion.identity);
				button.transform.SetParent(this.mContents["Cars"].transform, false);
				RobotBtn btn = button.GetComponent<RobotBtn>();
				btn.SetName(check.GetString("robotname"));
				initCarVector += new Vector3(0, -34f, 0);
			}
		}
		// List<JSONObject>values = null;
		// Dictionary<string, JSONObject>mArrary = GameUtilities.GetJSON(new JSONObject(robots));
		// foreach(KeyValuePair<string, JSONObject> entry in mArrary) {
		// 	Debug.Log(entry.Key);
		// 	Debug.Log(entry.Value); // JSON Array
		// 	foreach(JSONObject j in entry.Value.list){
		// 		Dictionary<string, JSONObject>m    = GameUtilities.GetJSON(j);
		// 	}
		// }
	}

	/// <summary>
	/// Method to disable the content.
	/// This will be based on the location parameter
	/// </summary>
	/// <param name="location">Location.</param>
	private void DisableContent(string location){
		switch (location.ToLower ()) {
		case "home":
			mContents ["HomeScreen"].SetActive (false);
			break;
		case "parts":
			mContents ["PartsContent"].SetActive (false);
			break;
		case "heads":
			mContents ["Heads"].SetActive (false);
			break;
		case "leftarms":
			mContents ["LeftArms"].SetActive (false);
			break;
		case "rightarms":
			mContents ["RightArms"].SetActive (false);
			break;
		case "cars":
			mContents ["Cars"].SetActive (false);
			break;
		}
	}
	/// <summary>
	/// Method to enable the content.
	/// This will be based on the location parameter
	/// </summary>
	/// <param name="location">Location.</param>
	private void EnableContent(string location){
		switch (location.ToLower ()) {
		case "home":
			mContents ["HomeScreen"].SetActive (true);
			break;
		case "parts":
			mContents ["PartsContent"].SetActive (true);
			break;
		case "heads":
			mContents ["Heads"].SetActive (true);
			break;
		case "leftarms":
			mContents ["LeftArms"].SetActive (true);
			break;
		case "rightarms":
			mContents ["RightArms"].SetActive (true);
			break;
		case "cars":
			mContents ["Cars"].SetActive (true);
			break;
		}
	}

	#region ContentManaging
	// Content methods
	private void Home() {
		//Debug.Log ("Delegate Home is called");

		mManager = Home;
		switch (mLocation.ToLower ()) {
			case "home":
				mManager += HomeScreen;
			break;
			case "parts":
				mManager += Parts;	
			break;
			case "heads":
				mManager += Heads;	
			break;
			case "leftarms":
				mManager += LeftArms;	
			break;
			case "rightarms":
				mManager += RightArms;	
			break;
			case "cars":
				mManager += Cars;	
			break;
		}
	}
	private void HomeScreen(){
		//Debug.Log ("Delegate Home is called");
		newLocation = "Home";
		DisableContent (prevLocation);
		EnableContent (newLocation);
	}
	private void Parts() {
		//Debug.Log ("Delegate Parts is called");
		newLocation = "Parts";
		DisableContent (prevLocation);
		EnableContent (newLocation);


	}
	private void Heads() {
		//Debug.Log ("Delegate Heads is called");
		newLocation = "Heads";
		DisableContent (prevLocation);
		EnableContent (newLocation);
	}
	private void LeftArms() {
		//Debug.Log ("Delegate Left Arms is called");
		newLocation = "LeftArms";
		DisableContent (prevLocation);
		EnableContent (newLocation);
	}
	private void RightArms() {
		//Debug.Log ("Delegate Right arms is called");
		newLocation = "RightArms";
		DisableContent (prevLocation);
		EnableContent (newLocation);	
	}
	private void Cars() {
		//Debug.Log ("Delegate Cars is called");
		newLocation = "Cars";
		DisableContent (prevLocation);
		EnableContent (newLocation);
	}
	#endregion

	/// <summary>
	/// Method to change the stats of a part.
	/// </summary>
	/// <param name="part">Part.</param>
	/// <param name="robotName">Robot name.</param>
	private void ChangeStats(PART part, string robotName = ""){
                
		if (robotName == "")
			return;
		string head = "", arm = "", car = "";
		JSONObject json;
		switch (part) {
		case PART.HEAD:
			head = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
			json = JSONObject.Parse(head);
			mRobot.SetValue (part, "SetHealth", (float)json.GetNumber("health"));
			mRobot.SetValue (part, "SetArmor", (float)json.GetNumber("armor"));
			mRobot.SetValue (part, "SetStrength", (float)json.GetNumber("armorstrength"));
			mRobot.SetValue (part, "SetWeight", (int)json.GetNumber("weight"));
			break;
		case PART.LARM:
			arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
			json = JSONObject.Parse(arm);
			mRobot.SetValue (part, "SetHealth", (float)json.GetNumber("health"));
			mRobot.SetValue (part, "SetArmor", (float)json.GetNumber("armor"));
			mRobot.SetValue (part, "SetStrength", (float)json.GetNumber("armorstrength"));
			mRobot.SetValue (part, "SetWeight", (int)json.GetNumber ("weight"));
			mRobot.SetValue (part, "SetDamagePerRound", (float)json.GetNumber ("damageperround"));
			mRobot.SetValue (part, "SetRoundsPerSecond", (float)json.GetNumber ("roudspersecond"));
			mRobot.SetValue (part, "SetAccuracy", (float)json.GetNumber ("accuracy"));
			break;
		case PART.RARM:
			arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
			json = JSONObject.Parse(arm);
			mRobot.SetValue (part, "SetHealth", (float)json.GetNumber ("health"));
			mRobot.SetValue (part, "SetArmor", (float)json.GetNumber ("armor"));
			mRobot.SetValue (part, "SetStrength", (float)json.GetNumber ("armorstrength"));
			mRobot.SetValue (part, "SetWeight", (int)json.GetNumber ("weight"));
			mRobot.SetValue (part, "SetDamagePerRound", (float)json.GetNumber ("damageperround"));
			mRobot.SetValue (part, "SetRoundsPerSecond", (float)json.GetNumber ("roudspersecond"));
			mRobot.SetValue (part, "SetAccuracy", (float)json.GetNumber("accuracy"));
			break;
		case PART.CAR:
			car = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
			json = JSONObject.Parse(car);
			mRobot.SetValue (part, "SetHealth", (float)json.GetNumber ("health"));
			mRobot.SetValue (part, "SetArmor", (float)json.GetNumber ("armor"));
			mRobot.SetValue (part, "SetStrength", (float)json.GetNumber ("armorstrength"));
			mRobot.SetValue (part, "SetWeight", (int)json.GetNumber ("weight"));
			mRobot.SetValue (part, "SetSpeed", (float)json.GetNumber ("speed"));
			mRobot.SetValue (part, "SetJumpStrength", (float)json.GetNumber ("jumpstrength"));
			break;
		}
	}
}
