using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	
	public Material mSkyBox;
	public Camera cam;
	
	void Awake(){
		Vector3 v = GameObject.FindGameObjectWithTag("Robot").transform.position;
		v.y = 100f;
		GameObject.FindGameObjectWithTag("Robot").transform.position = v;
		GameObject.Find("Cylinder").GetComponent<Renderer>().enabled = false;
		if(!GameObject.FindObjectOfType<GameManager>().enemy)
			GameObject.FindObjectOfType<GameManager>().enemy = GameObject.FindGameObjectWithTag("Enemy");
		
		GameObject.FindObjectOfType<GameManager>().Initialize();
		
	}

	// Use this for initialization
	void Start () {
		this.cam = Camera.main;
		cam.gameObject.AddComponent<Skybox>();
		cam.gameObject.AddComponent<SimpleRotation>();
		cam.GetComponent<SimpleRotation>().mSpeed = 15f;
		cam.GetComponent<Skybox>().material = this.mSkyBox;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
