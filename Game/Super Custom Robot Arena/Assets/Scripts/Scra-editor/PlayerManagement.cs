using UnityEngine;
using System.Collections;

public class PlayerManagement : MonoBehaviour {

	public Texture2D mTexture2D;

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
//			Cursor.SetCursor (mTexture2D, new Vector2 (0.5f, 0.5f), CursorMode.Auto);
			Cursor.lockState = (mCursorOn) ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}
