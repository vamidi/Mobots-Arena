using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class Transition : MonoBehaviour {

	public enum OUTTRANSITIONTYPE { FADE, FADEINSTANT, FLICKER };
	public OUTTRANSITIONTYPE mOutTransitionType;
	public enum INTRANSTIONTYPE { FADE, FADEINSTANT, FLICKER };
	public INTRANSTIONTYPE mInTranstionType;
	public string mParentTag;
	public Vector3 mSpawnPosition = Vector3.zero;
	public float mFadeSpeed = 2f;
	public float mFlickerRate = 0.025f;
	
//	private bool mTransitionInitialized = false;
	private bool mStartTransition = false;
	private float mInColorAlpha = 0f;
	private float mOutColorAlpha = 0f;
	private Text[] mTransitionTxts, mTxts;
	private Image[] mTransitionImages, mImages;
	private RectTransform mTransitionPage;
//	private RectTransform mThisPage;
	
	public GameObject InitializeTransitionPage (GameObject transition) {
		GameObject go = Instantiate(transition as GameObject);
		this.mTransitionPage = go.GetComponent<RectTransform>();
		this.mTransitionPage.SetParent(GameObject.FindGameObjectWithTag(this.mParentTag).transform);
		this.mTransitionPage.localScale = Vector3.one;
		this.mTransitionImages = this.mTransitionPage.GetComponentsInChildren<Image>();
		this.mTransitionTxts = this.mTransitionPage.GetComponentsInChildren<Text>();
		
		foreach(Text t in this.mTransitionTxts )
			t.color = new Vector4(t.color.r, t.color.g, t.color.b, 0);
		foreach(Image i in this.mTransitionImages )
			i.color = new Vector4(i.color.r, i.color.g, i.color.b, 0);		
		 
//		this.mTransitionInitialized = true;
		
		return this.mTransitionPage.gameObject;
	}
	
	public void StartTransition () {
		this.mStartTransition = true;	
	}
	
	// Use this for initialization
	void Start () {
		if(this.mOutTransitionType == OUTTRANSITIONTYPE.FADE)
			this.mOutColorAlpha = 1f;
		
//		this.mThisPage = this.GetComponent<RectTransform>();
		//
		this.mImages = this.GetComponentsInChildren<Image>();
		this.mTxts = this.GetComponentsInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(this.mStartTransition){
			switch(this.mOutTransitionType){
				case OUTTRANSITIONTYPE.FADE: this.FadePageOut(); break;
				case OUTTRANSITIONTYPE.FADEINSTANT: this.mOutColorAlpha = 0;break;
				case OUTTRANSITIONTYPE.FLICKER: StartCoroutine(this.FlickerOut(this.mFlickerRate));break;
			}
			switch(this.mInTranstionType){
				case INTRANSTIONTYPE.FADE: this.FadePageIn(); break;
				case INTRANSTIONTYPE.FADEINSTANT: this.mOutColorAlpha = 1f; break;
				case INTRANSTIONTYPE.FLICKER: StartCoroutine(this.FlickerIn(this.mFlickerRate)); break;
			}
			
			this.UpdateTransitonPageColors();
			this.UpdateCurrentPageColors();
		}
	}
	
	private void FadePageOut (){
		this.mOutColorAlpha = Mathf.Lerp(this.mOutColorAlpha, 0, this.mFadeSpeed * Time.deltaTime);
	}
	
	private IEnumerator FlickerOut (float frequency) {
		yield return null;
	}
	
	private void FadePageIn () {
		this.mInColorAlpha = Mathf.Lerp(this.mInColorAlpha, 1, this.mFadeSpeed * Time.deltaTime);
		
		if(this.mInColorAlpha > 0.99f)
			this.mInColorAlpha = 1f;
		
		if(this.mInColorAlpha == 1f)
			Destroy(this.gameObject);
	}
	
	private IEnumerator FlickerIn (float frequency) {
		for(int i = 0; i < 8; i++){
			yield return new WaitForSeconds(frequency);
			this.mInColorAlpha = 0.35f;
			yield return new WaitForSeconds(frequency);
			this.mInColorAlpha = 1f;
		}
		
		if(this.mInColorAlpha == 1f)
			Destroy(this.gameObject);
	}
	
	private void UpdateTransitonPageColors () {
		if(this.mTransitionImages != null){
			foreach(Image i in this.mTransitionImages)
				i.color = new Vector4(i.color.r, i.color.g, i.color.b, this.mInColorAlpha);
		}
		
		if(this.mTransitionTxts != null){
			foreach(Text t in this.mTransitionTxts)
				t.color = new Vector4(t.color.r, t.color.g, t.color.b, this.mInColorAlpha);
		}	
	}
	
	private void UpdateCurrentPageColors () {
		if(this.mImages != null){
			foreach(Image i in this.mImages){
				i.color = new Vector4(i.color.r, i.color.g, i.color.b, this.mOutColorAlpha);
			}
		}
		
		if(this.mTxts != null){
			foreach(Text t in this.mTxts){
				t.color = new Vector4(t.color.r, t.color.g, t.color.b, this.mOutColorAlpha);
			}
		}
	}
}
