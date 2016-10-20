using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void mAssignValues (PART part, string name);
/// <summary>
/// Editor class.
/// 
/// </summary>
public class Editor : MonoBehaviour {

	/// <summary>
	/// The content of the menu
	/// </summary>
	private Dictionary<string, GameObject> mContents = new Dictionary<string, GameObject>();
	/// <summary>
	/// The values of the parts
	/// </summary>
	private Dictionary<string, object> mValues = new Dictionary<string, object> ();
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
			
		if (holder != null && mRobot != null && part != null) {
			mRobot.SetRobot (part, robotName, holder, mAssign);

		}
	}

	// Use this for initialization
	void Start () {
		mRobot = GameObject.FindGameObjectWithTag ("Robot").GetComponent<Robot>();

		mContents.Add("HomeScreen", GameObject.Find("HomeContent"));
		mContents.Add("PartsContent", GameObject.Find("PartsContent"));
		mContents.Add("Heads", GameObject.Find("HeadsContent"));
		mContents.Add("LeftArms", GameObject.Find("LeftArmsContent"));
		mContents.Add("RightArms", GameObject.Find("RightArmsContent"));
		mContents.Add("Cars", GameObject.Find("CarsContent"));

		foreach(KeyValuePair<string, GameObject> entry in mContents) {
			if (entry.Value != null && entry.Key != "HomeScreen") {
				//Debug.Log (entry);
				entry.Value.SetActive (false);
			}
		}
			
		mManager = new mContentManager (Home);
		mAssign = new mAssignValues (ChangeStats);
	}
	
	// Update is called once per frame
	void Update () {

		if(mManager != null)
			mManager ();
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
		mValues.Clear ();
		switch (part) {
		case PART.HEAD:
			head = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
			mValues = GameUtilities.GetJSONData (new JSONObject (head));
			mRobot.SetValue (part, "SetHealth", mValues ["health"]);
			mRobot.SetValue (part, "SetArmor", mValues ["armor"]);
			mRobot.SetValue (part, "SetStrength", mValues ["armorstrength"]);
			mRobot.SetValue (part, "SetWeight", mValues ["weight"]);
			break;
		case PART.LARM:
			arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
			mValues = GameUtilities.GetJSONData (new JSONObject (arm));
			mRobot.SetValue (part, "SetHealth", mValues ["health"]);
			mRobot.SetValue (part, "SetArmor", mValues ["armor"]);
			mRobot.SetValue (part, "SetStrength", mValues ["armorstrength"]);
			mRobot.SetValue (part, "SetWeight", mValues ["weight"]);
			mRobot.SetValue (part, "SetDamagePerRound", mValues ["damageperround"]);
			mRobot.SetValue (part, "SetRoundsPerSecond", mValues ["roudspersecond"]);
			mRobot.SetValue (part, "SetAccuracy", mValues ["accuracy"]);
			break;
		case PART.RARM:
			arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
			mValues = GameUtilities.GetJSONData (new JSONObject (arm));
			mRobot.SetValue (part, "SetHealth", mValues ["health"]);
			mRobot.SetValue (part, "SetArmor", mValues ["armor"]);
			mRobot.SetValue (part, "SetStrength", mValues ["armorstrength"]);
			mRobot.SetValue (part, "SetWeight", mValues ["weight"]);
			mRobot.SetValue (part, "SetDamagePerRound", mValues ["damageperround"]);
			mRobot.SetValue (part, "SetRoundsPerSecond", mValues ["roudspersecond"]);
			mRobot.SetValue (part, "SetAccuracy", mValues ["accuracy"]);
			break;
		case PART.CAR:
			car = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
			mValues = GameUtilities.GetJSONData (new JSONObject (car));
			mRobot.SetValue (part, "SetHealth", mValues ["health"]);
			mRobot.SetValue (part, "SetArmor", mValues ["armor"]);
			mRobot.SetValue (part, "SetStrength", mValues ["armorstrength"]);
			mRobot.SetValue (part, "SetWeight", mValues ["weight"]);
			mRobot.SetValue (part, "SetSpeed", mValues ["speed"]);
			mRobot.SetValue (part, "SetJumpStrength", mValues ["jumpstrength"]);
			break;
		}
	}
}
