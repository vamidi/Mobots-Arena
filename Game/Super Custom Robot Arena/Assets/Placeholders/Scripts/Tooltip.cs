using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MBA {
	
	namespace UI {
		
		public enum OPENSTYLE { WIDTHTOHEIGHT, HEIGHTTOWIDTH, HEIGHTANDWIDTH }
		public class Tooltip : MonoBehaviour {
			
			[System.Serializable]
			public class AnimationSettings {
				public OPENSTYLE mOpenStyle;
				public float mWidthSmooth = 4.6f, mHeightSmooth = 4.6f;
				public float mTextSmooth = .1f;
				
				[HideInInspector]
				public bool mWidthOpen = false, mHeightOpen = false;
				
				public void Initialize(){
					this.mWidthOpen = false;
					this.mHeightOpen = false;
				}
			}
			
			[System.Serializable]
			public class UISettings {
				public Image mTextBox;
				public Text mText;
				public Vector2 mInitialBox = new Vector2(.25f, .25f);
				public Vector2 mOpenedBox = new Vector2(400f, 200f);
				public float mSnapToSizeDistance = .25f;
				public float mLiefSpan = 5f;
				
				
				public bool mOpening = true;
				[HideInInspector]
				public Color mTextColor;
				[HideInInspector]
				public Color mTextBoxColor;
				[HideInInspector]
				public RectTransform mTextBoxRect;
				[HideInInspector]
				public Vector2 mCurrentSize;
				
				public void Initialize() {
					this.mTextBoxRect = mTextBox.GetComponent<RectTransform>();
					this.mTextBoxRect.sizeDelta = this.mInitialBox;
					this.mCurrentSize = this.mTextBoxRect.sizeDelta;
					this.mOpening = true;
					this.mTextColor = this.mText.color;
					this.mTextColor.a = 0f;
					this.mText.color = this.mTextColor;
					this.mTextBoxColor = this.mTextBox.color;
					this.mTextBoxColor.a = 1f;
					this.mTextBox.color = this.mTextBoxColor;  
					
//					this.mTextBox.gameObject.SetActive(false);
//					this.mText.gameObject.SetActive(false);
				}
			}

			public AnimationSettings mAnimSettings = new AnimationSettings();
			public UISettings mUISettings = new UISettings();
			
			private float mLifeTimer = 0f;
			
			/// <summary>
			/// Method when we click the button
			/// This will active new ui elements
			/// </summary>
			public void StartOpen(){
				this.mUISettings.mOpening = true;
				this.mUISettings.mTextBox.gameObject.SetActive(true);
				this.mUISettings.mText.gameObject.SetActive(true);
			}
			
			// Use this for initialization
			void Awake () {
				this.mAnimSettings.Initialize();
				this.mUISettings.Initialize();
			}

			// Update is called once per frame
			void Update () {
				if(this.mUISettings.mOpening){
					OpenToolTip();
					
					if(this.mAnimSettings.mWidthOpen && this.mAnimSettings.mHeightOpen){
						this.mLifeTimer += Time.deltaTime;
						if(this.mLifeTimer > this.mUISettings.mLiefSpan){
							this.FadeToolTipOut();
						}else{
							this.FadeTextIn();
						}
						
					}
				}
			}
			
			void OpenToolTip(){
				switch(this.mAnimSettings.mOpenStyle){
					case OPENSTYLE.HEIGHTTOWIDTH:
						this.OpenHeightToWidth();
						break;
					case OPENSTYLE.WIDTHTOHEIGHT:
						this.OpenWidthToHeight();
						break;
					case OPENSTYLE.HEIGHTANDWIDTH:
						this.OpenWidthAndHeight();
						break;
					default:
						Debug.LogError("No Animation is selected for this style");
						break;
				}
				
				this.mUISettings.mTextBoxRect.sizeDelta = this.mUISettings.mCurrentSize;
			}
			
			void OpenHeightToWidth() {
				if(!this.mAnimSettings.mHeightOpen){
					this.OpenHeight();
				}else{
					this.OpenWidth();
				}
			}
			
			void OpenWidthToHeight() {
				if(!this.mAnimSettings.mWidthOpen){
					this.OpenWidth();
				}else{
					this.OpenHeight();
				}				
			}
			
			void OpenWidthAndHeight() {
				if(!this.mAnimSettings.mWidthOpen){
					this.OpenWidth();
				}
				if(!this.mAnimSettings.mHeightOpen){
					this.OpenHeight();
				}
			}
			
			void OpenWidth() {
				this.mUISettings.mCurrentSize.x = Mathf.Lerp(this.mUISettings.mCurrentSize.x, this.mUISettings.mOpenedBox.x, this.mAnimSettings.mWidthSmooth * Time.deltaTime);
				if(Mathf.Abs(this.mUISettings.mCurrentSize.x - this.mUISettings.mOpenedBox.x) < this.mUISettings.mSnapToSizeDistance){
					this.mUISettings.mCurrentSize.x = this.mUISettings.mOpenedBox.x;
					this.mAnimSettings.mWidthOpen = true;
				}
			}
			
			void OpenHeight() {
				this.mUISettings.mCurrentSize.y = Mathf.Lerp(this.mUISettings.mCurrentSize.y, this.mUISettings.mOpenedBox.y, this.mAnimSettings.mHeightSmooth * Time.deltaTime);
				if(Mathf.Abs(this.mUISettings.mCurrentSize.y - this.mUISettings.mOpenedBox.y) < this.mUISettings.mSnapToSizeDistance){
					this.mUISettings.mCurrentSize.y = this.mUISettings.mOpenedBox.y;
					this.mAnimSettings.mHeightOpen = true;
				}
			}
			
			void FadeTextIn() {
				this.mUISettings.mTextColor.a = Mathf.Lerp(this.mUISettings.mTextColor.a, 1, this.mAnimSettings.mTextSmooth * Time.deltaTime);
				this.mUISettings.mText.color = this.mUISettings.mTextColor;
			}
			
			void FadeToolTipOut(){
				this.mUISettings.mTextColor.a = Mathf.Lerp(this.mUISettings.mTextColor.a, 0, this.mAnimSettings.mTextSmooth * Time.deltaTime);
				this.mUISettings.mText.color = this.mUISettings.mTextColor;
				this.mUISettings.mTextBoxColor.a = Mathf.Lerp(this.mUISettings.mTextBoxColor.a, 0, this.mAnimSettings.mTextSmooth * Time.deltaTime);
				this.mUISettings.mTextBox.color = this.mUISettings.mTextColor;
				
				if(this.mUISettings.mTextBoxColor.a < 0.01f){
					this.mUISettings.mOpening = false;
					this.mAnimSettings.Initialize();
					this.mUISettings.Initialize();
					this.mLifeTimer = 0;
				}
			}
		}		
	}
}

