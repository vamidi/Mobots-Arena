using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.Standard {
	[Node (false, "Example/GameObject Node")]
	public class GameObjectNode : Node {

		public const string ID = "gameobjectNode";
		public override string GetID { get { return ID; } }
		
		public GameObject val1;

		public override Node Create (Vector2 pos) 
		{
			GameObjectNode node = CreateInstance <GameObjectNode> ();

			node.name = "GameObject Node";
			node.rect = new Rect (pos.x, pos.y, 200, 100);

			NodeOutput.Create (node, "Value", "GameObject");

			return node;
		}
		
		protected internal override void NodeGUI () 
		{
			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ();
			
			val1 = RTEditorGUI.ObjectField<GameObject> (val1, false);
			OutputKnob (0);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

			if (GUI.changed)
				NodeEditor.RecalculateFrom (this);

		}

		public override bool Calculate () 
		{
			Outputs[0].SetValue<GameObject> (val1);
			return true;
		}
	}
}

