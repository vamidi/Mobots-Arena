using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectActive : BaseNode {

	private BaseInputNode mInputOne;
	private Rect mRectOne;
	private GameObject mControlledObject;
	
	public override void DrawWindow() {
		base.DrawWindow();
		Event e = Event.current;
		string inputOneTitle = "None";
		
		if(this.mInputOne){
			inputOneTitle = this.mInputOne.GetResult();
		}
		
		GUILayout.Label("Input one: " + inputOneTitle);
		
		if(e.type == EventType.Repaint){
			this.mRectOne = GUILayoutUtility.GetLastRect();
		}
		
		this.mControlledObject = (GameObject) EditorGUILayout.ObjectField(this.mControlledObject, typeof(GameObject), true);
	}
	
	public override void Tick(float deltatime) {
		if(this.mInputOne){
			if(this.mControlledObject){
				if(this.mInputOne.GetResult().Equals("true")){
					this.mControlledObject.SetActive(true);
				}else{
					this.mControlledObject.SetActive(false);
				}
			}
		}
	}
	
	public override void SetInput(BaseInputNode node, Vector2 clickposition) {
		clickposition.x -= this.mWindowRect.x;
		clickposition.y -= this.mWindowRect.y;
		
		if(this.mRectOne.Contains(clickposition)){
			this.mInputOne = node;
			
		}
	}
	
	public override void NodeDeleted(BaseNode node) {
		if(node.Equals(this.mInputOne)){
			this.mInputOne = null;
		}
	}
	
	public override BaseInputNode ClickedOnInput(Vector2 position) {
		BaseInputNode retVal = null;
		position.x -= this.mWindowRect.x;
		position.y -= this.mWindowRect.y;
		
		if(this.mRectOne.Contains(position)){
			retVal = this.mInputOne;
			this.mInputOne = null;
		}
		
		return retVal;
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
		
		
	}
	
	 
}
