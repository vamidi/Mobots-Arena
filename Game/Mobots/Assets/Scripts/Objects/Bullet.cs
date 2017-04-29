using UnityEngine;
using System.Collections;
using Mobots.Robot;

public class Bullet : MonoBehaviour {

	public GameObject parent;
	public GameObject hitParticle;
	/// <summary>
	/// The life time of the bullet
	/// </summary> 
	public float mLife = 2f;
	/// <summary>
	/// Speed of the bullet
	/// </summary>
	public float mSpeed = 80f;
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
	private void Start () {
		this.mRigidbody = this.GetComponent<Rigidbody>();
//		this.transform.rotation *= Quaternion.Euler(90, 0, 0); // Rotate because the object can come out wrong.
	}
	
	// Update is called once per frame
	private void Update () {
		Destroy(gameObject, mLife);
		this.mRigidbody.velocity = transform.forward * mSpeed;
	}
	
	private void OnTriggerEnter(Collider col) {
		if(col.CompareTag("Bullet"))
			return;

		if (col.gameObject == parent)
			return;
		
		if(col.CompareTag("Head") || col.CompareTag("Left") || col.CompareTag("Right") || col.CompareTag("Car") || col.CompareTag("Target")) {
			if(this.hitParticle){
				GameObject particle = (GameObject) Instantiate(hitParticle, this.transform.position, Quaternion.identity);
				Destroy(particle, 1.2f);
			}
			col.SendMessage("Damage", mDamage, SendMessageOptions.DontRequireReceiver);
		}
					
		Destroy(this.gameObject);	
	}
}
