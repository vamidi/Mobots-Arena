using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobots.UI {

	public delegate void OnEnterState(int i);
	public delegate void OnExitState(int i);
	public class MenuController : MonoBehaviour {

		[Header("Panels")] 
		public string[] mPageNames;
		public GameObject[] mPanels;
		public int mCurrentPageName;
		public int mPreviousPage;

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
			mEnterState = OnEnterPanel;
			mExitState = OnExitPanel;
			for (int i = 0; i < mPanels.Length; i++) {
				if (i != 0) {
					mPanels[i]?.GetComponent<Animator>()?.SetTrigger("Close");
				}
			}
		}

		// Update is called once per frame
		void Update() { }

		private void OnEnterPanel(int index) {
			mPanels[index]?.GetComponent<Animator>()?.SetTrigger("Open");
			mPanels[index]?.GetComponent<UIPanel>()?.OnEnterState();
		}

		private void OnExitPanel(int index) {
			mPanels[index]?.GetComponent<Animator>()?.SetTrigger("Close");
			mPanels[index]?.GetComponent<UIPanel>()?.OnExitState();
		}

		private void RevealPagePanel(int index) {
			// Exit state of the previous page
			mExitState?.Invoke(mPreviousPage);
			// Enter state of the current page
			mEnterState?.Invoke(index);
		}

		private void SetPreviousPage() {
			mExitState?.Invoke(mCurrentPageName);
			mEnterState?.Invoke(mPreviousPage);
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
	}
}