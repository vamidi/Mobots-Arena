using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {

	// each robot gets an id
	public string mName = "Red Boy";
	public int mId { get; private set; }

	public string mHeadTag = "";
	public string mLarmTag = "";
	public string mRamTag = "";
	public string mCarTag = "";

	[SerializeField]
	private GameObject goHead;
	private GameObject goLarm;
	private GameObject goRarm;
	private GameObject goCar;

	public Part[] mParts= new Part[4];

	public void SetRobot(PART part, string robotName, GameObject newObj, mAssignValues method){
		switch (part) {
		case PART.HEAD:
			if (newObj.name != goHead.name) {
				Transform parent = goHead.transform.parent;
				GameObject holder = (GameObject)Instantiate (newObj, goHead.transform.position, goHead.transform.rotation);
				holder.name = newObj.name;
				holder.AddComponent<Head>();
				mParts [0] = holder.GetComponent<Head> ();
				holder.transform.parent = parent;
				Destroy (goHead);
				goHead = holder;
			}
			break;
		case PART.LARM:
			if (newObj.name != goHead.name) {
				Transform parent = goLarm.transform.parent;
				GameObject holder = (GameObject)Instantiate (newObj, goLarm.transform.position, goLarm.transform.rotation);
				holder.name = newObj.name;
				holder.AddComponent<Larm>();
				mParts [1] = holder.GetComponent<Larm> ();
				holder.transform.parent = parent;
				Destroy (goLarm);
				goLarm = holder;
			}
			break;
		case PART.RARM:
			if (newObj.name != goHead.name) {
				Transform parent = goRarm.transform.parent;
				GameObject holder = (GameObject)Instantiate (newObj, goRarm.transform.position, goRarm.transform.rotation);
				holder.name = newObj.name;
				holder.AddComponent<Rarm>();
				mParts [2] = holder.GetComponent<Rarm> ();
				holder.transform.parent = parent;
				Destroy (goRarm);
				goRarm = holder;
			}
			break;
		case PART.CAR:
			if (newObj.name != goHead.name) {
				Transform parent = goCar.transform.parent;
				GameObject holder = (GameObject)Instantiate (newObj, goCar.transform.position, goCar.transform.rotation);
				holder.name = newObj.name;
				holder.AddComponent<Car>();
				mParts [3] = holder.GetComponent<Car> ();
				holder.transform.parent = parent;
				Destroy (goCar);
				goCar = holder;
			}
			break;
		}

		method (part, robotName);
	}

	public void SetValue(PART part, string method = "", object value = null) {
		if (method == "" || value == null)
			return;

		switch (part) {
		case PART.HEAD:
			mParts [0].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			break;
		case PART.LARM:
			mParts [1].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			break;
		case PART.RARM:
			mParts [2].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			break;
		case PART.CAR:
			mParts [3].SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			break;
		}
	}

	// Use this for initialization
	void Start () {
		foreach( Transform child in this.transform){
			if (child.gameObject.tag == mCarTag) {
				goCar = child.gameObject;
			}
			if(child.childCount > 0) {
				foreach( Transform nodeChild in child){
					if (nodeChild.gameObject.tag == mHeadTag) {
						goHead = nodeChild.gameObject;
					}
					foreach (Transform innerChild in nodeChild) {
						if (innerChild.gameObject.tag == mLarmTag) {
							goLarm = innerChild.gameObject;
						}else if(innerChild.gameObject.tag == mRamTag){
							goRarm = innerChild.gameObject;
						}
					}
				}
			}
		}

		mParts [0] = goHead.GetComponent<Head> ();
		mParts [1] = goLarm.GetComponent<Larm> ();
		mParts [2] = goRarm.GetComponent<Rarm> ();
		mParts [3] = goCar.GetComponent<Car> ();

		if (mParts [0].GetPart () != PART.HEAD)
			Debug.LogError ("The part is not a head part");

		if (mParts [1].GetPart () != PART.LARM)
			Debug.LogError ("The part is not a left arm part");

		if (mParts [2].GetPart () != PART.RARM)
			Debug.LogError ("The part is not a right arm part");

		if (mParts [3].GetPart () != PART.CAR)
			Debug.LogError ("The part is not a car part");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Movement of the robot
	/// </summary>
	private void Movement() {

	}
}
