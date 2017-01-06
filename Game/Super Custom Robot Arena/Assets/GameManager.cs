using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Boomlagoon.JSON;
using UnityEngine.SceneManagement;
using MBA.UI;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	
	/// <summary>
	/// The robot array.
	/// </summary>
	public JSONArray robots {
		get; private set;
	}
	
	/// <summary>
	/// The dictionary that contains the robots and their parts
	/// </summary>
	public Dictionary<string, JSONObject>robotDictionary {
		get; private set;
	}
	
	public SceneLoader mLoader;
	public string robotname = "MKVII";
	public string enemyName = "";
	public GameObject enemy;
	public CallBack mCallback = null;
	public bool mInGame = false;
	public bool mCursorOn = false;
	
	private GameObject mPlayer;
	private GameObject enemyPrefab;
	private List<Vector3> mSpawnpoints = new List<Vector3>();
	
	public void CreateRobot(GameObject newEnemy){
		this.enemyPrefab = newEnemy;
	}
	
	public void StartGame(Vector3 startpoint) {
		if(this.mPlayer){
			this.mPlayer.transform.position = startpoint;
			this.mSpawnpoints.Remove(startpoint);
			this.enemy.transform.position = this.mSpawnpoints[Random.Range(0, this.mSpawnpoints.Count)];
			this.mSpawnpoints.Add(startpoint);
			
			Destroy(this.mPlayer.GetComponent<SimpleRotation>());
			Destroy(this.mPlayer.GetComponent<RobotEditor>());
			Player holder = this.mPlayer.AddComponent<Player>();
			Camera.main.gameObject.AddComponent<CameraController>();
			holder.isControllable = true;
		}		
	}
	
	public void ChooseSpawnpoints(Scene oldScene, Scene newScene) {
		if(mLoader.mSceneName == newScene.name) {
			GameObject[] points = GameObject.FindGameObjectsWithTag("Spawnpoints");
			foreach(GameObject p in points){
				this.mSpawnpoints.Add(p.transform.position);
				Tooltip t = p.GetComponent<Tooltip>();
				t.StartOpen();
			}
		}
		
	}
	
	void Awake() {
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
		
		this.mLoader = GameObject.FindObjectOfType<SceneLoader>();
		string robots = GameUtilities.ReadResource ("Robots/robots");
		this.robotDictionary = new Dictionary<string, JSONObject>();
		this.robots = JSONObject.Parse(robots).GetArray("robots");
	
		foreach(JSONValue o in this.robots) {
			// Set the json of the robot into the dictionary.
			this.robotDictionary.Add(o.Obj.GetString("robotname"), o.Obj);
		}
		
		Cursor.lockState = (mCursorOn) ? CursorLockMode.Locked : CursorLockMode.None;
	}
	
	// Use this for initialization
	void Start () {
		this.mPlayer = GameObject.FindGameObjectWithTag("Robot");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
