using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SCRA {

	namespace UI {
	
		public class ButtonFader : MonoBehaviour {
			
			public bool mFaded = false; // determine if it can fade
			
			private Image mImage;
			private Text mText;
			private Color mButtonColor;
			private Color mTextColor;
			private bool mStartFade = false;
			private float mSmooth = 0f;
			private bool mInitialized = false;
			
			public void Fade (float rate){
				
				// Make sure we are initialized
				if(!this.mInitialized)
					this.Initialize();
				
				// set the fade speed
				this.mSmooth = rate;
				this.mStartFade = true;
				
				// increase alpha component of the colors
				this.mButtonColor.a += rate;
				this.mImage = this.mButtonColor;
				// text color
				if(this.mText){
					this.mTextColor.a += rate;
					this.mText.color = this.mTextColor;
				}
				
			}
			
			// Use this for initialization
			void Start () {
				this.Initialize();
			}
			
			// Update is called once per frame
			void Update () {
				if(this.mStartFade){
					this.Fade(this.mSmooth);
					if(this.mButtonColor.a > 0.9f)
						this.mFaded = true; 
				}
			}
			
			void Initialize () {
				this.mStartFade = false;
				this.mFaded = false;
				this.mImage = this.GetComponent<Image>();
				this.mButtonColor = this.mImage.color;
				if(this.GetComponentInChildren<Text>()){
					this.mText = this.GetComponentInChildren<Text>();
					this.mTextColor = this.mText.color;
				}
				mInitialized = true;
			}
		}
	}
}

