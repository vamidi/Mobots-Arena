using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Mobots.UI {

	public delegate void OnEnterState(int i);
	public delegate void OnExitState(int i);
	public class MenuController : MonoBehaviour {

		[Header("Panels")] 
		public string[] mPageNames;
		public GameObject[] mPanels;
		public int mCurrentPageName = 0;
		public int mPreviousPage = 0;
		
		[Header("Robot Settings")] 
		public GameObject mRobot;
		
		[Header("States")]
		public OnEnterState mEnterState;
		public OnExitState mExitState;
		
		public void QuitGame() {
			Debug.Log("You have quite the game");
		}
		
		// Use this for initialization
		void Start() {
			for (int i = 0; i < mPanels.Length; i++) {
				if (i != 0) {
					mPanels[i].SetActive(false);
				}
			}
			mEnterState = OnEnterPanel;
			mExitState = OnExitPanel;
			if (mRobot) {
				mRobot.SetActive(false);
			}
		}

		// Update is called once per frame
		void Update() { }

		private void OnEnterPanel(int index) {
			if (mPanels[index].GetComponent<Animator>()) {
				mPanels[index].GetComponent<Animator>().SetTrigger("Open");
			}
		}
		
		private void OnExitPanel(int index) {
			if (mPanels[index].GetComponent<Animator>()) {
				mPanels[index].GetComponent<Animator>().SetTrigger("Close");
			}
		}

		private void RevealPagePanel(int index, bool previous = false) {
			// Exit state of the previous page
			mExitState?.Invoke(mPreviousPage);

			if (previous) {
//				mPanels[mCurrentPageName].SetActive(false);
//				mPanels[index].SetActive(true);
				mCurrentPageName = index;
			} else {
//				mPanels[mPreviousPage].SetActive(false);
//				mPanels[index].SetActive(true);
			}
			// Enter state of the current page
			mEnterState?.Invoke(index);
		}
		
		private void SetPreviousPage () {
			for(int i = 0; i < mPageNames.Length; i++) {
				if(mPreviousPage == i) {
					RevealPagePanel(i, true);
				}
			}
		}
			
		private void SetNextPage (string pageCode) {
			for(int i = 0; i < mPageNames.Length; i++) {
				if(pageCode == mPageNames[i]) {
					mPreviousPage = mCurrentPageName;
					mCurrentPageName = i;
					RevealPagePanel(i);
				}
			}
		}
	}
}