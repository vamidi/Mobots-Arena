using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;

using Mobots.Robots;
using Boomlagoon.JSON;

namespace Mobots.UI {
	
	[System.Serializable]
	public class TagSettings {
		/// <summary>
		/// Tag for the head
		/// </summary>
		public string mHeadTag = "Head";
		/// <summary>
		/// Tag for the left arm
		/// </summary>
		public string mLarmTag = "Left";
		/// <summary>
		/// Tag for the right arm
		/// </summary>
		public string mRarmTag = "Right";
		/// <summary>
		/// Tag for the car
		/// </summary>
		public string mCarTag = "Car";
		/// <summary>
		/// Tag for the floor
		/// </summary>
		public string mGroundTag = "Ground";
		/// <summary>
		/// The robot tag.
		/// </summary>
		public string mRobotTag = "Robot";
		/// <summary>
		/// The enemy tag.
		/// </summary>
		public string mEnemyTag = "Enemy";
		/// <summary>
		/// The head UI tag
		/// </summary>
		public string mHeadUi = "HeadUI";
		/// <summary>
		/// The right UI tag
		/// </summary>
		public string mRightUi = "RightUI";
		/// <summary>
		/// The left UI tag
		/// </summary>
		public string mLeftUi = "LeftUI";
		/// <summary>
		/// Thecar UI tag
		/// </summary>
		public string mCarUi = "CarUI";
		/// <summary>
		/// The cancel key
		/// </summary>
		public string mCancel = "Cancel";
	}
	
	public class RobotController : MonoBehaviour {

		public static RobotController Instance {
			get {
				if (_applicationIsQuitting) {
					Debug.LogWarning("[Singleton] Instance '" + typeof(RobotController) +
					                 "' already destroyed on application quit." +
					                 " Won't create again - returning null.");
					return null;
				}
				
				lock (Lock) { // Lock is to block other threads from doing something else this statement needs to be completed
					if (_instance == null) {
						_instance = (RobotController) FindObjectOfType(typeof(RobotController));

						if (FindObjectsOfType(typeof(RobotController)).Length > 1) {
							Debug.LogError("[Singleton] Something went really wrong " +
							               " - there should never be more than 1 singleton!" +
							               " Reopening the scene might fix it.");
							return _instance;
						}

						if (_instance == null) {
							GameObject singleton = new GameObject();
							_instance = singleton.AddComponent<RobotController>();
							singleton.name = "(singleton) " + typeof(RobotController).ToString();

							DontDestroyOnLoad(singleton);

							Debug.Log("[Singleton] An instance of " + typeof(RobotController) +
							          " is needed in the scene, so '" + singleton +
							          "' was created with DontDestroyOnLoad.");
						} else {
							Debug.Log("[Singleton] Using instance already created: " +
							          _instance.gameObject.name);
						}
					}

					return _instance;
				}
			}
		}

		private static bool _applicationIsQuitting;
		/// <summary>
		/// Robot controller instance
		/// </summary>
		private static RobotController _instance;
		private static readonly object Lock = new object();

		/// <summary>
		/// The robot array.
		/// </summary>
		[SerializeField] 
		public JSONArray Robots {
			get; private set;
		}
			
	
		/// <summary>
		/// The dictionary that contains the robots and their parts
		/// </summary>
		public Dictionary<string, JSONObject>RobotDictionary {
			get; private set;
		}

		/// <summary>
		/// Classes of the parts
		/// </summary>
		[Header("Parts", order=0)] 
		[SerializeField] 
		protected Part[] mParts = new Part[4];	
		/// <summary>
		/// Gameobject of the parts
		/// </summary>
		[Header("Parts", order=1)] 
		[SerializeField] 
		protected GameObject goHead, goLarm, goRarm, goCar;
		protected readonly TagSettings mTags = new TagSettings();
		
		protected RobotController () { }

		void Start() {

		}

		void OnEnable() {
			GetParts();
			GetCurrentParts();			
		}

		void OnDisable() { 
			RobotDictionary = null;
			Robots = null;
		}


		/// <summary>
		/// Change the part of the robot.
		/// </summary>
		/// <param name="part">Part.</param>
		/// <param name="newObj">New object.</param>
		public void SetRobot(PART part, GameObject newObj) {
			GameObject holder, muzzle = null;

			switch (part) {
				case PART.Head:
					if ( (newObj && goHead) && newObj.name != goHead.name) {
						Transform parent = goHead.transform.parent;
						holder = (GameObject)Instantiate (newObj, goHead.transform.position, goHead.transform.rotation);
						holder.name = newObj.name;
						holder.tag = this.mTags.mHeadTag;
						//							holder.AddComponent<Head>();
						//							mParts [0] = holder.GetComponent<Head> ();
						holder.transform.parent = parent;
						holder.transform.localScale = newObj.transform.localScale;
						holder.transform.localPosition = newObj.transform.localPosition;
						Destroy (goHead);
						goHead = holder;

						//					goLarm.transform.localPosition = GameObject.Find("larm_spawn").transform.localPosition;
						//					goRarm.transform.localPosition = GameObject.Find("rarm_spawn").transform.localPosition;
					}
					break;
				case PART.Larm:
					if (newObj.name != goLarm.name) {
						Transform parent = goLarm.transform.parent;
						//							gunEnd = goLarm.GetComponentsInChildren<Transform>()[1].gameObject;
						if(goLarm.GetComponentsInChildren<Transform>().Length > 2)
							muzzle = goLarm.GetComponentsInChildren<Transform>()[2].gameObject;
						holder = (GameObject)Instantiate (newObj, goLarm.transform.position, goLarm.transform.rotation);
						//holder.transform.localPosition = GameObject.Find("larm_spawn").transform.localPosition;
						holder.name = newObj.name;
						if(!holder.GetComponent<LineRenderer>()){
							holder.AddComponent<LineRenderer>();
							ComponentUtility.PasteComponentValues(goLarm.GetComponent<LineRenderer>());
							holder.GetComponent<LineRenderer>().enabled = false;
						}

						holder.tag = this.mTags.mLarmTag;
						//							holder.AddComponent<Larm>();
						//							mParts [1] = holder.GetComponent<Larm> ();
						holder.transform.parent = parent;
						if(muzzle)
							muzzle.transform.parent = holder.transform;
						holder.transform.localScale = newObj.transform.localScale;
						Destroy (goLarm);
						goLarm = holder;
					}
					break;
				case PART.Rarm:
					if (newObj.name != goRarm.name) {
						Transform parent = goRarm.transform.parent;
						//							gunEnd = goRarm.GetComponentsInChildren<Transform>()[1].gameObject;
						if(goRarm.GetComponentsInChildren<Transform>().Length > 2)
							muzzle = goRarm.GetComponentsInChildren<Transform>()[2].gameObject;
						holder = (GameObject)Instantiate (newObj, goRarm.transform.position, goRarm.transform.rotation);
						holder.name = newObj.name;
						if(!holder.GetComponent<LineRenderer>()){
							holder.AddComponent<LineRenderer>();
							ComponentUtility.PasteComponentValues(goRarm.GetComponent<LineRenderer>());
							holder.GetComponent<LineRenderer>().enabled = false;
						}
						holder.tag = this.mTags.mRarmTag;
						//							holder.AddComponent<Rarm>();
						//							mParts [2] = holder.GetComponent<Rarm> ();
						holder.transform.parent = parent;
						if(muzzle)
							muzzle.transform.parent = holder.transform;
						holder.transform.localScale = newObj.transform.localScale;
						Destroy (goRarm);
						goRarm = holder;
					}
					break;
				case PART.Car:
					if (newObj.name != goCar.name) {
						Transform parent = goCar.transform.parent;
						holder = (GameObject)Instantiate (newObj, goCar.transform.position, goCar.transform.rotation);
						holder.name = newObj.name;
						holder.tag = this.mTags.mCarTag;
						//							holder.AddComponent<Car>();
						//							mParts [3] = holder.GetComponent<Car> ();
						holder.transform.parent = parent;
						holder.transform.localScale = newObj.transform.localScale;
						Destroy (goCar);
						goCar = holder;
					}
					break;
			}

			//				callBack (robotName);
		}

		
		/// <summary>
		/// When Unity quits, it destroys objects in a random order.
		/// In principle, a Singleton is only destroyed when application quits.
		/// If any script calls Instance after it have been destroyed, 
		///   it will create a buggy ghost object that will stay on the Editor scene
		///   even after stopping playing the Application. Really bad!
		/// So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		public void OnDestroy () {
			_applicationIsQuitting = true;
		}

		private void GetParts() {
			string allRobots = GameUtilities.ReadTextAsset ("Robots/robots");
			RobotDictionary = new Dictionary<string, JSONObject>();
			Robots = JSONObject.Parse(allRobots).GetArray("robots");

			foreach(JSONValue o in Robots) {
				// Set the json of the robot into the dictionary.
				RobotDictionary.Add(o.Obj.GetString("robotname"), o.Obj);
			}
		}

		private void GetCurrentParts() {
			foreach (Transform child in transform) {
				if (child.gameObject.CompareTag(mTags.mCarTag)) {
					goCar = child.gameObject;
				}
				if (child.childCount > 0) {
					foreach (Transform nodeChild in child) {
						if (nodeChild.gameObject.CompareTag(mTags.mHeadTag)) {
							goHead = nodeChild.gameObject;
						}
						foreach (Transform innerChild in nodeChild) {
							if (innerChild.gameObject.CompareTag(mTags.mLarmTag)) {
								goLarm = innerChild.gameObject;
							} else if (innerChild.gameObject.CompareTag(mTags.mRarmTag)) {
								goRarm = innerChild.gameObject;
							}
						}
					}
				}
			}
		}


		/// <summary>
		/// Sets the correct values to the right part
		/// </summary>
		/// <param name="part">Part.</param>
		/// <param name="method">Method.</param>
		/// <param name="value">Value.</param>
		private void SetValue(PART part, string method = "", object value = null)  {


			if (method == "" || value == null)
				return;

			switch (part) {
				case PART.Head:
					mParts [0].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
					break;
				case PART.Larm:
					mParts [1].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
					break;
				case PART.Rarm:
					mParts [2].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
					break;
				case PART.Car:
					mParts [3].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
					break;
			}
		}

	}
}
