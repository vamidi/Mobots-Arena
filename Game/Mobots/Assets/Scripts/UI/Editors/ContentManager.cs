using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Mobots.UI;

namespace MBA {
	
	namespace UI {

		public class ContentManager : MonoBehaviour {
			
			#region Classes
			
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
				public Vector2 mButtonOffset = Vector2.zero;

				[HideInInspector]
				public float mButtonSpacing = 5f;

				public void FitSpacingToScreenSize (Vector2 screensize) {
					float refScreenFloat = (screensize.x + screensize.y) / 2;
					float screenfloat = (Screen.width + Screen.height) / 2;
					this.mButtonSpacing = (this.mBaseButtonSpacing * screenfloat ) / refScreenFloat;
				}	
			}
			
			#endregion
			
			public GameObject mButtonRef;
			public Sprite mDefaultHolder;
			public SCALEMODE mScaleMode;
			public Vector2 mReferenceButtonSize;
			public Vector2 mReferenceScreenSize;
			public Vector3 targetPos;
			public ButtonScaler mButtonScaler = new ButtonScaler();
			public RevealSettings mRevealSettings = new RevealSettings();
			public LinearSpawner mLinearSpawner = new LinearSpawner();

			private MBAEditor mEditor;
			private GameManager manager;
			private List<GameObject>mButtons;
			private int mCount;
			private float mLastScreenWidth = 0;
			private float mLastScreenHeight = 0;
			
			#region UNITYMETHODS
		
			// Use this for initialization
			void Start () {
				this.manager = GameObject.FindObjectOfType<GameManager>();
				this.mEditor = GameObject.FindObjectOfType<MBAEditor>();
				this.mCount = this.manager.robots.Length;
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
					
					this.RevealLinearLyNormal();
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
					GameObject b = Instantiate(this.mButtonRef as GameObject);
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
						Vector3 initPosition = this.targetPos;
						initPosition.x = (column * this.mLinearSpawner.mButtonOffset.x) - targetPos.x;
						initPosition.y = (row * this.mLinearSpawner.mButtonOffset.y) + targetPos.y;
						initPosition.z = 0;
						positions.Add(initPosition);

					}
				}		

				for(int i = 0; i < this.mButtons.Count; i++){
					RectTransform buttonRect = this.mButtons[i].GetComponent<RectTransform>();					
					string name = this.manager.robots[i].Obj.GetString("robotname");
					buttonRect.GetComponentInChildren<Text>().text = name;
					buttonRect.GetComponentsInChildren<Image>()[1].sprite = RevealImageByName(name);
					buttonRect.gameObject.GetComponent<DynamicListener>().mMessageParameter = name;
					buttonRect.anchoredPosition = Vector3.Lerp(buttonRect.anchoredPosition, positions[i], this.mRevealSettings.mTranslateSmooth * Time.deltaTime);
					buttonRect.localScale = Vector3.one;
				}
			}
			
			Sprite RevealImageByName (string robotName) {
				Texture2D t2d = null;
				Sprite holder = null;
				switch (this.mEditor.GetPart()) {
					case PART.HEAD:
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_head_image");	
						break;
					case PART.LARM:
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_larm_image");		
						break;
					case PART.RARM:
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_rarm_image");		
						break;
					case PART.CAR:
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
		}
	}
}
		