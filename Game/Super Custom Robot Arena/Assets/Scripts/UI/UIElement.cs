using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SCRA {

	namespace UI {
		
		public enum SCALEMODE { MATCHWIDTHHEIGHT, INDEPENDENTWIDTHHEIGHT }
		
		public class UIElement : MonoBehaviour {
			
			public class ImageScaler {
				SCALEMODE mScaleMode;
				Vector2 mReferenceImageSize;

				[HideInInspector]
				public Vector2 mReferenceScreenSize;
				public Vector2 mNewButtonSize;

				public void Initialize (Vector2 refImage, Vector2 screensize, int scalemode) {
					this.mScaleMode = (SCALEMODE)scalemode;
					this.mReferenceImageSize = refImage;
					this.mReferenceScreenSize = screensize;
					this.SetImageSize();
				}

				void SetImageSize () {
					if(this.mScaleMode == SCALEMODE.INDEPENDENTWIDTHHEIGHT){
						this.mNewButtonSize.x = (this.mReferenceImageSize.x * Screen.width) / this.mReferenceScreenSize.x;
						this.mNewButtonSize.y = (this.mReferenceImageSize.y * Screen.height) / this.mReferenceScreenSize.y; 						
					}else if(this.mScaleMode == SCALEMODE.MATCHWIDTHHEIGHT){
						this.mNewButtonSize.x = (this.mReferenceImageSize.x * Screen.width) / this.mReferenceScreenSize.x;
						this.mNewButtonSize.y = this.mNewButtonSize.x;
					}					
				}
			}
		
			
			public SCALEMODE mScaleMode;
			public Vector2 mReferenceImageSize;
			public Vector2 mReferenceScreenSize;
			public ImageScaler mImageScaler = new ImageScaler();
			
			protected RectTransform mCurrentRect;
			protected float mLastScreenWidth = 0f;
			protected float mLastScreenHeight = 0f;

			// Use this for initialization
			protected virtual void Start () {
				this.mCurrentRect = this.GetComponent<RectTransform>();
				Debug.Log(this.GetComponent<RectTransform>().sizeDelta);
				this.mReferenceImageSize = this.GetComponent<RectTransform>().sizeDelta;
				this.mLastScreenWidth = Screen.width;
				this.mLastScreenHeight = Screen.height;					
				this.mImageScaler.Initialize(this.mReferenceImageSize, this.mReferenceScreenSize, (int)this.mScaleMode);
				this.ResizeImage();
			}
			
			// Update is called once per frame
			protected virtual void Update () {
				if(this.mLastScreenWidth != Screen.width || this.mLastScreenHeight != Screen.height){
					this.ResizeImage();
				}
			}
			
			protected virtual void ResizeImage () {
				this.mImageScaler.Initialize(this.mReferenceImageSize, this.mReferenceScreenSize, (int)this.mScaleMode);
				this.mCurrentRect.sizeDelta = this.mImageScaler.mNewButtonSize;
				this.GetComponentsInChildren<Transform>()[1].GetComponent<RectTransform>().sizeDelta = this.mImageScaler.mNewButtonSize;
			}
		}
	}
}
