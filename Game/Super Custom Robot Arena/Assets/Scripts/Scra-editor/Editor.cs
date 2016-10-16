using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Editor : MonoBehaviour {

	private Dictionary<string, GameObject> mContents = new Dictionary<string, GameObject>();
	private delegate void mContentManager();
	private mContentManager mManager; 
	private string mLocation = "Home";
	private string prevLocation = "Home";
	private string newLocation = "";

	public void onClick(string location){
		prevLocation = mLocation;
		mLocation = location;
	}

	// Use this for initialization
	void Start () {
		mContents.Add("HomeScreen", GameObject.Find("HomeContent"));
		mContents.Add("PartsContent", GameObject.Find("PartsContent"));
		mContents.Add("Heads", GameObject.Find("HeadsContent"));
		mContents.Add("LeftArms", GameObject.Find("LeftArmsContent"));
		mContents.Add("RightArms", GameObject.Find("RightArmsContent"));
		mContents.Add("Car", GameObject.Find("CarsContent"));

		foreach(KeyValuePair<string, GameObject> entry in mContents) {
			if (entry.Value != null && entry.Key != "HomeScreen") {
				//Debug.Log (entry);
				entry.Value.SetActive (false);
			}
		}

		mManager = new mContentManager (Home);
	}
	
	// Update is called once per frame
	void Update () {

		if(mManager != null)
			mManager ();
	} 

	private void Home() {
		//Debug.Log ("Delegate Home is called");

		mManager = Home;

		switch (mLocation.ToLower()) {
		case "home":
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
	private void Parts() {
		Debug.Log ("Delegate Parts is called");
		newLocation = "Parts";
		DisableContent (prevLocation);
		EnableContent (newLocation);


	}

	private void Heads() {
		Debug.Log ("Delegate Heads is called");
		newLocation = "Parts";
		DisableContent (prevLocation);
	}

	private void LeftArms() {
		Debug.Log ("Delegate Left Arms is called");
		newLocation = "Parts";
		DisableContent (prevLocation);
	}

	private void RightArms() {
		Debug.Log ("Delegate Right arms is called");
		newLocation = "Parts";
		DisableContent (prevLocation);
	}

	private void Cars() {
		Debug.Log ("Delegate Cars is called");
		newLocation = "Parts";
		DisableContent (prevLocation);
	}

}
