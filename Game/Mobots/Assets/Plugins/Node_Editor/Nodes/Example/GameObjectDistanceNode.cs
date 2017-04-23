using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.Standard {
	[Node (false, "Example/GameObjectDistance Node")]
	public class GameObjectDistanceNode : Node {
		
		public enum CalcType { Add, Substract, Multiply, Divide }
		public CalcType type = CalcType.Add;
		
		public const string ID = "gamobjectDistanceNode";
		public override string GetID { get { return ID; } }
		
		public GameObject inputVal1;
		

		public override Node Create (Vector2 pos) 
		{
			GameObjectDistanceNode node = CreateInstance <GameObjectDistanceNode> ();

			node.name = "Calc Node";
			node.rect = new Rect (pos.x, pos.y, 200, 100);

			node.CreateInput ("Input 1", "GameObject");


			return node;
		}

		protected internal override void NodeGUI () 
		{
			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ();

			Inputs [0].DisplayLayout (new GUIContent ("Texture", "The texture to output."));

			if (inputVal1 != null) 
			{
				GUILayout.Label(inputVal1.name);
			}			
			InputKnob (0);
			// --
			GUILayout.EndVertical ();
			GUILayout.BeginVertical ();

			Outputs [0].DisplayLayout ();

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

			if (GUI.changed)
				NodeEditor.RecalculateFrom (this);

		}

		public override bool Calculate () 
		{
			if (Inputs [0].connection == null || Inputs [0].connection.IsValueNull)
				inputVal1 = null;
			else
				inputVal1 = Inputs [0].connection.GetValue<GameObject> ();
			return true;
		}
	}
}

