using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {

	// each robot gets an id
	public string mName = "Henk de Tank";
	public int mId { get; private set; }

	public GameObject goHead;
	public GameObject goLarm;
	public GameObject goRarm;
	public GameObject goCar;

	public Part[] mParts= new Part[4];

	// Use this for initialization
	void Start () {
		mParts [0] = goHead.GetComponent<Head> ();
		mParts [1] = goLarm.GetComponent<Larm> ();
		mParts [2] = goRarm.GetComponent<Rarm> ();
		mParts [3] = goCar.GetComponent<Car> ();

		if (mParts [0].getPart () != PART.HEAD)
			Debug.LogError ("The part is not a head part");

		if (mParts [1].getPart () != PART.LARM)
			Debug.LogError ("The part is not a left arm part");

		if (mParts [2].getPart () != PART.RARM)
			Debug.LogError ("The part is not a right arm part");

		if (mParts [3].getPart () != PART.CAR)
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
