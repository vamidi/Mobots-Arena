using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace SCRA {
	
	namespace UI {
	
		using Boomlagoon.JSON;

		public class RobotBtn : MonoBehaviour {
		
			private string mRobotName = "";
			private Button mButton = null;
			private Transform mCurrentObj = null;
			private UIEditor mEditor;
		
			public void SetName(string name){
				this.mRobotName = name;
				if(this.mCurrentObj != null && this.mCurrentObj.FindChild("Text").GetComponent<Text>()){
					Text t = this.mCurrentObj.FindChild("Text").GetComponent<Text>();
					t.text = this.mRobotName;
				}
			}
		
			void Awake () {
				this.mCurrentObj = this.GetComponentInChildren<Transform>();
				this.mEditor = GameObject.FindObjectOfType<UIEditor>();
				mButton = GetComponent<Button>();
				if(mButton != null)
				 	mButton.GetComponent<Button>().onClick.AddListener(() => { OnClickListener(mRobotName); }); 
			}
		
			private void OnClickListener(string name){		
				if(this.mEditor != null)
					this.mEditor.ChangeRobotByName(name);
			}
		}
	}
}
