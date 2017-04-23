using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine.EventSystems;

public abstract class BaseNode : ScriptableObject {
	
	public Rect mWindowRect;
	public bool mHasInput = false;
	public string mWindowTitle = "";
	
	public virtual void DrawWindow() {
		this.mWindowTitle = EditorGUILayout.TextField("Title", mWindowTitle);
	}
		
	public virtual void SetInput(BaseInputNode node, Vector2 clickposition){
		
	}
	
	public virtual void NodeDeleted(BaseNode node) { }
	
	public virtual BaseInputNode ClickedOnInput(Vector2 position) {
		return null;
	}
	
 	public abstract void DrawCurves();
	
	public abstract void Tick(float deltatime);
}
