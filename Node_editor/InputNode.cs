using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

public class InputNode : BaseInputNode {

	public enum INPUTTYPE { NUMBER, RANDOMNUMBER }
	
	private INPUTTYPE mInputType;
	
	private string mRandomFrom = "";
	private string mRandomTo = "";
	private string mInputValue = "";
	
	public InputNode(){
		this.mWindowTitle = "Input Node";
	}
	
	public override void DrawWindow() {
		base.DrawWindow();
		this.mInputType = (INPUTTYPE) EditorGUILayout.EnumPopup("Input type: ", this.mInputType);
		
		if(this.mInputType == INPUTTYPE.NUMBER){
			this.mInputValue = EditorGUILayout.TextField("Value: ", mInputValue);
		}else if(this.mInputType == INPUTTYPE.RANDOMNUMBER){
			this.mRandomFrom = EditorGUILayout.TextField("Random from: ", this.mRandomFrom);
			this.mRandomFrom = EditorGUILayout.TextField("Random to: ", this.mRandomTo);
			if(GUILayout.Button("Calculate Random")){
				CalculateRandom();
			}			
		}
		
		this.mNodeResult = mInputValue.ToString();
	}
	
	public override void Tick(float deltatime) { }
	
	public override void DrawCurves() { }
	
	public override string GetResult() {
		return this.mInputValue.ToString();
	}
	
	private void CalculateRandom() {
		float rFrom = 0;
		float rTo = 0;
		
		float.TryParse(this.mRandomFrom, out rFrom);
		float.TryParse(this.mRandomTo, out rTo);
		
		int randFrom = (int)(rFrom * 10);
		int randTo = (int)(rTo * 10);
		
		int selected = UnityEngine.Random.Range(randFrom, randTo + 1);
		
		float selectedValue = selected / 10;
		
		this.mInputValue = selectedValue.ToString();
	}
	
}
