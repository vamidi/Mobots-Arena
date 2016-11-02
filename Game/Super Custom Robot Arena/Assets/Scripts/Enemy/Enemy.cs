using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	/// <summary>
	/// To see if the player is controllable
	/// </summary>
	public bool isControllable = true;
	/// <summary>
	/// The m physics.
	/// </summary>
	[SerializeField]
	public PhysicSettings mPhysics = new PhysicSettings();
	
	public TagSettings mTags = new TagSettings();
	/// <summary>
	/// Rigidbody of the robot
	/// </summary>
	[SerializeField]                    
	private Rigidbody mRigidbody;
	/// <summary>
	/// The m velocity.
	/// </summary>
	private Vector3 mVelocity = Vector3.zero;
	/// <summary>
	/// The mass of the robot
	/// </summary>
	[SerializeField]
	private int mMass;
	/// <summary>
	/// Gameobject of the parts
	/// </summary>
	[SerializeField]
	private GameObject goHead, goLarm, goRarm, goCar;                                   
	/// <summary>
	/// Classes of the parts
	/// </summary>
	[SerializeField]
	private Part[] mParts= new Part[4];   
	
	public Part GetPart(int index){
		return mParts[index];
	}
	
	/// <summary>
	/// See if the player is grounded
	/// </summary>
	bool Grounded(){
		return Physics.Raycast(this.transform.position, Vector3.down, mPhysics.mDistToGround, mPhysics.Ground);
	}
	
	void Awake(){
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
		this.mRigidbody = this.GetComponent<Rigidbody>();

		if (!mRigidbody)
			Debug.LogError("Character needs Rigidbody");

		this.mParts [0] = this.goHead.GetComponent<Head> ();
		this.mParts [1] = this.goLarm.GetComponent<Larm> ();
		this.mParts [2] = this.goRarm.GetComponent<Rarm> ();
		this.mParts [3] = this.goCar.GetComponent<Car> ();

		if (this.mParts [0].GetPart () != PART.HEAD)
			Debug.LogError ("The part is not a head part");

		if (this.mParts [1].GetPart () != PART.LARM)
			Debug.LogError ("The part is not a left arm part");

		if (this.mParts [2].GetPart () != PART.RARM)
			Debug.LogError ("The part is not a right arm part");

		if (this.mParts [3].GetPart () != PART.CAR)
			Debug.LogError ("The part is not a car part");

		this.mMass =  this.mParts [0].mRobotWegith + this.mParts [1].mRobotWegith + this.mParts [2].mRobotWegith + this.mParts [3].mRobotWegith;
		((Car)this.mParts[3]).SetSpeed(1825);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// FixedUpdate is called 
	void FixedUpdate() {
		if(this.isControllable){
			this.Move();
			this.Jump();

			this.mRigidbody.velocity = this.transform.TransformDirection(this.mVelocity);
		}
	}
	
	void Move(){
		
	}
	
	void Jump(){
		
	}
}
