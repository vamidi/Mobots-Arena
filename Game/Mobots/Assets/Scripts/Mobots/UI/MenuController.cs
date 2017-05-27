using System;
using UnityEngine;

namespace Mobots.UI {
	/// <summary>
	/// Initialize state of the page
	/// </summary>
	public delegate void OnEnterState();
	/// <summary>
	/// Exit state of the page
	/// </summary>
	public delegate void OnExitState();
	public class MenuController : MonoBehaviour {

		public string mTag = "Menu";
		public OnExitState mExitState;
		// Add all menu's that must work as pages
		public GameObject[] mPages;
		// must corrospond to the pages
		public String[] mPageNames;

		/// <summary>
		/// Previous page name
		/// </summary>
		private string mPreviousPage;
		/// <summary>
		/// Current page name
		/// </summary>
		private string mCurrentPageName;
		/// <summary>
		/// Current page object of the menu
		/// </summary>
		private GameObject mCurrentPage;
		/// <summary>
		/// Robot editor
		/// </summary>
		private MobotsEditor mMobotsEditor;

		/// <summary>
		/// Quit the game
		/// </summary>
		public void QuitGame() {
//				Debug.Log("QUIT");
			Application.Quit();
		}

		// Use this for initialization
		private void Start() {
			/**
			 * if the tag is not empty and if the tag of this game is
			 * different than what we need.
			*/
			if (mTag != "" && !gameObject.CompareTag(mTag)) {
				gameObject.tag = mTag;
			}
			
			this.mCurrentPageName = this.mPageNames[0];
			this.SetCurrentPage(this.mPages[0]);
		}

		// Update is called once per frame
		void Update() { }
		
		private void SetCurrentPage (GameObject page) {
			GameObject p = Instantiate(page as GameObject);
			p.transform.SetParent(this.transform);
			RectTransform rt = p.GetComponent<RectTransform>();
			Transition t = p.GetComponent<Transition>();
			rt.offsetMax = new Vector2(t.mSpawnPosition.x, t.mSpawnPosition.y);
			rt.offsetMin = new Vector2(t.mSpawnPosition.x, t.mSpawnPosition.y);
//			rt.transform.localPosition = Vector3.zero;
			p.transform.localScale = Vector3.one;
			this.mCurrentPage = p;
				
		}

		private void SetPreviousPage () {
			for(int i = 0; i < this.mPageNames.Length; i++){
				if(this.mPreviousPage == this.mPageNames[i]){
					this.RevealPageInUI(i);
				}
			}
		}
			
		private void SetNextPage (string PAGE_CODE) {
			for(int i = 0; i < this.mPageNames.Length; i++){
				if(PAGE_CODE == this.mPageNames[i]){
					this.mPreviousPage = this.mCurrentPageName;
					this.RevealPageInUI(i);
				}
			}
		}
			
		private void RevealPageInUI (int index) {
			this.mCurrentPageName = this.mPageNames[index];
			Transition t = this.mCurrentPage.GetComponent<Transition>();
			t.StartTransition();
			this.mCurrentPage = t.InitializeTransitionPage(this.mPages[index]);
			RectTransform rt = this.mCurrentPage.GetComponent<RectTransform>();
			t = this.mCurrentPage.GetComponent<Transition>();
			rt.offsetMax = new Vector2(t.mSpawnPosition.x, t.mSpawnPosition.y);
			rt.offsetMin = new Vector2(t.mSpawnPosition.x, t.mSpawnPosition.y);
			rt.transform.localPosition = Vector3.zero;
			if(this.mExitState != null){
				this.mExitState();
				this.mExitState = null;
			}
		}
	}
}
