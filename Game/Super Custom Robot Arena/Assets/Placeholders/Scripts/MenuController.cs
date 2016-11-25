﻿using UnityEngine;
using System.Collections;
using System;

namespace SCRA {

	namespace UI {

		/// <summary>
		/// Menu controller.that is responsible for determing
		/// which pages are be loaded
		/// </summary>
		public class MenuController : MonoBehaviour {
		
			// Add all menu's that must work as pages
			public GameObject[] mPages;
			// must corrospond to the pages 
			public String[] mPageNames;
			
			private GameObject mCurrentPage;
			// for pause or in other game menus
			private bool mEnterScreen = false;
		
			public bool GetEnteredScreen {
				get { return this.mEnterScreen; }
				set { this.mEnterScreen = value; } 
			}
			
			// Use this for initialization
			void Start () {
				this.SetCurrentPage(this.mPages[0]);
			}
			
			// Update is called once per frame
			void Update () {
			
			}
			
			private void SetCurrentPage (GameObject page) {
				GameObject p = Instantiate(page as GameObject);
				
				p.transform.SetParent(this.transform);
				RectTransform tr = p.GetComponent<RectTransform>();
				// Transition t = p.GetComponent<Transition>();
				
				this.mCurrentPage = p;
				
			}
			
			private void SetNextPage () {
				
			}
			
			private void RevealNewPage () {
				
			}
		}
	}
}