using System;
using System.Collections.Generic;
using Boomlagoon.JSON;
using Mobots.Robot;
using UnityEngine;

using Utils;

namespace Mobots.Robots {
	public class RobotsLibrary : MonoBehaviour {
		public static RobotsLibrary instance;              //Static instance of RobotLibrary which allows it to be accessed by any other script.

		/// <summary>
		/// The dictionary that contains the robots and their parts
		/// </summary>
		public Dictionary<string, JSONObject>RobotDictionary {
			get; private set;
		}

		public bool SetStatisByPart(string robotName = "", PartType part = PartType.Unassigned) {

			return true;
		}

		void Awake() {
			// check if instance already exists
			if (instance == null)

				//if not, set instance to this
				instance = this;

			// if instance already exists and it's not this:
			else if (instance != this)

				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);

			// sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);

			// get the robots out of the text asset
			string robots = GameUtilities.ReadTextAsset ("Robots/robots");
			RobotDictionary = new Dictionary<string, JSONObject>();
			JSONArray robotArr = JSONObject.Parse(robots).GetArray("robots");

			// fill them in the dictionary
			foreach(JSONValue o in robotArr) {
				// Set the json of the robot into the dictionary.
				RobotDictionary.Add(o.Obj.GetString("robotname"), o.Obj);
			}

		}
	}
}
