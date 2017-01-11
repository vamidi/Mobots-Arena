﻿using UnityEngine;
using System.Collections;
using MBA.UI;
using UnityEngine.UI;

public class DialogAnimation : Tooltip {

	[System.Serializable]
	public class DialogUISettings : Tooltip.UISettings {
		public Button PositiveButton, NegativeButton;
		
		[HideInInspector]
		public Color mNegativeButtonColor, mPositiveButtonColor;
		[HideInInspector]
		public Color mPositiveTextColor, mNegativeTextColor;

		public override void Initialize() { 
			this.mNegativeButtonColor = this.NegativeButton.GetComponent<Image>().color;
			this.mNegativeButtonColor.a = 0;
			this.NegativeButton.GetComponent<Image>().color = this.mNegativeButtonColor;
			this.mPositiveButtonColor = this.NegativeButton.GetComponent<Image>().color;
			this.mPositiveButtonColor.a = 0;
			this.PositiveButton.GetComponent<Image>().color = this.mPositiveButtonColor;
			
			this.mPositiveTextColor = this.PositiveButton.GetComponentInChildren<Text>().color;
			this.mPositiveTextColor.a = 0f;
			this.PositiveButton.GetComponentInChildren<Text>().color = this.mPositiveTextColor;
			this.mNegativeTextColor = this.NegativeButton.GetComponentInChildren<Image>().color;
			this.mNegativeTextColor.a = 0f;
			this.NegativeButton.GetComponentInChildren<Text>().color = this.mNegativeTextColor;
		}
	}

	public DialogUISettings mDialogUISettings = new DialogUISettings();
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mDialogUISettings.Initialize();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(this.mUISettings.mOpening == false && this.mOpened == true)
			this.mUISettings.mOpening = true;

		if(this.mUISettings.mOpening){
			OpenToolTip();

			if(this.mAnimSettings.mWidthOpen && this.mAnimSettings.mHeightOpen){
				this.mLifeTimer += Time.deltaTime;
				if(this.mLifeTimer < this.mUISettings.mLiefSpan){
					this.FadeTextIn();
					this.FadeButtonsIn();
				}
			}
		}
	}
	
	void FadeButtonsIn() {
		this.mDialogUISettings.mPositiveButtonColor.a = Mathf.Lerp(this.mDialogUISettings.mPositiveButtonColor.a, 1, this.mAnimSettings.mTextSmooth * Time.deltaTime);
		this.mDialogUISettings.mNegativeButtonColor.a = Mathf.Lerp(this.mDialogUISettings.mNegativeButtonColor.a, 1, this.mAnimSettings.mTextSmooth * Time.deltaTime);
		
		this.mDialogUISettings.mPositiveTextColor.a = Mathf.Lerp(this.mDialogUISettings.mPositiveTextColor.a, 1, this.mAnimSettings.mTextSmooth * Time.deltaTime);
		this.mDialogUISettings.mNegativeTextColor.a = Mathf.Lerp(this.mDialogUISettings.mNegativeTextColor.a, 1, this.mAnimSettings.mTextSmooth * Time.deltaTime);

		this.mDialogUISettings.PositiveButton.GetComponent<Image>().color = this.mDialogUISettings.mPositiveTextColor;
		this.mDialogUISettings.NegativeButton.GetComponent<Image>().color = this.mDialogUISettings.mNegativeTextColor;
		
		this.mDialogUISettings.PositiveButton.GetComponentInChildren<Text>().color = this.mDialogUISettings.mPositiveTextColor;
		this.mDialogUISettings.NegativeButton.GetComponentInChildren<Text>().color = this.mDialogUISettings.mNegativeTextColor;
	}
}