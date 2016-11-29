using UnityEngine;
using System.Collections;

public class PlayerManagement : MonoBehaviour {
	
	private bool mInGame = true;
	private bool mCursorOn = true;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(this.mInGame){
			if(Input.GetKeyDown(KeyCode.Escape)){
				mCursorOn = !mCursorOn;
			}
			Cursor.lockState = (mCursorOn) ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}
