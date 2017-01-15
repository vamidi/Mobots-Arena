using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MBA {

	namespace UI {
		
		public delegate void OnExitState();
		/// <summary>
		/// Menu controller.that is responsible for determing
		/// which pages are be loaded
		/// </summary>
		public class MenuController : MonoBehaviour {
		
			public OnExitState mExitState;			
			// Add all menu's that must work as pages
			public GameObject[] mPages;
			// must corrospond to the pages 
			public String[] mPageNames;
			
			private string mPreviousPage;
			private string mCurrentPageName;
			private GameObject mCurrentPage;
			// for pause or in other game menus
			private bool mEnterScreen = false;
		
			public bool GetEnteredScreen {
				get { return this.mEnterScreen; }
				set { this.mEnterScreen = value; } 
			}
			
			public void QuitGame() {
//				Debug.Log("QUIT");
				Application.Quit();
			}
			
			// Use this for initialization
			void Start () {
//				DontDestroyOnLoad(this.gameObject);
				this.mCurrentPageName = this.mPageNames[0];
				this.SetCurrentPage(this.mPages[0]);
			}
			
			// Update is called once per frame
			void Update () {
			
			}
			
			private void SetCurrentPage (GameObject page) {
				GameObject p = Instantiate(page as GameObject);
				p.transform.SetParent(this.transform);
				RectTransform rt = p.GetComponent<RectTransform>();
				Transition t = p.GetComponent<Transition>();
				rt.offsetMax = new Vector2(t.mSpawnPosition.x, t.mSpawnPosition.y);
				rt.offsetMin = new Vector2(t.mSpawnPosition.x, t.mSpawnPosition.y);
				rt.transform.localPosition = Vector3.zero;
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
}
