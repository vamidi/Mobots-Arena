using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SCRA {
	
	namespace UI {

		public enum SCALEMODE { MATCHWIDTHHEIGHT, INDEPENDENTWIDTHHEIGHT }
		
		public class ButtonBranch : MonoBehaviour {
		
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
			
			[System.Serializable]
			public class CircularSpawner {
				public enum REVEALSTYLE { SLIDETOPOSITION, FADEINATPOSITION };
				public REVEALSTYLE mRevealStyle;
				public Angle mAngle;
				public float mBaseDistFromBrancher = 20f;

				[HideInInspector]
				public float mDistFromBrancher = 5f;
				
				[System.Serializable]
				public struct Angle { public float mMinAngle, mMaxAngle; }

				public void FitDistanceToScreenSize (Vector2 screensize) {
					float refScreenFloat = (screensize.x + screensize.y) / 2;
					float screenfloat = (Screen.width + Screen.height) / 2;
					this.mDistFromBrancher = (this.mBaseDistFromBrancher * screenfloat ) / refScreenFloat;
				}
			}
			
			public GameObject[] mButtonRefs;
			[HideInInspector]
			public List<GameObject>mButtons;
			public SCALEMODE mScaleMode;
			public Vector2 mReferenceButtonSize;
			public Vector2 mReferenceScreenSize;
			public RevealSettings mRevealSettings = new RevealSettings();
			public LinearSpawner mLinearSpawner = new LinearSpawner();
			public CircularSpawner mCircularSpawner = new CircularSpawner();
			
			private ButtonScaler mButtonScaler = new ButtonScaler();
			private float mLastScreenWidth = 0;
			private float mLastScreenHeight = 0;
			
			#region UNITYMETHODS
			
			// Use this for initialization
			void Start () {
				this.mButtons = new List<GameObject>();
				this.mLastScreenWidth = Screen.width;
				this.mLastScreenHeight = Screen.height;					
				this.mButtonScaler.Initialize(this.mReferenceButtonSize, this.mReferenceScreenSize, (int)this.mScaleMode);
				this.mCircularSpawner.FitDistanceToScreenSize(this.mButtonScaler.mReferenceScreenSize);
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
					this.mButtonScaler.Initialize(this.mReferenceButtonSize, this.mReferenceScreenSize, (int)this.mScaleMode);
					this.mCircularSpawner.FitDistanceToScreenSize(this.mButtonScaler.mReferenceScreenSize);
					this.mLinearSpawner.FitSpacingToScreenSize(this.mButtonScaler.mReferenceScreenSize);
					this.SpawnButtons();
				}
				
				if(this.mRevealSettings.mOpening){
					if(!this.mRevealSettings.mSpawned)
						this.SpawnButtons();

					switch(this.mRevealSettings.mRevealOption){
						case RevealSettings.REVEALOPTION.LINEAR:
							switch(this.mLinearSpawner.mRevealStyle){
								case LinearSpawner.REVEALSTYLE.SLIDETOPOSITION: this.RevealLinearLyNormal(); break;									
								case LinearSpawner.REVEALSTYLE.FADEINATPOSITION: this.RevealLinearlyFade();break;										
							}
							break;
						case RevealSettings.REVEALOPTION.CIRCULAR:
							switch(this.mCircularSpawner.mRevealStyle){
								case CircularSpawner.REVEALSTYLE.SLIDETOPOSITION: this.RevealCircularNormal(); break;									
								case CircularSpawner.REVEALSTYLE.FADEINATPOSITION: this.RevealCircularFade();break;									
							}
							break;
					}
				}
			}
			
			#endregion
			
			Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle) {
				Vector3 targetPoint = point - pivot;
				targetPoint = Quaternion.Euler(0, 0, angle) * targetPoint;
				targetPoint += pivot;
				return targetPoint;
			}
			
			void SpawnButtons(){
				this.mRevealSettings.mOpening = true;
				
				for(int i = 0; i < mButtons.Count; i ++)
					Destroy(this.mButtons[i]);
				
				this.mButtons.Clear();
				
				this.ClearCommonButtonBranch();
				
				for(int i = 0; i < this.mButtonRefs.Length; i++){
					GameObject b = Instantiate(this.mButtonRefs[i] as GameObject);
					b.transform.SetParent(this.transform);
					b.transform.position = this.transform.position;
					if(this.mLinearSpawner.mRevealStyle == LinearSpawner.REVEALSTYLE.FADEINATPOSITION || this.mCircularSpawner.mRevealStyle == CircularSpawner.REVEALSTYLE.FADEINATPOSITION){
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
				for(int i = 0; i < mButtons.Count; i++){
					Vector3 targetPos;
					RectTransform buttonRect = this.mButtons[i].GetComponent<RectTransform>();
					// set size
					buttonRect.sizeDelta = new Vector2(this.mButtonScaler.mNewButtonSize.x, this.mButtonScaler.mNewButtonSize.y);
					targetPos.x = this.mLinearSpawner.mDirection.x * ( (i + this.mLinearSpawner.mButtonOffset) * ( buttonRect.sizeDelta.x + this.mLinearSpawner.mButtonSpacing) ) + this.transform.position.x;
					targetPos.y = this.mLinearSpawner.mDirection.y * ( (i + this.mLinearSpawner.mButtonOffset) * ( buttonRect.sizeDelta.y + this.mLinearSpawner.mButtonSpacing) ) + this.transform.position.y;
					targetPos.z = 0;
					buttonRect.position = Vector3.Lerp(buttonRect.position, targetPos, this.mRevealSettings.mTranslateSmooth * Time.deltaTime);				
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
				}
			}

			void RevealCircularNormal () {
				for (int i = 0; i < this.mButtons.Count; i++) {
					//find angle
					float angleDist = Mathf.Abs(this.mCircularSpawner.mAngle.mMaxAngle - this.mCircularSpawner.mAngle.mMinAngle);
					float targetAngle = this.mCircularSpawner.mAngle.mMinAngle + (angleDist / this.mButtons.Count) * i;
					//find pos
					Vector3 targetPos = transform.position + Vector3.right * this.mCircularSpawner.mDistFromBrancher;
					targetPos = RotatePointAroundPivot(targetPos, transform.position, targetAngle);
					RectTransform buttonRect = this.mButtons[i].GetComponent<RectTransform>();
					//resize button
					buttonRect.sizeDelta = new Vector2(this.mButtonScaler.mNewButtonSize.x, this.mButtonScaler.mNewButtonSize.y);

					buttonRect.position = Vector3.Lerp(buttonRect.position, targetPos, this.mRevealSettings.mTranslateSmooth * Time.deltaTime);
				}
			}

			void RevealCircularFade () {
				for (int i = 0; i < this.mButtons.Count; i++) {
					//find angle
					float angleDist = Mathf.Abs(this.mCircularSpawner.mAngle.mMaxAngle - this.mCircularSpawner.mAngle.mMinAngle);
					float targetAngle = this.mCircularSpawner.mAngle.mMinAngle + (angleDist / this.mButtons.Count) * i;
					//find pos
					Vector3 targetPos = transform.position + Vector3.right * this.mCircularSpawner.mDistFromBrancher;
					targetPos = RotatePointAroundPivot(targetPos, transform.position, targetAngle);
					RectTransform buttonRect = this.mButtons[i].GetComponent<RectTransform>();
					//resize button
					buttonRect.sizeDelta = new Vector2(this.mButtonScaler.mNewButtonSize.x, this.mButtonScaler.mNewButtonSize.y);

					ButtonFader previousButtonFader;
					if (i > 0)
						previousButtonFader = this.mButtons[i - 1].GetComponent<ButtonFader>();
					else
						previousButtonFader = null;
					ButtonFader buttonFader = this.mButtons[i].GetComponent<ButtonFader>();

					if (previousButtonFader) /* first button wont have a previous button */ {
						if (previousButtonFader.mFaded) {
							buttonRect.position = targetPos;
							if (buttonFader)
								buttonFader.Fade(this.mRevealSettings.mFadeSmooth);
							else
								Debug.LogError("You want to fade your buttons, but they need a ButtonFader script to be attached first.");
						}
					}else{
						buttonRect.position = targetPos;
						if (buttonFader)
							buttonFader.Fade(this.mRevealSettings.mFadeSmooth); //for the first button in the array
						else
							Debug.LogError("You want to fade your buttons, but they need a ButtonFader script to be attached first.");
					}
				}
			}
			
			#endregion
			
			void ClearCommonButtonBranch () {
				GameObject[] buttonBranch = GameObject.FindGameObjectsWithTag("ButtonBranch");
				foreach(GameObject bb in buttonBranch){
					// Check if the button brancher has the same branch of this one
					if(bb.transform.parent == this.transform.parent){
	
						ButtonBranch mButtonBranch = bb.GetComponent<ButtonBranch>();
						// Remove all buttons to keep things tidy
						for(int i = mButtonBranch.mButtons.Count -1; i >= 0; i--)
							Destroy(mButtonBranch.mButtons[i]);

						mButtonBranch.mButtons.Clear();
					}
				}
			}
		}
	}
}
