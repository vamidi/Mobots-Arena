using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobots.managers {

	public class PlayerManager : MonoBehaviour {
		[Header("Robot Settings")] 
		public string mCurrentRobotName; 
		
		// Use this for initialization
		void Start() {
			DontDestroyOnLoad(gameObject);
			mCurrentRobotName = (PlayerPrefManager.GetValue("mCurrRobot", PrefTypes.String) != null) ? (string)PlayerPrefManager.GetValue("mCurrRobot", PrefTypes.String) : "MKVII";
		}

		// Update is called once per frame
		void Update() { }
		
		
	}
}