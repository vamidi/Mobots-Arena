using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MBA.UI;

[System.Serializable]
public class DialogInterFace : MonoBehaviour {
	
	public Tooltip mToolTip;
	public Image mPanel; 
	public Text mText;
	public Button mPositiveButton, mNegativeButton, mAlternativeButton;
	public const int BUTTON_POSITIVE = 0;
	public const int BUTTON_NEGATIVE = 1;
	public const int BUTTON_NEUTRAL = 2;
	
	public void Dismiss(){
		Destroy(this.gameObject);
	}
	
	public void Awake(){
		this.mToolTip = GetComponent<Tooltip>();
		this.mPanel = GetComponent<Image>();
		Color c = this.mPanel.color;
		c.a = .8f;
		this.mPanel.color = c;

		c = this.mText.color;
		c.a = 0;
		this.mText.color = c;

		c = this.mPositiveButton.GetComponent<Image>().color;
		c.a = 0;
		this.mPositiveButton.GetComponent<Image>().color = c;

		c = this.mNegativeButton.GetComponent<Image>().color;
		c.a = 0;
		this.mNegativeButton.GetComponent<Image>().color = c;

		c = this.mPositiveButton.GetComponentInChildren<Text>().color;
		c.a = 0;
		this.mPositiveButton.GetComponentInChildren<Text>().color = c;

		c = mNegativeButton.GetComponentInChildren<Text>().color;
		c.a = 0;
		this.mNegativeButton.GetComponentInChildren<Text>().color = c;	
		
		if(this.mAlternativeButton){
			c = this.mAlternativeButton.GetComponent<Image>().color;
			c.a = 0;
			this.mAlternativeButton.GetComponent<Image>().color = c;
			c = this.mAlternativeButton.GetComponentInChildren<Text>().color;
			c.a = 0;
			this.mAlternativeButton.GetComponentInChildren<Text>().color = c;	
		}
	}
	
	
	public void Show(){
		Color c = this.mPanel.color;
		c.a = 0.8f;
		this.mPanel.color = c;

		c = this.mText.color;
		c.a = 1f;
		this.mText.color = c;

		c = this.mPositiveButton.GetComponent<Image>().color;
		c.a = 1f;
		this.mPositiveButton.GetComponent<Image>().color = c;

		c = this.mNegativeButton.GetComponent<Image>().color;
		c.a = 1f;
		this.mNegativeButton.GetComponent<Image>().color = c;

		c = this.mPositiveButton.GetComponentInChildren<Text>().color;
		c.a = 1f;
		this.mPositiveButton.GetComponentInChildren<Text>().color = c;

		c = mNegativeButton.GetComponentInChildren<Text>().color;
		c.a = 1f;
		this.mNegativeButton.GetComponentInChildren<Text>().color = c;
		
		c = mNegativeButton.GetComponentInChildren<Text>().color;
		c.a = 1f;
		this.mNegativeButton.GetComponentInChildren<Text>().color = c;	
		if(this.mAlternativeButton){
			c = this.mAlternativeButton.GetComponent<Image>().color;
			c.a = 1f;
			this.mAlternativeButton.GetComponent<Image>().color = c;
			c = mAlternativeButton.GetComponentInChildren<Text>().color;
			c.a = 1f;
			this.mAlternativeButton.GetComponentInChildren<Text>().color = c;	
		}
		
		this.mToolTip.StartOpen();
	}
	
	public interface OnClickListener {
		void OnClick(DialogInterFace dialog, int which);
	}
		
	[System.Serializable]
	public class Builder {
		[HideInInspector]
		public DialogInterFace mInterface = null;
		public string mMessage = "";
		public string mPositiveText = "";
		public string mNegativeText = "";
		public string mAlternativeText = "";
		[HideInInspector]
		public OnClickListener mOnClickListener = null;
	
		public DialogInterFace Create(){
			this.mInterface.mText.text = this.mMessage;
			this.mInterface.mPositiveButton.GetComponentInChildren<Text>().text = this.mPositiveText;
			this.mInterface.mNegativeButton.GetComponentInChildren<Text>().text = this.mNegativeText;
			if(this.mInterface.mAlternativeButton != null)
				this.mInterface.mAlternativeButton.GetComponentInChildren<Text>().text = this.mAlternativeText;
			return this.mInterface;
		}
			
		public Builder(DialogInterFace i){
			this.mInterface = i;
		}
		
		public void SetMessage(string msg){
			this.mMessage = msg;
		}
		
		public void SetPositiveButton(string msg, OnClickListener DialogClickListener){
			this.mPositiveText = msg;
			this.mInterface.mPositiveButton.onClick.AddListener(() => DialogClickListener.OnClick(this.mInterface, DialogInterFace.BUTTON_POSITIVE));
			
		}
		
		public void SetNegativeButton(string msg, OnClickListener DialogClickListener){
			this.mNegativeText = msg;
			this.mInterface.mNegativeButton.onClick.AddListener(() => DialogClickListener.OnClick(this.mInterface, DialogInterFace.BUTTON_NEGATIVE));
		}
		
		public void SetAlternativeButton(string msg, OnClickListener DialogClickListener){
			this.mAlternativeText = msg;
			this.mInterface.mAlternativeButton.onClick.AddListener(() => DialogClickListener.OnClick(this.mInterface, DialogInterFace.BUTTON_NEUTRAL));
		}
	}
}

