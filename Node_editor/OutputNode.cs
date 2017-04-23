using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputNode : BaseNode {

	private string mResult = "";
	
	private BaseInputNode mInputNode;
	private Rect mInputRect;
	
	public OutputNode() {
		this.mWindowTitle = "Output Node";
		this.mHasInput = true;
	}
	
	public override void DrawWindow() {
		base.DrawWindow();
		Event e = Event.current;
		string inputOneTitle = "None";
		
		if(mInputNode){
			inputOneTitle = mInputNode.GetResult();
		}
		
		GUILayout.Label("Input one: " + inputOneTitle);
		if(e.type == EventType.Repaint){
			this.mInputRect = GUILayoutUtility.GetLastRect();
		}
		
		GUILayout.Label("Result: " + this.mResult);
	}
	
	public override void DrawCurves() {
		if(this.mInputNode){
			Rect rect = this.mWindowRect;
			rect.x += this.mInputRect.x;
			rect.y += this.mInputRect.y + this.mInputRect.height / 2;
			rect.width = 1;
			rect.height = 1;
			
		    NodeEditor.DrawNodeCurve(this.mInputNode.mWindowRect, rect);
		}
	}
	
	public override void NodeDeleted(BaseNode node) {
		if(node.Equals(this.mInputNode)){
			this.mInputNode = null;
		}
	}
	
	public override BaseInputNode ClickedOnInput(Vector2 position)
	{
		BaseInputNode retVal = null;
		
		position.x -= this.mWindowRect.x;
		position.y -= this.mWindowRect.y;
		
		if(this.mInputRect.Contains(position)){
			retVal = this.mInputNode;
			this.mInputNode = null;
	 	}
		
		return retVal;
	}
	
	public override void SetInput(BaseInputNode node, Vector2 clickposition) {
		clickposition.x -= this.mWindowRect.x;
		clickposition.y -= this.mWindowRect.y;
		
		if(this.mInputRect.Contains(clickposition)){
			this.mInputNode = node;
		}
		
	}
	
	public override void Tick(float deltatime)  {}
}
