using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Image mCurrentHealthBar;
	public Text mRatioText;
	
	private Part mPart = null;
	

	public void UpdateHealthBar(){
		if(mPart != null){
			float h = mPart.GetHealth();
			float m = mPart.GetMaxHealth();
			
			Debug.Log(h);
			Debug.Log(m);
			
			float ratio = mPart.GetHealth() / mPart.GetMaxHealth();
			this.mCurrentHealthBar.rectTransform.localScale = new Vector3(ratio , 1, 1);
			mRatioText.text = (ratio * 100 ).ToString() + "%";
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
