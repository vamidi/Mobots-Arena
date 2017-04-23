using UnityEngine;
using System.Collections;
using NodeEditorFramework;

namespace NodeEditorFramework.Standard
{
	[System.Serializable]
	[Node (false, "Float/Display")]
	public class DisplayNode : Node 
	{
		public const string ID = "displayNode";
		public override string GetID { get { return ID; } }

		[HideInInspector]
		public bool assigned = false;
		public GameObject value;

		public override Node Create (Vector2 pos) 
		{ // This function has to be registered in Node_Editor.ContextCallback
			DisplayNode node = CreateInstance <DisplayNode> ();
			
			node.name = "Display Node";
			node.rect = new Rect (pos.x, pos.y, 150, 50);
						
			node.CreateInput ("Value", "GameObject");

			return node;
		}
		
		protected internal override void NodeGUI () 
		{
			if(value){
				string name = value.name;
				Inputs [0].DisplayLayout (new GUIContent ("Value : " + (assigned? name : ""), "The input value to display"));
			}
		}
		
		public override bool Calculate () 
		{
			if (!allInputsReady ()) 
			{
				value = null;
				assigned = false;
				return false;
			}

			value = Inputs[0].connection.GetValue<GameObject>();
			assigned = true;

			return true;
		}
	}
}