using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Image mCurrentHealthBar;
	public Text mRatioText;
	
	[SerializeField]
	private Part mPart = null;
	

	public void UpdateHealthBar(){
		if(mPart != null){
			float ratio = mPart.GetHealth() / mPart.GetMaxHealth();
			string test = mPart.GetPart().ToString();
			
			this.mCurrentHealthBar.rectTransform.localScale = new Vector3(ratio , 1, 1);
			mRatioText.text = test + ": " + (ratio * 100 ).ToString("0") + "%";			
		}
	}
	
	// Use this for initialization
	void Start () {
		mPart = GetComponent<Part>();
		UpdateHealthBar();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
