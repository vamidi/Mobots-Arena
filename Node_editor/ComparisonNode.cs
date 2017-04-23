using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

public class ComparisonNode : BaseInputNode {

	public enum COMPARISONTYPE { GREATER, LESS, EQUAL }
	private BaseInputNode mInputOne;
	private Rect mRectOne;

	private BaseInputNode mInputTwo;
	private Rect mRectTwo;
	private COMPARISONTYPE mComparisonType;
	
	private string mComparisonText = "";
	
	public ComparisonNode() {
		this.mWindowTitle = "Comparison Node";
		this.mHasInput = true;
	}
	
	public override void DrawWindow() {
		base.DrawWindow();
		Event e = Event.current;
		this.mComparisonType = (COMPARISONTYPE) EditorGUILayout.EnumPopup("Calculation Type: ", this.mComparisonType);
		string inputNodeOneTitle = "None";
		string inputNodeTwoTitle = "None";
		if(this.mInputOne){
			inputNodeOneTitle = this.mInputOne.GetResult();
		}

		GUILayout.Label("Input one: " + inputNodeOneTitle);

		if(e.type == EventType.Repaint){
			this.mRectOne = GUILayoutUtility.GetLastRect();
		}

		if(this.mInputTwo){
			inputNodeTwoTitle = this.mInputTwo.GetResult();
		}

		GUILayout.Label("Input two: " + inputNodeTwoTitle);

		if(e.type == EventType.Repaint){
			this.mRectTwo = GUILayoutUtility.GetLastRect();
		}
	}
	
	public override void SetInput(BaseInputNode node, Vector2 clickposition) {
		clickposition.x -= this.mWindowRect.x;
		clickposition.y -= this.mWindowRect.y;

		if(this.mRectOne.Contains(clickposition)){
			this.mInputOne = node;
		}else if(this.mRectTwo.Contains(clickposition)){
			this.mInputTwo = node;
		}
	}
	
	public override void DrawCurves() {
		if(this.mInputOne){
			Rect rect = this.mWindowRect;
			rect.x += this.mRectOne.x;
			rect.y += this.mRectOne.y + this.mRectOne.height / 2;
			rect.width = 1;
			rect.height = 1;

			NodeEditor.DrawNodeCurve(this.mInputOne.mWindowRect, rect);

		}

		if(this.mInputTwo){
			Rect rect = this.mWindowRect;
			rect.x += this.mRectTwo.x;
			rect.y += this.mRectTwo.y + this.mRectTwo.height / 2;
			rect.width = 1;
			rect.height = 1;

			NodeEditor.DrawNodeCurve(this.mInputTwo.mWindowRect, rect);

		}
	}
	
	public override void Tick(float deltatime) {
		float inputOneValue = 0;
		float inputTwoValue = 0;

		if(this.mInputOne){
			string inputOneRaw = this.mInputOne.GetResult();
			float.TryParse(inputOneRaw, out inputOneValue);
		}

		if(this.mInputTwo){
			string inputTwoRaw = this.mInputTwo.GetResult();
			float.TryParse(inputTwoRaw, out inputTwoValue);
		}

		string result = "";

		switch(this.mComparisonType){
			case COMPARISONTYPE.EQUAL:
				result = (inputOneValue == inputTwoValue) ? "true" : "false";
				break;
			case COMPARISONTYPE.GREATER:
				result = (inputOneValue > inputTwoValue) ? "true" : "false";
				break;
			case COMPARISONTYPE.LESS:
				result = (inputOneValue < inputTwoValue) ? "true" : "false";
				break;
		}

		this.mNodeResult = result;
	}
	
	public override BaseInputNode ClickedOnInput(Vector2 position) {
		BaseInputNode retVal = null;

		position.x -= this.mWindowRect.x;
		position.y -= this.mWindowRect.y;

		if(this.mRectOne.Contains(position)){
			retVal = this.mInputOne;
			this.mInputOne = null;
		}else if(this.mRectTwo.Contains(position)){
			retVal = this.mInputTwo;
			this.mInputTwo = null;
		}

		return retVal;
	}
	
	public override void NodeDeleted(BaseNode node) {
		if(node.Equals(this.mInputOne)){
			this.mInputOne = null;
		}

		if(node.Equals(this.mInputTwo)){
			this.mInputTwo = null;
		}
	}
}
