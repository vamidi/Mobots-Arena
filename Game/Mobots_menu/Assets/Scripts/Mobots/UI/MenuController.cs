using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Boomlagoon.JSON;

namespace Mobots.UI {

	using Robots;

	public delegate void OnEnterState(int i);
	public delegate void OnExitState(int i);

	public enum MenuState {
		Main,
		Editor,
		EnemySelect,
		LevelSelect
	}
	public class MenuController : MonoBehaviour {

		[Header("Panels", order=0)] 
		public string[] mPageNames;
		[Header("Panels", order=1)] 
		public GameObject[] mPanels;
		[Header("Panels", order=2)] 
		public int mCurrentPageName;
		[Header("Panels", order=3)] 
		public int mPreviousPage;

		[Header("Robot Settings")] 
		public GameObject mRobot;
		[Header("Robot Settings", order=1)] 
		public string mCurrentRobotName;
		[Header("Robot Settings", order=2)] 
		public Text mTitle;

		[Header("States", order=0)] 
		public OnEnterState mEnterState;
		[Header("States", order=1)] 
		public OnExitState mExitState;
		[Header("States", order=2)] 
		public PART mCurrentPart = PART.Head;


		[Header("Editor")] 
		public RobotController mRobotController;


		public void QuitGame() {
			Debug.Log("You have quite the game");
		}

		public void ChangeSection(string part) {
			switch(part.ToLower()) {
				case "head":
					mCurrentPart = PART.Head;
					mTitle.text = "Head Parts";
					break;
				case "larm":
					mCurrentPart = PART.Larm;
					mTitle.text = "Left Parts";
					break;
				case "rarm":
					mCurrentPart = PART.Rarm;
					mTitle.text = "Right Parts";
					break;
				case "car":
					mCurrentPart = PART.Car;
					mTitle.text = "Car Parts";
					break;
				default:
					mCurrentPart = PART.Head; 
					mTitle.text = "Head Parts";
					break;
			}

		}

		public void SelectLevel(string levelname) {
			// GameObject.FindObjectOfType<SceneLoader>().LoadNewLevel(this.mLevels[levelname].GetString("scene"));
		}

		// Use this for initialization
		void Start() {
			mRobotController = RobotController.Instance;
			mEnterState = OnEnterPanel;
			mExitState = OnExitPanel;
			for (int i = 0; i < mPanels.Length; i++) {
				if (i != 0) {
					if(mPanels[i] && mPanels[i].GetComponent<Animator>())
						mPanels[i].GetComponent<Animator>().SetTrigger("Close");
				} else {
					if(mPanels[i] && mPanels[i].GetComponent<UIPanel>())
						mPanels[i].GetComponent<UIPanel>().OnRenderState();
				}
			}
			ChangeSection("head");
		}

		// ROBOT ADJUSTMENTS
		public void ChangePart(string robotName) {
			// holder for the part
			GameObject holder = null;

			switch (mCurrentPart) {
				case PART.Head:
					holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_head", typeof(GameObject));	
					// mCurrentTextParts[0].text = robotName + " head";
					this.mCurrentRobotName = robotName;
					break;
				case PART.Larm:
					holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_larm", typeof(GameObject));		
					// this.mCurrentTextParts[1].text = robotName + " left arm";
					break;
				case PART.Rarm:
					holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_rarm", typeof(GameObject));		
					// this.mCurrentTextParts[2].text = robotName + " right arm";
					break;
				case PART.Car:
					holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_car", typeof(GameObject));		
					// this.mCurrentTextParts[3].text = robotName + " car";
					break;
			}


			if (mRobotController != null && holder != null && mCurrentPart != PART.Unassigned )
				// Change the robot part with the new object (assign is a callback)
				mRobotController.SetRobot (mCurrentPart, holder);
		}

		private void OnEnterPanel(int index) {
			if (mPanels[index] && mPanels[index].GetComponent<Animator>()) {
				mPanels[index].GetComponent<Animator>().SetTrigger("Open");
				if (mPanels[index].GetComponent<UIPanel>()) {
					UIPanel p = mPanels[index].GetComponent<UIPanel>();
					if (p) {
						p.OnEnterState();
						p.OnRenderState();
					}
				}
			}
		}

		private void OnExitPanel(int index) {
			if (mPanels[index] && mPanels[index].GetComponent<Animator>()) {
				mPanels[index].GetComponent<Animator>().SetTrigger("Close");
				if(mPanels[index].GetComponent<UIPanel>())
					mPanels[index].GetComponent<UIPanel>().OnExitState();
			}
		}

		private void RevealPagePanel(int index) {
			// Exit state of the previous page
			if(mExitState != null)
				mExitState.Invoke(mPreviousPage);
			// Enter state of the current page
			if (mEnterState != null)
				mEnterState.Invoke(index);
		}

		private void SetPreviousPage() {
			if(mExitState != null)
				mExitState.Invoke(mCurrentPageName);
			if(mEnterState != null)
				mEnterState.Invoke(mPreviousPage);
			mCurrentPageName = mPreviousPage;
		}

		private void SetNextPage(string pageCode) {
			for (int i = 0; i < mPageNames.Length; i++) {
				if (pageCode == mPageNames[i]) {
					mPreviousPage = mCurrentPageName;
					mCurrentPageName = i;
					RevealPagePanel(i);
				}
			}
		}

		private void SelectRobot() {
			SetNextPage("LevelSelect");
		}



	}
}