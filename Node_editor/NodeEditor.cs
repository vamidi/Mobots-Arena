using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NodeEditor : EditorWindow {
	
	private List<BaseNode>windows = new List<BaseNode>();
	private Vector2 mMousePosition;
	private BaseNode mSelectedNode;
	private bool mInTransitionNode = false;
	private System.Diagnostics.Stopwatch mStopWatch = new System.Diagnostics.Stopwatch();

	public static void DrawNodeCurve(Rect start, Rect end){
		Vector3 startposition = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
		Vector3 endposition = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
		Vector3 startTan = startposition + Vector3.right * 50;
		Vector3 endTan = endposition + Vector3.left * 50;
		Color shadowCol = new Color(0, 0, 0, 0.06f);
		
		for( int i = 0; i < 3; i++){
			Handles.DrawBezier(startposition, endposition, startTan, endTan, shadowCol, null, (i+1) * 5);
		}
		
		Handles.DrawBezier(startposition, endposition, startTan, endTan, Color.black, null, 1);
		
	}
	
	[MenuItem("Window/Node Editor")]
	static void ShowEditor(){
		NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
		editor.mStopWatch.Start();
	}
	
	void Update(){
		long dTime = this.mStopWatch.ElapsedMilliseconds;
		float deltaTime = ((float)dTime)/1000;
		foreach(BaseNode n in this.windows){
			n.Tick(deltaTime);
		}
		
		this.mStopWatch.Reset();
		this.mStopWatch.Start();
		
		Repaint();
	}
	
	void OnGUI(){
		Event e = Event.current;
		this.mMousePosition = e.mousePosition;
		int selectedIndex = -1;

		if(e.button == 1 && !this.mInTransitionNode){
			if(e.type == EventType.MouseDown){
				bool clickedOnWindow = false;
				
				for(int i = 0; i < this.windows.Count; i++){
					if(windows[i].mWindowRect.Contains(this.mMousePosition)){
						selectedIndex = i;
						clickedOnWindow = true;
						break;
					}
				}
				
				if(!clickedOnWindow){
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("Add Input Node"), false, this.ContextCallBack, "inputNode");
					menu.AddItem(new GUIContent("Add Output Node"), false, this.ContextCallBack, "outputNode");
					menu.AddItem(new GUIContent("Add Calculation Node"), false, this.ContextCallBack, "calculateNode");
					menu.AddItem(new GUIContent("Add Comparison Node"), false, this.ContextCallBack, "comparisonNode");
					menu.AddItem(new GUIContent("Add Timer Node"), false, this.ContextCallBack, "timerNode");
					menu.AddSeparator("");
					menu.AddItem(new GUIContent("Add GameObject Node"), false, this.ContextCallBack, "goActive");
					menu.AddItem(new GUIContent("Add Distance Node"), false, this.ContextCallBack, "goDistance");

					menu.ShowAsContext();
					e.Use();
					
				}else{
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("Make Transition"), false, this.ContextCallBack, "makeTransition");
					menu.AddSeparator("");
					menu.AddItem(new GUIContent("Delete Node"), false, this.ContextCallBack, "deleteNode");

					menu.ShowAsContext();
					e.Use();				
				}
				
			}
		}else if(e.button == 0 && e.type == EventType.MouseDown && this.mInTransitionNode){
			bool clickedOnWindow = false;

			for(int i = 0; i < this.windows.Count; i++){
				if(windows[i].mWindowRect.Contains(this.mMousePosition)){
					selectedIndex = i;
					clickedOnWindow = true;
					break;
				}
			}
			
			
			if(clickedOnWindow && !windows[selectedIndex].Equals(this.mSelectedNode)){
				this.windows[selectedIndex].SetInput((BaseInputNode)this.mSelectedNode, this.mMousePosition);
				this.mInTransitionNode = false;
				this.mSelectedNode = null;
			}
			
			if(!clickedOnWindow){
				this.mInTransitionNode = false;
				this.mSelectedNode = null;
			}
			
			e.Use();
		}else if(e.button == 0 && e.type == EventType.MouseDown && !this.mInTransitionNode){
			bool clickedOnWindow = false;

			for(int i = 0; i < this.windows.Count; i++){
				if(windows[i].mWindowRect.Contains(this.mMousePosition)){
					selectedIndex = i;
					clickedOnWindow = true;
					break;
				}
			}

			if(clickedOnWindow){
				BaseInputNode nodeToChange = this.windows[selectedIndex].ClickedOnInput(this.mMousePosition);
				
				if(nodeToChange != null){
					this.mSelectedNode = nodeToChange;
					this.mInTransitionNode = true;
				}
			}
		}
		
		if(this.mInTransitionNode && this.mSelectedNode != null){
			Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);
			DrawNodeCurve(this.mSelectedNode.mWindowRect, mouseRect);
			
			Repaint();
		}
		
		foreach(BaseNode n in this.windows){
			n.DrawCurves();
		}
		
		BeginWindows();
		
		for(int i = 0; i < this.windows.Count; i++){
			windows[i].mWindowRect = GUI.Window(i, windows[i].mWindowRect, this.DrawNodeWindow, windows[i].mWindowTitle);
		}
		
		EndWindows();
	}
	
	void DrawNodeWindow(int id) {
		windows[id].DrawWindow();
		GUI.DragWindow();
	}
	
	void ContextCallBack(object obj) {
		string callback = obj.ToString();
		if(callback.Equals("inputNode")){
			InputNode node = ScriptableObject.CreateInstance<InputNode>();
			node.mWindowRect = new Rect(this.mMousePosition.x, this.mMousePosition.y, 200, 150);
			windows.Add(node);
		}else if(callback.Equals("outputNode")){
			OutputNode node = ScriptableObject.CreateInstance<OutputNode>();
			node.mWindowRect = new Rect(this.mMousePosition.x, this.mMousePosition.y, 200, 100);
			windows.Add(node);
		}else if(callback.Equals("calculateNode")){
			CalculateNode node = ScriptableObject.CreateInstance<CalculateNode>();
			node.mWindowRect = new Rect(this.mMousePosition.x, this.mMousePosition.y, 200, 100);
			windows.Add(node); 
		}else if(callback.Equals("comparisonNode")){
			ComparisonNode node = ScriptableObject.CreateInstance<ComparisonNode>();
			node.mWindowRect = new Rect(this.mMousePosition.x, this.mMousePosition.y, 200, 100);
			windows.Add(node); 
		}else if(callback.Equals("goActive")){
			GameObjectActive node = ScriptableObject.CreateInstance<GameObjectActive>();
			node.mWindowRect = new Rect(this.mMousePosition.x, this.mMousePosition.y, 200, 200);
			windows.Add(node);
		}else if(callback.Equals("goDistance")){
			GameObjectDistance node = ScriptableObject.CreateInstance<GameObjectDistance>();
			node.mWindowRect = new Rect(this.mMousePosition.x, this.mMousePosition.y, 200, 200);
			windows.Add(node);
			
		}else if(callback.Equals("timerNode")){
			TimerNode node = ScriptableObject.CreateInstance<TimerNode>();
			node.mWindowRect = new Rect(this.mMousePosition.x, this.mMousePosition.y, 200, 100);
			windows.Add(node);			
		}else if(callback.Equals("makeTransition")){
			bool clickedOnWindow = false;
			int selectedIndex = -1;

			for(int i = 0; i < this.windows.Count; i++){
				if(windows[i].mWindowRect.Contains(this.mMousePosition)){
					selectedIndex = i;
					clickedOnWindow = true;
					break;
				}
			}
			
			if(clickedOnWindow){
				this.mSelectedNode = this.windows[selectedIndex];
				this.mInTransitionNode = true;
			}
		}else if(callback.Equals("deleteNode")){
			bool clickedOnWindow = false;
			int selectedIndex = -1;

			for(int i = 0; i < this.windows.Count; i++){
				if(windows[i].mWindowRect.Contains(this.mMousePosition)){
					selectedIndex = i;
					clickedOnWindow = true;
					break;
				}
			}

			if(clickedOnWindow){
				BaseNode selNode = this.windows[selectedIndex];
				windows.RemoveAt(selectedIndex);
				foreach(BaseNode n in this.windows){
					n.NodeDeleted(selNode);
				}
			}
		}
	}
}
