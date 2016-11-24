using UnityEngine;
using System.Collections;

public class ParticleEmit : MonoBehaviour {
	
	public float mLife = 3f;
	
	private ParticleSystem mParticleSystem;

	public void EmitParticleSystem(){
		this.mParticleSystem.Play();
	}
	
	// Use this for initialization
	private void Awake () {
		this.mParticleSystem = GetComponent<ParticleSystem>();
		if(!this.mParticleSystem)
			Debug.LogError("There is no particle attached to this script");
		
		Vector3 direction = GameObject.FindGameObjectWithTag("Robot").transform.position - this.transform.position;
		this.transform.LookAt(direction);
	}
	
	void Update(){
		Destroy(this.gameObject, this.mLife);
	}
}
