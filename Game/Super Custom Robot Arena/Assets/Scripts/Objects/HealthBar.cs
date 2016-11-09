﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Image mCurrentHealthBar;
	public Text mRatioText;
	public float mColorLerpSpeed = 2f;
	
	[SerializeField]
	private Part mPart = null;
	private Color[] mColorArr = new Color[2];	

	public void UpdateHealthBar(){
		if(mPart != null){
			float ratio = Map( mPart.GetHealth(), 0, mPart.GetMaxHealth(), 0, 1);
			if(this.mCurrentHealthBar && this.mCurrentHealthBar.fillAmount != ratio){
				this.mCurrentHealthBar.fillAmount = Mathf.Lerp(this.mCurrentHealthBar.fillAmount, ratio, Time.deltaTime * this.mColorLerpSpeed);
				this.mCurrentHealthBar.color = Color.Lerp(this.mColorArr[0], this.mColorArr[1], ratio);
			}
			
			if(mRatioText)
				mRatioText.text = (ratio * 100 ).ToString("0") + "%";			
		}
	}
	
	// Use this for initialization
	void Start () {
		mPart = GetComponent<Part>();
		this.mColorArr[0] = new Color(1f, .007f, .007f);
		this.mColorArr[1] = new Color(.17f, .96f, 0f);
		this.Init();
	}
	
	// Update is called once per frame
	void Update () {
		this.UpdateHealthBar();
	}
	
	private void Init(){
		float ratio = Map( mPart.GetHealth(), 0, mPart.GetMaxHealth(), 0, 1);
		this.mCurrentHealthBar.color = Color.Lerp(this.mColorArr[0], this.mColorArr[1], ratio);	
	}
	
	private float Map(float value, float inMin, float inMax, float outMin, float outMax){
		return ( value - inMin ) * ( outMax - outMin) / ( inMax - inMin ) + outMin;
	}
}