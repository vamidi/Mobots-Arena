using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;
using Mobots.Robot;
using Mobots.Robots;
using UnityEngine;

namespace Mobots.Editors {
	public class RobotEditor : MonoBehaviour {
		public Robot.Robot mRobot;

		private RobotsLibrary mRobotsLibrary;
		private PartType currentPartType = PartType.Unassigned;
		// Use this for initialization
		void Start() {
			mRobotsLibrary = FindObjectOfType<RobotsLibrary>();
		}

		// Update is called once per frame
		void Update() { }
	}
}
