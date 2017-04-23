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
				
				public bool mOpening;
				[HideInInspector]
				public Color mTextColor;
				[HideInInspector]
				public Color mTextBoxColor;
				public RectTransform mTextBoxRect;
				[HideInInspector]
				public Vector2 mCurrentSize;
				
				public virtual void Initialize() {
					if(!this.mTextBoxRect)
						this.mTextBoxRect = mTextBox.GetComponent<RectTransform>();
					this.mOpenedBox = this.mTextBoxRect.sizeDelta;
					this.mTextBoxRect.sizeDelta = this.mInitialBox;
					this.mCurrentSize = this.mTextBoxRect.sizeDelta;
					this.mOpening = false;
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
			
			protected bool mOpened = false;
			protected float mLifeTimer = 0f;
			
			/// <summary>
			/// Method when we click the button
			/// This will active new ui elements
			/// </summary>
			public void StartOpen(){
				this.mOpened = true;
				this.mUISettings.mTextBox.gameObject.SetActive(true);
				this.mUISettings.mText.gameObject.SetActive(true);
			}
			
			// Use this for initialization
			protected virtual void Start () {
				this.mAnimSettings.Initialize();
				this.mUISettings.Initialize();
			}

			// Update is called once per frame
			protected virtual void Update () {
				if(this.mUISettings.mOpening == false && this.mOpened == true)
					this.mUISettings.mOpening = true;
				
				if(this.mUISettings.mOpening){
					OpenToolTip();
					
					if(this.mAnimSettings.mWidthOpen && this.mAnimSettings.mHeightOpen){
						this.mLifeTimer += Time.deltaTime;
						if(this.mLifeTimer < this.mUISettings.mLiefSpan)
							this.FadeTextIn();
					}
				}
			}
			
			protected void OpenToolTip(){
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
			
			protected void OpenHeightToWidth() {
				if(!this.mAnimSettings.mHeightOpen){
					this.OpenHeight();
				}else{
					this.OpenWidth();
				}
			}
			
			protected void OpenWidthToHeight() {
				if(!this.mAnimSettings.mWidthOpen){
					this.OpenWidth();
				}else{
					this.OpenHeight();
				}				
			}
			
			protected void OpenWidthAndHeight() {
				if(!this.mAnimSettings.mWidthOpen){
					this.OpenWidth();
				}
				if(!this.mAnimSettings.mHeightOpen){
					this.OpenHeight();
				}
			}
			
			protected void OpenWidth() {
				this.mUISettings.mCurrentSize.x = Mathf.Lerp(this.mUISettings.mCurrentSize.x, this.mUISettings.mOpenedBox.x, this.mAnimSettings.mWidthSmooth * Time.deltaTime);
				if(Mathf.Abs(this.mUISettings.mCurrentSize.x - this.mUISettings.mOpenedBox.x) < this.mUISettings.mSnapToSizeDistance){
					this.mUISettings.mCurrentSize.x = this.mUISettings.mOpenedBox.x;
					this.mAnimSettings.mWidthOpen = true;
				}
			}
			
			protected void OpenHeight() {
				this.mUISettings.mCurrentSize.y = Mathf.Lerp(this.mUISettings.mCurrentSize.y, this.mUISettings.mOpenedBox.y, this.mAnimSettings.mHeightSmooth * Time.deltaTime);
				if(Mathf.Abs(this.mUISettings.mCurrentSize.y - this.mUISettings.mOpenedBox.y) < this.mUISettings.mSnapToSizeDistance){
					this.mUISettings.mCurrentSize.y = this.mUISettings.mOpenedBox.y;
					this.mAnimSettings.mHeightOpen = true;
				}
			}
			
			protected void FadeTextIn() {
				this.mUISettings.mTextColor.a = Mathf.Lerp(this.mUISettings.mTextColor.a, 1, this.mAnimSettings.mTextSmooth * Time.deltaTime);
				this.mUISettings.mText.color = this.mUISettings.mTextColor;
			}		
			
			protected void FadeToolTipOut(){
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

