using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Boomlagoon.JSON;

public class RobotBtn : MonoBehaviour {

	private string mRobotName = "";
	private Button mButton = null;
	private Transform thisObject = null; 

	public void SetName(string name){
		this.mRobotName = name;
		if(thisObject != null && thisObject.FindChild("Text").GetComponent<Text>()){
			Text t = thisObject.FindChild("Text").GetComponent<Text>();
			t.text = this.mRobotName;
		}
	}

	void Awake () {
		thisObject = GetComponentInChildren<Transform>();
		mButton = GetComponent<Button>();
		if(mButton != null)
		 	mButton.GetComponent<Button>().onClick.AddListener(() => { OnClickListener(mRobotName); }); 
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnClickListener(string name){
		Editor editor = GameObject.FindObjectOfType<Editor>();

		if(editor != null)
			editor.ChangeRobotByName(name);
	}
}
