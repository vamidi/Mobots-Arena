using UnityEngine;
using System.Collections;

namespace MBA {
	
	namespace UI {

		public class RobotEditor : MonoBehaviour {
			
			/// <summary>
			/// The Tags
			/// </summary>
			public TagSettings mTags = new TagSettings();
			/// <summary>
			/// Gameobject of the parts
			/// </summary>
			[SerializeField]
			private GameObject goHead, goLarm, goRarm, goCar; 
			
			/// <summary>
			/// Classes of the parts
			/// </summary>
			[SerializeField]
			private Part[] mParts = new Part[4];  

			public GameObject GetPartObj(int index){
				switch(index){
					case 0:
						return this.goHead;
					case 1:
						return this.goLarm;
					case 2:
						return this.goRarm;
					case 3:
						return this.goCar;
					default:
						return this.goHead;
				}				
			}
			
			public Part GetPart(int index){
				return this.mParts[index];
			}
		
			/// <summary>
			/// Change the part of the robot.
			/// </summary>
			/// <param name="part">Part.</param>
			/// <param name="robotName">Robot name.</param>
			/// <param name="newObj">New object.</param>
			/// <param name="callBack">Call back.</param>
			public void SetRobot(PART part, string robotName, GameObject newObj, AssignValues callBack){
				GameObject holder = null;
				switch (part) {
					case PART.HEAD:
						if (newObj.name != goHead.name) {
							Transform parent = goHead.transform.parent;
							holder = (GameObject)Instantiate (newObj, goHead.transform.position, goHead.transform.rotation);
							holder.name = newObj.name;
							holder.tag = this.mTags.mHeadTag;
							//							holder.AddComponent<Head>();
							//							mParts [0] = holder.GetComponent<Head> ();
							holder.transform.parent = parent;
							Destroy (goHead);
							goHead = holder;

							//					goLarm.transform.localPosition = GameObject.Find("larm_spawn").transform.localPosition;
							//					goRarm.transform.localPosition = GameObject.Find("rarm_spawn").transform.localPosition;
						}
						break;
					case PART.LARM:
						if (newObj.name != goHead.name) {
							Transform parent = goLarm.transform.parent;
							holder = (GameObject)Instantiate (newObj, goLarm.transform.position, goLarm.transform.rotation);
							//holder.transform.localPosition = GameObject.Find("larm_spawn").transform.localPosition;
							holder.name = newObj.name;
							holder.tag = this.mTags.mLarmTag;
							//							holder.AddComponent<Larm>();
							//							mParts [1] = holder.GetComponent<Larm> ();
							holder.transform.parent = parent;
							Destroy (goLarm);
							goLarm = holder;
						}
						break;
					case PART.RARM:
						if (newObj.name != goHead.name) {
							Transform parent = goRarm.transform.parent;
							holder = (GameObject)Instantiate (newObj, goRarm.transform.position, goRarm.transform.rotation);
							holder.name = newObj.name;
							holder.tag = this.mTags.mRamTag;
							//							holder.AddComponent<Rarm>();
							//							mParts [2] = holder.GetComponent<Rarm> ();
							holder.transform.parent = parent;
							Destroy (goRarm);
							goRarm = holder;
						}
						break;
					case PART.CAR:
						if (newObj.name != goHead.name) {
							Transform parent = goCar.transform.parent;
							holder = (GameObject)Instantiate (newObj, goCar.transform.position, goCar.transform.rotation);
							holder.name = newObj.name;
							holder.tag = this.mTags.mCarTag;
							//							holder.AddComponent<Car>();
							//							mParts [3] = holder.GetComponent<Car> ();
							holder.transform.parent = parent;
							Destroy (goCar);
							goCar = holder;
						}
						break;
				}

				//				callBack (robotName);
			}
			
			void Awake () {
				DontDestroyOnLoad(this.gameObject);
				foreach( Transform child in this.transform){
					if (child.gameObject.tag == this.mTags.mCarTag) {
						this.goCar = child.gameObject;
					}
					if(child.childCount > 0) {
						foreach( Transform nodeChild in child){
							if (nodeChild.gameObject.tag == this.mTags.mHeadTag) {
								this.goHead = nodeChild.gameObject;
							}
							foreach (Transform innerChild in nodeChild) {
								if (innerChild.gameObject.tag == this.mTags.mLarmTag) {
									this.goLarm = innerChild.gameObject;
								}else if(innerChild.gameObject.tag == this.mTags.mRamTag){
									this.goRarm = innerChild.gameObject;
								}
							}
						}
					}
				}
			}
			
			// Use this for initialization
			void Start () {
			
			}
			
			// Update is called once per frame
			void Update () {
			
			}
		}
	}
}