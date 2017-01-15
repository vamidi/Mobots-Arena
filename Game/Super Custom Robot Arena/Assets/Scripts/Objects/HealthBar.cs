using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public float mColorLerpSpeed = 2f;
	public PART part = PART.HEAD;
	[SerializeField]
	public Image mCurrentHealthBar;

	private GameManager manager;
	private TagSettings mTags = new TagSettings();
	private Text mRatioText;
	
	[SerializeField]
	private Part mPart = null;
	private Color[] mColorArr = new Color[2];
	
	public void Initialize(){
		float ratio = Map( mPart.GetHealth(), 0, mPart.GetMaxHealth(), 0, 1);
		switch(this.part){
			case PART.HEAD:
				this.mCurrentHealthBar = GameObject.FindGameObjectWithTag(this.mTags.mHeadUI).GetComponentsInChildren<Image>()[1];
				this.mRatioText = GameObject.FindGameObjectWithTag(this.mTags.mHeadUI).GetComponentInChildren<Text>();
				break;
			case PART.RARM:
				this.mCurrentHealthBar = GameObject.FindGameObjectWithTag(this.mTags.mRightUI).GetComponentsInChildren<Image>()[1];
				this.mRatioText = GameObject.FindGameObjectWithTag(this.mTags.mRightUI).GetComponentInChildren<Text>();
				break;
			case PART.LARM:
				this.mCurrentHealthBar = GameObject.FindGameObjectWithTag(this.mTags.mLeftUI).GetComponentsInChildren<Image>()[1];
				this.mRatioText = GameObject.FindGameObjectWithTag(this.mTags.mLeftUI).GetComponentInChildren<Text>();
				break;
			case PART.CAR:
				this.mCurrentHealthBar = GameObject.FindGameObjectWithTag(this.mTags.mCarUI).GetComponentsInChildren<Image>()[1];
				this.mRatioText = GameObject.FindGameObjectWithTag(this.mTags.mCarUI).GetComponentInChildren<Text>();
				break;
		}
		
		if(this.mCurrentHealthBar)
			this.mCurrentHealthBar.color = Color.Lerp(this.mColorArr[0], this.mColorArr[1], ratio);	
	}

	public void UpdateHealthBar(){
		if(mPart != null){
			float ratio = Map( mPart.GetHealth(), 0, mPart.GetMaxHealth(), 0, 1);
			if(this.mCurrentHealthBar && this.mCurrentHealthBar.fillAmount != ratio){
				if(manager.mInGame)
					this.mCurrentHealthBar.fillAmount = Mathf.Lerp(this.mCurrentHealthBar.fillAmount, ratio, Time.deltaTime * this.mColorLerpSpeed);
				else
					this.mCurrentHealthBar.fillAmount = ratio;
				
				this.mCurrentHealthBar.color = Color.Lerp(this.mColorArr[0], this.mColorArr[1], ratio);
			}
			
			if(mRatioText)
				mRatioText.text = (ratio * 100 ).ToString("0") + "%";			
		}
	}
	
	// Use this for initialization
	void Start () {
		this.manager = GameObject.FindObjectOfType<GameManager>();
		this.mPart = GetComponent<Part>();
		if(this.mPart)
			this.part = this.mPart.GetPart();
		this.mColorArr[0] = new Color(1f, .007f, .007f);
		this.mColorArr[1] = new Color(.17f, .96f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		this.UpdateHealthBar();
	}
	
	private float Map(float value, float inMin, float inMax, float outMin, float outMax){
		return ( value - inMin ) * ( outMax - outMin) / ( inMax - inMin ) + outMin;
	}
}
