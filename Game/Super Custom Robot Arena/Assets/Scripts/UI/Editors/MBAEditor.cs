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
		public class MBAEditor : MonoBehaviour, DialogInterFace.OnClickListener {
			
			public GameObject mMainStage;
			public GameObject mRobotPlaceholder;
			public GameObject DialogPrefab;
			public bool mStartImmidiatly = false;
			public Color mGood, mBad, mEcual;
			public string mCurrentRobotName = "MKVII", mCurrentRobotNameRarm = "MKVII",  mCurrentRobotNameLarm = "MKVII",  mCurrentRobotNameCar = "MKVII";
			public Text	mTitle, mOldName, mNewName;
			public Text[] mStats, mCurrentTextParts, mOverallTexts;
			public Button mButtonHolder;
			public Image mReferenceImage;
			
			private string mSlot = "";
			[SerializeField]
			private bool assigned = false;
			/// <summary>
			/// The manager to assign values to
			/// a particular robot part
			/// </summary>
			private AssignValues mAssign;
			private GameManager manager;
			private RobotEditor mRobotEditor;
			private PART mPart = PART.HEAD;
			[SerializeField]
			private DialogInterFace.Builder builder;
			
			public PART GetPart() {
				return this.mPart;
			}
			
			#region MENULISTENER
			
			public void CheckRobot(string robotName) {
				this.mReferenceImage.sprite = RevealImageByName(robotName);
				this.mButtonHolder.GetComponent<DynamicListener>().mMessageParameter = robotName;
				this.ChangeText(robotName);	
			}
			
			public void EquipRobot (string robotName = ""){
				// holder for the part
				GameObject holder = null;
				if(this.mButtonHolder != null) {
					robotName = (robotName == "" ) ? this.mButtonHolder.GetComponent<DynamicListener>().mMessageParameter : robotName;
				
					switch (this.mPart) {
						case PART.HEAD:
							holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_head", typeof(GameObject));	
							this.mCurrentTextParts[0].text = robotName + " head";
							this.mCurrentRobotName = robotName;
							break;
						case PART.LARM:
							holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_larm", typeof(GameObject));		
							this.mCurrentTextParts[1].text = robotName + " left arm";
							this.mCurrentRobotNameLarm = robotName;
							break;
						case PART.RARM:
							holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_rarm", typeof(GameObject));		
							this.mCurrentTextParts[2].text = robotName + " right arm";
							this.mCurrentRobotNameRarm = robotName;
							break;
						case PART.CAR:
							holder = (GameObject)Resources.Load ("Robots/" + robotName + "/" + robotName + "_car", typeof(GameObject));		
							this.mCurrentTextParts[3].text = robotName + " car";
							this.mCurrentRobotNameCar = robotName;
							break;
					}
	
					if (this.mAssign != null && this.mRobotEditor != null && holder != null && this.mPart != PART.UNASSIGNED )
						// Change the robot part with the new object (assign is a callback)
						this.mRobotEditor.SetRobot (this.mPart, holder);
					
//					this.ChangeText(robotName);
					this.Initialize();
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
			
			public void SaveToSlot(string index){
				this.mSlot = "slot_#" + index + ".txt";
//				Debug.Log(GameUtilities.CheckFileExists("Slots/", slot));
				DialogInterFace[] interfaces = GameObject.FindObjectsOfType<DialogInterFace>();
				if(interfaces.Length > 0){
					foreach(DialogInterFace i in interfaces){
						Destroy(i.gameObject);
					}
				}
				if(GameUtilities.CheckFileExists("Slots/", this.mSlot)){
					GameObject d = (GameObject) Instantiate(DialogPrefab, DialogPrefab.transform.position, Quaternion.identity);					
					DialogInterFace.Builder builder = new DialogInterFace.Builder(d.GetComponentInChildren<DialogInterFace>());
					builder.SetMessage("Save slot is already in use.\nDo you want to override it?\nThis can't be undone.");
					builder.SetNegativeButton("NO", this);
					builder.SetPositiveButton("YES", this);
					DialogInterFace alert = builder.Create();
					alert.Show();	
				}else{
					this.CreateSlot();
				}				
			}
			
			public void OnClick(DialogInterFace dialog, int which){
				switch(which){
					case DialogInterFace.BUTTON_POSITIVE:
						if(!GameUtilities.DeleteFile("Slots/", this.mSlot)){
							this.CreateSlot();
						}
						break;
					case DialogInterFace.BUTTON_NEGATIVE:
						if(this.mStartImmidiatly){
							GameObject.FindGameObjectWithTag("Menu").SendMessage("SetNextPage", "Enemy");
						}
						break;
				}
				
				dialog.Dismiss();
			}
			
			#endregion
			
			#region UNITYMETHODS    
			
			void Awake () {
				manager = GameObject.FindObjectOfType<GameManager>();
//				Destroy(Camera.main.gameObject.GetComponent<Skybox>());
				Destroy(Camera.main.gameObject.GetComponent<SimpleRotation>());
				Camera.main.gameObject.transform.rotation = Quaternion.identity;
				GameObject.Find("Cylinder").GetComponent<Renderer>().enabled = true;
				
				this.mOldName.text = this.mNewName.text = "";
				string file = "";
				if(GameUtilities.CheckFileExists("Slots/", "slot_#1.txt")){
					file = GameUtilities.ReadFile("Slots/", "slot_#1.txt");
				}
				
				if(file == ""){
					if(GameUtilities.CheckFileExists("Slots/", "slot_#2.txt")){
						file =  GameUtilities.ReadFile("Slots/", "slot_#2.txt");
					}
				}
				
				if(file == ""){
					if(GameUtilities.CheckFileExists("Slots/", "slot_#3.txt")){
						file = GameUtilities.ReadFile("Slots/", "slot_#3.txt");
					}
				}
				
				if(file == "")
					file = GameUtilities.ReadResource("Slots/standard");
				
				JSONObject j = JSONObject.Parse(file).GetObject("robot");
				
				this.mCurrentRobotName = j.GetString("head");
				this.mCurrentRobotNameLarm = j.GetString("left");
				this.mCurrentRobotNameRarm = j.GetString("right");
				this.mCurrentRobotNameCar = j.GetString("car");
			}
						
			void Start () {
				this.mRobotEditor = GameObject.FindGameObjectWithTag("Robot").GetComponent<RobotEditor>();
				this.mRobotEditor.transform.position = new Vector3(this.mRobotEditor.transform.position.x, 0.04f, this.mRobotEditor.transform.position.z );
				this.mAssign = new AssignValues (ChangeStats);
				this.mPart = PART.HEAD;
				this.EquipRobot(this.mCurrentRobotName);
				this.mPart = PART.LARM;
				this.EquipRobot(this.mCurrentRobotNameLarm);
				this.mPart = PART.RARM;
				this.EquipRobot(this.mCurrentRobotNameRarm);
				this.mPart = PART.CAR;
				this.EquipRobot(this.mCurrentRobotNameCar);				
				this.Initialize();
				this.transform.localRotation = Quaternion.identity;
				this.mPart = PART.HEAD;
			}
			
			void FixedUpdate(){
				if( SceneManager.GetActiveScene().name == "demo_arena" && this.assigned == false){
					// clean up begins
					StartCoroutine(this.SaveRobot());
					this.assigned = true;
				}
			}
			
			#endregion
			
			private void Initialize(){
				if(this.mOverallTexts.Length == 12){
					// json object from the robots dictionary
					JSONObject holder;
					float overallHP = 0;
					float weight = 0;
					// overall hp
					// shield hp
					// shield strength
					// left dps
					// left dpr
					// left acc
					// right dps
					// right dpr
					// right acc
					// speed
					// weight

					holder = manager.robotDictionary[this.mCurrentRobotName].GetObject("parts").GetObject("head").GetObject("stats");
					overallHP += (float)holder.GetNumber("hitpoints");
					this.mOverallTexts[1].text = holder.GetNumber("shieldhitpoints") + "";
					this.mOverallTexts[2].text = holder.GetNumber("shieldstrength") + "%";
					weight += (float)holder.GetNumber("weight");

					holder = manager.robotDictionary[this.mCurrentRobotNameLarm].GetObject("parts").GetObject("left").GetObject("stats");
					overallHP += (float)holder.GetNumber("hitpoints");
					weight += (float)holder.GetNumber("weight");
					this.mOverallTexts[3].text = holder.GetNumber("roundspersecond") + "";
					this.mOverallTexts[4].text = holder.GetNumber("damageperround") + "";
					this.mOverallTexts[5].text = holder.GetNumber("accuracy") + "";

					holder = manager.robotDictionary[this.mCurrentRobotNameRarm].GetObject("parts").GetObject("right").GetObject("stats");
					overallHP += (float)holder.GetNumber("hitpoints");
					weight += (float)holder.GetNumber("weight");
					this.mOverallTexts[6].text = holder.GetNumber("roundspersecond") + "";
					this.mOverallTexts[7].text = holder.GetNumber("damageperround") + "";
					this.mOverallTexts[8].text = holder.GetNumber("accuracy") + "";

					holder = manager.robotDictionary[this.mCurrentRobotNameCar].GetObject("parts").GetObject("car").GetObject("stats");
					overallHP += (float)holder.GetNumber("hitpoints");
					weight = (float)holder.GetNumber("weight");
					this.mOverallTexts[9].text = holder.GetNumber("speed") + "N";
					this.mOverallTexts[10].text = holder.GetNumber("jumpstrength") + "N";

					this.mOverallTexts[0].text = overallHP.ToString("0");
					this.mOverallTexts[11].text = weight.ToString("0") + "KG";								

				}
			}
			
			/// <summary>
			/// Method to change the stats of a part.
			/// </summary>
			/// <param name="part">Part.</param>
			/// <param name="robotName">Robot name.</param>
			private void ChangeStats(string robotName = "", Player p = null){
				
				if (robotName == "")
					return;

				// json object from the robots dictionary
				JSONObject json = manager.robotDictionary[robotName].GetObject("parts");
				switch (this.mPart) {
					case PART.HEAD:
						//				head = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
						//				json = JSONObject.Parse(head);
						json = json.GetObject("head").GetObject("stats");
						p.SetValue (this.mPart, "SetHealth", (float)json.GetNumber("hitpoints"));
						p.SetValue (this.mPart, "SetArmor", (float)json.GetNumber("shieldhitpoints"));
						p.SetValue (this.mPart, "SetStrength", (float)json.GetNumber("shieldstrength"));
						p.SetValue (this.mPart, "SetWeight", (int)json.GetNumber("weight"));
						break;
					case PART.LARM:
						//			    arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
						//			    json = JSONObject.Parse(arm);
						json = json.GetObject("left").GetObject("stats");
						p.SetValue (this.mPart, "SetHealth", (float)json.GetNumber("hitpoints"));
						p.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
						p.SetValue (this.mPart, "SetDamagePerRound", (float)json.GetNumber ("damageperround"));
						p.SetValue (this.mPart, "SetRoundsPerSecond", (float)json.GetNumber ("roundspersecond"));
						p.SetValue (this.mPart, "SetAccuracy", (float)json.GetNumber ("accuracy"));
						break;
					case PART.RARM:
						//			    arm = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
						//			    json = JSONObject.Parse(arm);
						json = json.GetObject("right").GetObject("stats");
						p.SetValue (this.mPart, "SetHealth", (float)json.GetNumber ("hitpoints"));
						p.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
						p.SetValue (this.mPart, "SetDamagePerRound", (float)json.GetNumber ("damageperround"));
						p.SetValue (this.mPart, "SetRoundsPerSecond", (float)json.GetNumber ("roundspersecond"));
						p.SetValue (this.mPart, "SetAccuracy", (float)json.GetNumber("accuracy"));
						break;
					case PART.CAR:
						//			    car = GameUtilities.ReadFile ("Robots/" + robotName + "/" + robotName + "_" + part + "_stats");
						//			    json = JSONObject.Parse(car);
						json = json.GetObject("car").GetObject("stats");
						p.SetValue (this.mPart, "SetHealth", (float)json.GetNumber ("hitpoints"));
						p.SetValue (this.mPart, "SetWeight", (int)json.GetNumber ("weight"));
						p.SetValue (this.mPart, "SetSpeed", (float)json.GetNumber ("speed"));
						p.SetValue (this.mPart, "SetJumpStrength", (float)json.GetNumber ("jumpstrength"));
						break;
				}
			}
			
			private void ChangeText(string robotName) {
				this.mOldName.text = this.mCurrentRobotName;
				this.mNewName.text = robotName;
				if(this.mStats.Length == 8){
					// json object from the robots dictionary
					JSONObject oldrobotpartjson = manager.robotDictionary[this.mCurrentRobotName].GetObject("parts");
					JSONObject json = manager.robotDictionary[robotName].GetObject("parts");	

					switch (this.mPart) {
						case PART.HEAD:
							oldrobotpartjson = manager.robotDictionary[this.mCurrentRobotName].GetObject("parts").GetObject("head").GetObject("stats");
							this.mStats[0].text = oldrobotpartjson.GetNumber("hitpoints") + "";
							this.mStats[1].text = oldrobotpartjson.GetNumber("shieldhitpoints") + "";
							this.mStats[2].text = oldrobotpartjson.GetNumber("shieldstrength") + "";
							this.mStats[3].text = oldrobotpartjson.GetNumber("weight") + "";		

							json = json.GetObject("head").GetObject("stats");
							this.mStats[4].text = json.GetNumber("hitpoints") + "";
							this.mStats[5].text = json.GetNumber("shieldhitpoints") + "";
							this.mStats[6].text = json.GetNumber("shieldstrength") + "";
							this.mStats[7].text = json.GetNumber("weight") + "";
							
							if(oldrobotpartjson.GetNumber("hitpoints") > json.GetNumber("hitpoints")) {
								this.mStats[0].color = this.mGood;
								this.mStats[4].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("hitpoints") == json.GetNumber("hitpoints")){
								this.mStats[0].color = this.mEcual;
								this.mStats[4].color = this.mEcual;
							}else {
								this.mStats[0].color = this.mBad;
								this.mStats[4].color = this.mGood;								
							}
							
							if(oldrobotpartjson.GetNumber("shieldhitpoints") > json.GetNumber("shieldhitpoints")){
								this.mStats[1].color = this.mGood;
								this.mStats[5].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("shieldhitpoints") == json.GetNumber("shieldhitpoints")){
								this.mStats[1].color = this.mEcual;
								this.mStats[5].color = this.mEcual;
							}else{
								this.mStats[1].color = this.mBad;
								this.mStats[5].color = this.mGood;
							}
							
							if(oldrobotpartjson.GetNumber("shieldstrength") > json.GetNumber("shieldstrength")) {
								this.mStats[2].color = this.mGood;
								this.mStats[6].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("shieldstrength") == json.GetNumber("shieldstrength")){
								this.mStats[2].color = this.mEcual;
								this.mStats[6].color = this.mEcual;
							}else {
								this.mStats[2].color = this.mBad;
								this.mStats[6].color = this.mGood;
							}
							
							if(oldrobotpartjson.GetNumber("weight") > json.GetNumber("weight")) {
								this.mStats[3].color = this.mGood;
								this.mStats[7].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("weight") == json.GetNumber("weight")){
								this.mStats[3].color = this.mEcual;
								this.mStats[7].color = this.mEcual;
							}else {
								this.mStats[3].color = this.mBad;
								this.mStats[7].color = this.mGood;
							}
							
							break;
						case PART.LARM:
							oldrobotpartjson = manager.robotDictionary[this.mCurrentRobotNameLarm].GetObject("parts").GetObject("left").GetObject("stats");
							this.mStats[0].text = oldrobotpartjson.GetNumber("hitpoints") + "";
							this.mStats[1].text = "";
							this.mStats[2].text = "";
							this.mStats[3].text = oldrobotpartjson.GetNumber("weight") + "";	

							json = json.GetObject("left").GetObject("stats");
							this.mStats[4].text = json.GetNumber("hitpoints") + "";
							this.mStats[5].text = "";
							this.mStats[6].text = "";
							this.mStats[7].text = json.GetNumber("weight") + "";
							
							if(oldrobotpartjson.GetNumber("hitpoints") > json.GetNumber("hitpoints")) {
								this.mStats[0].color = this.mGood;
								this.mStats[4].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("hitpoints") == json.GetNumber("hitpoints")){
								this.mStats[0].color = this.mEcual;
								this.mStats[4].color = this.mEcual;	
							}else{
								this.mStats[0].color = this.mBad;
								this.mStats[4].color = this.mGood;
							}

							if(oldrobotpartjson.GetNumber("weight") > json.GetNumber("weight")) {
								this.mStats[3].color = this.mGood;
								this.mStats[7].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("weight") == json.GetNumber("weight")){
								this.mStats[3].color = this.mEcual;
								this.mStats[7].color = this.mEcual;	
							}else {
								this.mStats[3].color = this.mBad;
								this.mStats[7].color = this.mGood;
							}
							
							break;
						case PART.RARM:
							oldrobotpartjson = manager.robotDictionary[this.mCurrentRobotNameRarm].GetObject("parts").GetObject("right").GetObject("stats");
							this.mStats[0].text = oldrobotpartjson.GetNumber("hitpoints") + "";
							this.mStats[1].text = "";
							this.mStats[2].text = "";
							this.mStats[3].text = oldrobotpartjson.GetNumber("weight") + "";	

							json = json.GetObject("right").GetObject("stats");
							this.mStats[4].text = json.GetNumber("hitpoints") + "";
							this.mStats[5].text = "";
							this.mStats[6].text = "";
							this.mStats[7].text = json.GetNumber("weight") + "";
							
							if(oldrobotpartjson.GetNumber("hitpoints") > json.GetNumber("hitpoints")) {
								this.mStats[0].color = this.mGood;
								this.mStats[4].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("hitpoints") == json.GetNumber("hitpoints")){
								this.mStats[0].color = this.mEcual;
								this.mStats[4].color = this.mEcual;	
							}else{
								this.mStats[0].color = this.mBad;
								this.mStats[4].color = this.mGood;
							}

							if(oldrobotpartjson.GetNumber("weight") > json.GetNumber("weight")) {
								this.mStats[3].color = this.mGood;
								this.mStats[7].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("weight") == json.GetNumber("weight")){
								this.mStats[3].color = this.mEcual;
								this.mStats[7].color = this.mEcual;	
							}else {
								this.mStats[3].color = this.mBad;
								this.mStats[7].color = this.mGood;
							}
							
							break;
						case PART.CAR:
							oldrobotpartjson = manager.robotDictionary[this.mCurrentRobotNameCar].GetObject("parts").GetObject("car").GetObject("stats");
							this.mStats[0].text = oldrobotpartjson.GetNumber("hitpoints") + "";
							this.mStats[1].text = "";
							this.mStats[2].text = "";
							this.mStats[3].text = oldrobotpartjson.GetNumber("weight") + "";	

							json = json.GetObject("car").GetObject("stats");
							this.mStats[4].text = json.GetNumber("hitpoints") + "";
							this.mStats[5].text = "";
							this.mStats[6].text = "";
							this.mStats[7].text = json.GetNumber("weight") + "";
							
							if(oldrobotpartjson.GetNumber("hitpoints") > json.GetNumber("hitpoints")) {
								this.mStats[0].color = this.mGood;
								this.mStats[4].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("hitpoints") == json.GetNumber("hitpoints")){
								this.mStats[0].color = this.mEcual;
								this.mStats[4].color = this.mEcual;	
							}else{
								this.mStats[0].color = this.mBad;
								this.mStats[4].color = this.mGood;
							}

							if(oldrobotpartjson.GetNumber("weight") > json.GetNumber("weight")) {
								this.mStats[3].color = this.mGood;
								this.mStats[7].color = this.mBad;
							}else if(oldrobotpartjson.GetNumber("weight") == json.GetNumber("weight")){
								this.mStats[3].color = this.mEcual;
								this.mStats[7].color = this.mEcual;	
							}else {
								this.mStats[3].color = this.mBad;
								this.mStats[7].color = this.mGood;
							}
							
							break;
					}
				}
			}
		
			private void CreateSlot(){
				//You can also create an "empty" JSONObject
				JSONObject parentObj = new JSONObject();
				JSONObject robotObj = new JSONObject();

				robotObj.Add("head", this.mCurrentRobotName);
				robotObj.Add("left", this.mCurrentRobotNameLarm);
				robotObj.Add("right", this.mCurrentRobotNameRarm);
				robotObj.Add("car", this.mCurrentRobotNameCar);

				//Adding values is easy (values are implicitly converted to JSONValues):
				parentObj.Add("robot", robotObj);	
				GameUtilities.WriteFile("Slots/", this.mSlot, parentObj.ToString());
				if(this.mStartImmidiatly){
					GameObject.FindGameObjectWithTag("Menu").SendMessage("SetNextPage", "Enemy");
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
			
			private IEnumerator SaveRobot(){
				Player holder = mRobotEditor.gameObject.AddComponent<Player>();
				holder.GetPartObj(0).AddComponent<Head>();
				holder.GetPartObj(1).AddComponent<Larm>();
				holder.GetPartObj(2).AddComponent<Rarm>();
				holder.GetPartObj(3).AddComponent<Car>();
				yield return new WaitForSeconds(2.0f);
				holder.Initialize();
				holder.isControllable = false;
				this.mPart = PART.HEAD;
				this.ChangeStats(this.mCurrentRobotName, holder);
				this.mPart = PART.LARM;
				this.ChangeStats(this.mCurrentRobotNameLarm, holder);
				this.mPart = PART.RARM;
				this.ChangeStats(this.mCurrentRobotNameRarm, holder);
				this.mPart = PART.CAR;
				this.ChangeStats(this.mCurrentRobotNameCar, holder);				
				this.mPart = PART.HEAD;
				yield return null;
			}
		}
	}
}
