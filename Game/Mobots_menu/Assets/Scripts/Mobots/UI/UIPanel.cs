using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Boomlagoon.JSON;

namespace Mobots.UI {
	public class UIPanel : MonoBehaviour {
		public GameObject mGameObject;
		public GameObject mContentPanel;
		public GameObject mRow;
		public GameObject mButton;
		public Sprite mStandardSprite;
		public Vector3 mTransform;
		public MenuState mMenuState = MenuState.Main;

		private RobotController mRobotController;
		private Dictionary<string, JSONObject>mLevels = new Dictionary<string,JSONObject>();
		private bool mIsLoaded;
		
		public void Start() {
			mTransform = mGameObject.GetComponent<Transform>().eulerAngles;
			mRobotController = RobotController.Instance;
		}
		
		public void OnEnterState() {
			switch (mMenuState) {
				case MenuState.Main:
					if(mGameObject)
						mGameObject.SetActive(false);
					break;
				case MenuState.Editor:
					if (mGameObject)
						mGameObject.SetActive(true);
					break;
			}
		}

		public void OnRenderState() {
			switch (mMenuState) {
				case MenuState.Main:
					if(mIsLoaded)
						return;
					if(mGameObject)
						mGameObject.SetActive(false);

					mIsLoaded = true;
					break;
				case MenuState.Editor:
					if (mIsLoaded)
						return;
					RenderRobots();
					mIsLoaded = true;
					break;
				case MenuState.LevelSelect:
					if(mIsLoaded)
						return;
					RenderLevels();
					mIsLoaded = true;
					break;
			}
		}
		
		public void OnExitState() {
			if (mGameObject) {
				mGameObject.transform.eulerAngles = mTransform;
				mGameObject.SetActive(false);
			}

			mIsLoaded = false;
		}

		private void RenderRobots() {
			float rows = Mathf.Floor( (float)mRobotController.Robots.Length / 3);
			int columns = 3;
			for(int row = 0; row <= rows; row++) {
				GameObject uiRow = Instantiate(mRow, Vector3.zero, Quaternion.identity);
				uiRow.transform.SetParent(mContentPanel.transform);
				var t = uiRow.GetComponent<RectTransform>();
				t.localPosition = Vector3.one;
				t.localRotation = Quaternion.identity;
				t.localScale = Vector3.one;
				for(int column = 0; column < columns; column++) {
					if ( (row * columns + column) <= (mRobotController.Robots.Length - 1) ) {
						GameObject btnPart = Instantiate(mButton, Vector3.zero, Quaternion.identity);
						btnPart.transform.SetParent(uiRow.transform);
						var b = btnPart.GetComponent<RectTransform>();
						b.localPosition = Vector3.one;
						b.localRotation = Quaternion.identity;
						b.localScale = Vector3.one;
						b.GetComponent<DynamicListener>().mMessageParameter = mRobotController.Robots[row * columns + column].Obj.GetString("robotname");
						b.GetComponentInChildren<Text>().text = mRobotController.Robots[row * columns + column].Obj.GetString("robotname");
					}
				}
			}
		}

		private void RenderLevels() {
			string levels = GameUtilities.ReadTextAsset ("Arenas/levels");
			if(levels != "") {
				JSONArray arr = JSONObject.Parse(levels).GetArray("levels");
				foreach(JSONValue o in arr){
					this.mLevels.Add(o.Obj.GetObject("level").GetString("levelname"), o.Obj.GetObject("level"));
				}		

				for(int i = 0; i < arr.Length; i++) {
					JSONValue o = arr[i];
					GameObject b = Instantiate(mButton as GameObject);
					if(b) {
						string name = o.Obj.GetObject("level").GetString("levelname");
						b.transform.SetParent(mContentPanel.transform, false);		
						RectTransform rect = b.GetComponent<RectTransform>();
						b.GetComponent<DynamicListener>().mMessageParameter = name;
						b.GetComponentInChildren<Text>().text = name;
						string path = "Arenas/" + name + "/Thumbnail/" + name + "_thumb";
						Sprite img = GameUtilities.GetImageSprite(path);
						if(img)
							b.GetComponentInChildren<Image>().sprite = img;
						else
							b.GetComponentInChildren<Image>().sprite = mStandardSprite;

					}
				}	
			}
		}
	}
}
