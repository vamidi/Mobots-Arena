using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Boomlagoon.JSON;

public class GameManager : MonoBehaviour {
	
	/// <summary>
	/// The robot array.
	/// </summary>
	public JSONArray robots {
		get; private set;
	}
	
	/// <summary>
	/// The dictionary that contains the robots and their parts
	/// </summary>
	public Dictionary<string, JSONObject>robotDictionary {
		get; private set;
	}
	
	public string robotname = "MKVII";
	public GameObject enemy;
	
	void Awake(){
		string robots = GameUtilities.ReadResource ("Robots/robots");
		this.robotDictionary = new Dictionary<string, JSONObject>();
		this.robots = JSONObject.Parse(robots).GetArray("robots");
	
		foreach(JSONValue o in this.robots) {
			// Set the json of the robot into the dictionary.
			this.robotDictionary.Add(o.Obj.GetString("robotname"), o.Obj);
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
