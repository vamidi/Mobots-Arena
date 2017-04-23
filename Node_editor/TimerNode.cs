using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimerNode : BaseInputNode {
	private float mEnabledSeconds;
	private float mDisabledSeconds;
	private float mStatusTimer = 0;
	private bool mEnableToWait = true;
	private bool mCurrentResult = false;
	
	public TimerNode(){
		this.mWindowTitle = "Timer Node";
	}
	
	public override void DrawWindow() {
		base.DrawWindow();
		float.TryParse(EditorGUILayout.TextField("Seconds to enable: ", this.mEnabledSeconds.ToString()) ,out this.mEnabledSeconds);	
		float.TryParse(EditorGUILayout.TextField("Seconds to disable: ", this.mDisabledSeconds.ToString()) ,out this.mDisabledSeconds);	
		
		string status = "Seconds to enable: " + (this.mEnabledSeconds - this.mStatusTimer);
		
		if(!this.mEnableToWait){
			status = "Seconds to disable: " + (this.mDisabledSeconds - this.mStatusTimer);
		}
		
		EditorGUILayout.LabelField(status);
	}
	
	public override void DrawCurves() { }

	public override void Tick(float deltatime) {
		if(this.mEnableToWait){
			if(this.mStatusTimer < this.mEnabledSeconds){
				this.mStatusTimer += deltatime;
			}else{
				this.mStatusTimer = 0;
				this.mEnableToWait = false;
				this.mCurrentResult = true;
			}
		}else{
			if(this.mStatusTimer < this.mDisabledSeconds){
				this.mStatusTimer += deltatime;
			}else{
				this.mStatusTimer = 0;
				this.mEnableToWait = true;
				this.mCurrentResult = false;
			}
		}
		
		this.mNodeResult = this.mCurrentResult.ToString().ToLower();
	}
	
}
