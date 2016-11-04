using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	/// <summary>
	/// The life time of the bullet
	/// </summary> 
	public float mLife = 2f;
	/// <summary>
	/// Speed of the bullet
	/// </summary>
	public float mSpeed = 60f;
	public float mDamage = 15f;
	/// <summary>
	/// The rigidbody of the bullet
	/// </summary>
	public Rigidbody mRigidbody;
	/// <summary>
	/// Which objects are breakable or 
	/// can leave bullet marks
	/// </summary>
	public LayerMask breakable;
		
	// Use this for initialization
	void Start () {
		this.mRigidbody = this.GetComponent<Rigidbody>();
		this.transform.rotation *= Quaternion.Euler(90, 0, 0); // Rotate because the object can come out wrong.
	}
	
	// Update is called once per frame
	void Update () {		
		Destroy(this.gameObject, mLife);		
		this.mRigidbody.velocity = this.transform.up * this.mSpeed;
	}
	
	void OnTriggerEnter(Collider col){
		if(col.tag == "Head" || col.tag == "Left" || col.tag == "Right" || col.tag == "Car" || col.tag == "Target"){
			col.SendMessage("Damage", this.mDamage, SendMessageOptions.DontRequireReceiver);
		}
					
		Destroy(this.gameObject);	
	}
}
