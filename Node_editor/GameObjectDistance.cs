using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectDistance : BaseInputNode {
	
	private GameObject mObjectOne;
	private GameObject mObjectTwo;

	public GameObjectDistance(){
		this.mWindowTitle = "GameObject Distance";
		this.mHasInput = false;
	}
	
	public override void DrawWindow() {
		base.DrawWindow();
		this.mObjectOne = (GameObject) EditorGUILayout.ObjectField(this.mObjectOne, typeof(GameObject), true);
		this.mObjectTwo = (GameObject) EditorGUILayout.ObjectField(this.mObjectTwo, typeof(GameObject), true);
	}
	
	public override void DrawCurves() { }
	
	public override void Tick(float deltatime) {
		float retVal = 0;
		if(this.mObjectOne && this.mObjectTwo){
			retVal = Vector3.Distance(this.mObjectOne.transform.position, this.mObjectTwo.transform.position);
		}
		
		this.mNodeResult = retVal.ToString();
	}
}
