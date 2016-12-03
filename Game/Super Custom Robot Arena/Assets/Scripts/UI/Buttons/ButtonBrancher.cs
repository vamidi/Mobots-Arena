using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Boomlagoon.JSON;
using System.IO;

namespace MBA {

	namespace UI {
		
		public class ButtonBrancher : MonoBehaviour {
		
			public class ButtonScaler {
				SCALEMODE mScaleMode;
				Vector2 mReferenceButtonSize;

				[HideInInspector]
				public Vector2 mReferenceScreenSize;
				public Vector2 mNewButtonSize;

				public void Initialize (Vector2 refButton, Vector2 screensize, int scalemode) {
					this.mScaleMode = (SCALEMODE)scalemode;
					this.mReferenceButtonSize = refButton;
					this.mReferenceScreenSize = screensize;
					this.SetButtonSize();
				}

				void SetButtonSize () {
					if(this.mScaleMode == SCALEMODE.INDEPENDENTWIDTHHEIGHT){
						this.mNewButtonSize.x = (this.mReferenceButtonSize.x * Screen.width) / this.mReferenceScreenSize.x;
						this.mNewButtonSize.y = (this.mReferenceButtonSize.y * Screen.height) / this.mReferenceScreenSize.y; 						
					}else if(this.mScaleMode == SCALEMODE.MATCHWIDTHHEIGHT){
						this.mNewButtonSize.x = (this.mReferenceButtonSize.x * Screen.width) / this.mReferenceScreenSize.x;
						this.mNewButtonSize.y = this.mNewButtonSize.x;
					}
				}
			}
			
			[System.Serializable]
			public class RevealSettings {
				public enum REVEALOPTION { LINEAR, CIRCULAR };
				public REVEALOPTION mRevealOption;
				public float mTranslateSmooth = 5f;
				public float mFadeSmooth = 0.01f;
				public bool mRevealOnStart = false;

				[HideInInspector]
				public bool mOpening = false;
				[HideInInspector]
				public bool mSpawned = false;
			}
			
			[System.Serializable]
			public class LinearSpawner {
				public enum REVEALSTYLE { SLIDETOPOSITION, FADEINATPOSITION };
				public REVEALSTYLE mRevealStyle;
				public Vector2 mDirection = new Vector2(0, 1f); // Slidedown
				public float mBaseButtonSpacing = 5f;
				public int mButtonOffset = 0;

				[HideInInspector]
				public float mButtonSpacing = 5f;

				public void FitSpacingToScreenSize (Vector2 screensize) {
					float refScreenFloat = (screensize.x + screensize.y) / 2;
					float screenfloat = (Screen.width + Screen.height) / 2;
					this.mButtonSpacing = (this.mBaseButtonSpacing * screenfloat ) / refScreenFloat;
				}	
			}
			
			public Sprite mDefaultHolder;
			public SCALEMODE mScaleMode;
			public GameObject mButtonRefs;
			public int mCount = 1;
			[HideInInspector]
			public List<GameObject>mButtons;
			public Vector2 mReferenceButtonSize;
			public Vector2 mReferenceScreenSize;
			public ButtonScaler mButtonScaler = new ButtonScaler();
			public RevealSettings mRevealSettings = new RevealSettings();
			public LinearSpawner mLinearSpawner = new LinearSpawner();

			private string mSection = "head";
			private JSONArray mRobots;
			private float mLastScreenWidth = 0;
			private float mLastScreenHeight = 0;
			
			/// <summary>
			/// The dictionary that contains the robots and their parts
			/// </summary>
			public void ChangeContent(string section){
				MBAEditor m = GameObject.FindObjectOfType<MBAEditor>();
				switch(section.ToLower()){
					case "head":
						this.mSection = section;
						m.ChangeTitle(section);
						break;
					case "left":
						this.mSection = section;
						m.ChangeTitle(section);
						
						break;
					case "right":
						this.mSection = section;
						m.ChangeTitle(section);
						
						break;
					case "car":
						this.mSection = section;
						m.ChangeTitle(section);
						
						break;
					default:
						this.mSection = "head";
						m.ChangeTitle(this.mSection);
						
						break;
				}
				
				if(!this.mRevealSettings.mOpening)
					this.SpawnButtons();
			}
			
			#region UNITYMETHODS

			// Use this for initialization
			void Start () {
				this.mRobots = JSONObject.Parse(GameUtilities.ReadFile ("Robots/robots")).GetArray("robots");
				this.mCount = this.mRobots.Length;
				this.mButtons = new List<GameObject>();
				this.mLastScreenWidth = Screen.width;
				this.mLastScreenHeight = Screen.height;	
				this.mButtonScaler.Initialize(this.mReferenceButtonSize, this.mReferenceScreenSize, (int)mScaleMode);
				this.mLinearSpawner.FitSpacingToScreenSize(this.mButtonScaler.mReferenceScreenSize);

				if(this.mRevealSettings.mRevealOnStart){
					this.SpawnButtons();
				}
			}

			// Update is called once per frame
			void Update () {
				if(Screen.width != this.mLastScreenWidth || Screen.height != this.mLastScreenHeight){
					this.mLastScreenWidth = Screen.width;
					this.mLastScreenHeight = Screen.height;					
					this.mLinearSpawner.FitSpacingToScreenSize(this.mButtonScaler.mReferenceScreenSize);
					this.SpawnButtons();
				}

				if(this.mRevealSettings.mOpening){
					if(!this.mRevealSettings.mSpawned){
						this.SpawnButtons();
					}

					switch(this.mRevealSettings.mRevealOption){
						case RevealSettings.REVEALOPTION.LINEAR:
							switch(this.mLinearSpawner.mRevealStyle){
								case LinearSpawner.REVEALSTYLE.SLIDETOPOSITION: this.RevealLinearLyNormal(); break;									
								case LinearSpawner.REVEALSTYLE.FADEINATPOSITION: this.RevealLinearlyFade();break;										
							}
							break;
					}
				}
			}

			#endregion
			
			void SpawnButtons(){
				this.mRevealSettings.mOpening = true;

				for(int i = 0; i < mButtons.Count; i ++)
					Destroy(this.mButtons[i]);

				this.mButtons.Clear();

//				this.ClearCommonButtonBranch();

				for(int i = 0; i < this.mCount; i++){
					GameObject b = Instantiate(this.mButtonRefs as GameObject);
					b.transform.SetParent(this.transform, false);
					if(this.mLinearSpawner.mRevealStyle == LinearSpawner.REVEALSTYLE.FADEINATPOSITION){
						Color c = b.GetComponent<Image>().color;
						c.a = 0;
						b.GetComponent<Image>().color = c;
						if(b.GetComponentInChildren<Text>()){
							c = b.GetComponentInChildren<Text>().color;
							c.a = 0;
							b.GetComponentInChildren<Text >().color = c;							
						}
					}

					this.mButtons.Add(b);
				}

				this.mRevealSettings.mSpawned = true;
			}
			
			#region REVEALMETHODS

			void RevealLinearLyNormal () {
				
				float rows = Mathf.Floor(mButtons.Count / 2);
				int columns = 2;
				List<Vector3> positions = new List<Vector3>();
				for(int row = 0; row <= rows; row++) {
					for(int column = 0; column < columns; column++) {
//						float r = column * rows + row;				
						Vector3 targetPos = new Vector3(-45f, 103f, 0);
						targetPos.x = (column * 90f) - this.mLinearSpawner.mButtonOffset;
						targetPos.y = targetPos.y - (90f * row);
						targetPos.z = 0;
						positions.Add(targetPos);

					}
				}		
				
				for(int i = 0; i < this.mButtons.Count; i++){
					RectTransform buttonRect = this.mButtons[i].GetComponent<RectTransform>();					
					string name = mRobots[i].Obj.GetString("robotname");
					buttonRect.GetComponentInChildren<Text>().text = name;
					buttonRect.GetComponentsInChildren<Image>()[1].sprite = RevealImageByName(name);
					buttonRect.gameObject.GetComponent<DynamicListener>().mMessageParameter = name;
					buttonRect.anchoredPosition = Vector3.Lerp(buttonRect.anchoredPosition, positions[i], this.mRevealSettings.mTranslateSmooth * Time.deltaTime);
					buttonRect.localScale = Vector3.one;
				}
			}

			void RevealLinearlyFade () {
				for(int i = 0; i < mButtons.Count; i++){
					Vector3 targetPos;
					RectTransform buttonRect = this.mButtons[i].GetComponent<RectTransform>();
					// set size
					buttonRect.sizeDelta = new Vector2(this.mButtonScaler.mNewButtonSize.x, this.mButtonScaler.mNewButtonSize.y);
					targetPos.x = this.mLinearSpawner.mDirection.x * ( (i + this.mLinearSpawner.mButtonOffset) * ( buttonRect.sizeDelta.x + this.mLinearSpawner.mButtonSpacing) ) + this.transform.position.x;
					targetPos.y = this.mLinearSpawner.mDirection.y * ( (i + this.mLinearSpawner.mButtonOffset) * ( buttonRect.sizeDelta.y + this.mLinearSpawner.mButtonSpacing) ) + this.transform.position.y;
					targetPos.z = 0;

					ButtonFader prevButtonFader;
					if(i > 0)
						prevButtonFader = this.mButtons[i - 1].GetComponent<ButtonFader>();
					else
						prevButtonFader = null;
					ButtonFader buttonFader = this.mButtons[i].GetComponent<ButtonFader>();

					if(prevButtonFader){
						if(prevButtonFader.mFaded) {
							this.mButtons[i].transform.position = targetPos;
							if(buttonFader)
								buttonFader.Fade(this.mRevealSettings.mFadeSmooth);
							else
								Debug.LogError("You want to fade your buttons, but they need a ButtonFader script attach to the gameobject");
						}		
					}else{
						this.mButtons[i].transform.position = targetPos;
						if(buttonFader)
							buttonFader.Fade(this.mRevealSettings.mFadeSmooth);
						else
							Debug.LogError("You want to fade your buttons, but they need a ButtonFader script attach to the gameobject");
					}
					
					buttonRect.localScale = Vector3.one;
				}
			}
			
			Sprite RevealImageByName (string robotName) {
				Texture2D t2d = null;
				Sprite holder = null;
				switch (this.mSection) {
					case "head":
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_head_image");	
						break;
					case "left":
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_larm_image");		
						break;
					case "right":
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_rarm_image");		
						break;
					case "car":
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_car_image");		
						break;
				}
				
				if(t2d)
					holder = Sprite.Create(t2d, new Rect(0,0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
				
				if(holder == null)
					holder = this.mDefaultHolder;
				
				return holder;
			}
			
			#endregion
			
			void ClearCommonButtonBranch () {
				GameObject[] buttonBranchers = GameObject.FindGameObjectsWithTag("ButtonBrancher");
				foreach(GameObject bb in buttonBranchers){
					// Check if the button brancher has the same branch of this one
					if(bb.transform.parent == this.transform.parent){

						ButtonBrancher mButtonBranch = bb.GetComponent<ButtonBrancher>();
						// Remove all buttons to keep things tidy
						for(int i = mButtonBranch.mCount -1; i >= 0; i--)
							Destroy(mButtonBranch.mButtons[i]);

						mButtonBranch.mButtons.Clear();
					}
				}
			}
		}
	}
}

