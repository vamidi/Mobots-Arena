using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

using Boomlagoon.JSON;

namespace MBA {
	
	namespace UI {
				
//		Robot stats pakken
//		Robot content aansturen
		
		/// <summary>
		/// Call back for when the robot is done with building
		/// </summary>
		public delegate void AssignValues (string name);
		
		/// <summary>
		/// Editor class.
		/// 
		/// </summary>
		public class MBAEditor : MonoBehaviour {
			
			public Text	mTitle;
			public Button mButtonHolder;
			public Image mReferenceImage;
			/// <summary>
			/// The manager to assign values to
			/// a particular robot part
			/// </summary>
			private AssignValues mAssign;
			private JSONArray mRobotArray;
			/// <summary>
			/// The dictionary that contains the robots and their parts
			/// </summary>
			private Dictionary<string, JSONObject>mRobots = new Dictionary<string, JSONObject>();
			private RobotEditor mRobotEditor;
			private PART mPart = PART.HEAD;
			
			public PART GetPart() {
				return this.mPart;
			}
			
			public int GetRobotSize () {
				return this.mRobots.Count;
			}
			
			public JSONValue GetRobot(int index){
				return this.mRobotArray[index];
			}
			
			public void CheckRobot(string robotName) {
				this.mReferenceImage.sprite = RevealImageByName(robotName);
				this.mButtonHolder.GetComponent<DynamicListener>().mMessageParameter = robotName;
			}
			
			public void EquipRobot (){
				// holder for the part
				GameObject holder = null;
				if(this.mButtonHolder != null) {
					string robotName = this.mButtonHolder.GetComponent<DynamicListener>().mMessageParameter;
				
					switch (this.mPart) {
						case PART.HEAD:
							holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_head", typeof(GameObject));	
							break;
						case PART.LARM:
							holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_larm", typeof(GameObject));		
							break;
						case PART.RARM:
							holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_rarm", typeof(GameObject));		
							break;
						case PART.CAR:
							holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_car", typeof(GameObject));		
							break;
					}
	
					if (this.mAssign != null && this.mRobotEditor != null && holder != null && this.mPart != PART.UNASSIGNED )
						// Change the robot part with the new object (assign is a callback)
						this.mRobotEditor.SetRobot (this.mPart, robotName, holder, this.mAssign);
				}
			}
	
			public void ChangeTitle(string section){
				switch(section.ToLower()) {
					case "head":
						this.mPart = PART.HEAD;
						this.mTitle.text = "Head Parts";
						break;
					case "left":
						this.mPart = PART.LARM;
						this.mTitle.text = "Left Parts";
						break;
					case "right":
						this.mPart = PART.RARM;
						this.mTitle.text = "Right Parts";
						break;
					case "car":
						this.mPart = PART.CAR;
						this.mTitle.text = "Car Parts";
						break;
					default:
						this.mPart = PART.HEAD; 
						this.mTitle.text = "Head Parts";
						break;
				}
			}
			
			#region UNITYMETHODS    
			
			void Awake () {
				string robots = GameUtilities.ReadFile ("Robots/robots");
				this.mRobotArray = JSONObject.Parse(robots).GetArray("robots");
				foreach(JSONValue o in this.mRobotArray) {
					// Set the json of the robot into the dictionary.
					mRobots.Add(o.Obj.GetString("robotname"), o.Obj);
				}
			}
						
			void Start () {
				this.mRobotEditor = GameObject.FindGameObjectWithTag("Robot").GetComponent<RobotEditor>();
				this.mAssign = new AssignValues (ChangeStats);
			}
			
			#endregion
			
			/// <summary>
			/// Method to change the stats of a part.
			/// </summary>
			/// <param name="part">Part.</param>
			/// <param name="robotName">Robot name.</param>
			private void ChangeStats(string robotName = ""){

				if (robotName == "")
					return;

				// json object from the robots dictionary
				JSONObject json = mRobots[robotName].GetObject("parts");

				switch (this.mPart) {
					case PART.HEAD:
						//				head = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
						//				json = JSONObject.Parse(head);
						json = json.GetObject("head").GetObject("stats");
						this.mRobotEditor.SetValue (this.mPart, "SetHealth", (float)json.GetNumber("hitpoints"));
						this.mRobotEditor.SetValue (this.mPart, "SetArmor", (float)json.GetNumber("shieldhitpoints"));
						this.mRobotEditor.SetValue (this.mPart, "SetStrength", (float)json.GetNumber("shieldstrength"));
						this.mRobotEditor.SetValue (this.mPart, "SetWeight", (int)json.GetNumber("weight"));
						break;
					case PART.LARM:
						//			    arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
						//			    json = JSONObject.Parse(arm);
						json = json.GetObject("left").GetObject("stats");
						this.mRobotEditor.SetValue (this.mPart, "SetHealth", (float)json.GetNumber("hitpoints"));
						this.mRobotEditor.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
						this.mRobotEditor.SetValue (this.mPart, "SetDamagePerRound", (float)json.GetNumber ("damageperround"));
						this.mRobotEditor.SetValue (this.mPart, "SetRoundsPerSecond", (float)json.GetNumber ("roundspersecond"));
						this.mRobotEditor.SetValue (this.mPart, "SetAccuracy", (float)json.GetNumber ("accuracy"));
						break;
					case PART.RARM:
						//			    arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
						//			    json = JSONObject.Parse(arm);
						json = json.GetObject("right").GetObject("stats");
						this.mRobotEditor.SetValue (this.mPart, "SetHealth", (float)json.GetNumber ("hitpoints"));
						this.mRobotEditor.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
						this.mRobotEditor.SetValue (this.mPart, "SetDamagePerRound", (float)json.GetNumber ("damageperround"));
						this.mRobotEditor.SetValue (this.mPart, "SetRoundsPerSecond", (float)json.GetNumber ("roundspersecond"));
						this.mRobotEditor.SetValue (this.mPart, "SetAccuracy", (float)json.GetNumber("accuracy"));
						break;
					case PART.CAR:
						//			    car = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
						//			    json = JSONObject.Parse(car);
						json = json.GetObject("car").GetObject("stats");
						this.mRobotEditor.SetValue (this.mPart, "SetHealth", (float)json.GetNumber ("hitpoints"));
						this.mRobotEditor.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
						this.mRobotEditor.SetValue (this.mPart, "SetSpeed", (float)json.GetNumber ("speed"));
						this.mRobotEditor.SetValue (this.mPart, "SetJumpStrength", (float)json.GetNumber ("jumpstrength"));
						break;
				}
			}
			
			private Sprite RevealImageByName (string robotName) {
				Texture2D t2d = null;
				Sprite holder = null;
				switch (this.mPart) {
					case PART.HEAD:
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_head_image");	
						break;
					case PART.LARM:
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_larm_image");		
						break;
					case PART.RARM:
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_rarm_image");		
						break;
					case PART.CAR:
						t2d = Resources.Load<Texture2D> ("Robots/" + robotName + "/" + robotName.ToLower() + "_car_image");		
						break;
				}

				if(t2d)
					holder = Sprite.Create(t2d, new Rect(0,0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));

				return holder;
			}
		}
	}
}
