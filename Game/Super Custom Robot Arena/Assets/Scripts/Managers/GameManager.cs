using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

//
using Boomlagoon.JSON;
// own namespace
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
	public GameObject mRobotUI;
	public GameObject mPauseObj;
	public string robotname = "MKVII";
	public string enemyName = "", enemyHead, enemyLarm, enemyRarm, enemyCar;
	public GameObject enemy;
	public CallBack mCallback = null;
	public bool mInGame = false;
	public bool mCursorOn = false;
	
	public TagSettings mTagSettings = new TagSettings();
	
	private GameObject mPlayer;
	private GameObject enemyPrefab;
	private GameObject mPause;
	private List<Vector3> mSpawnpoints = new List<Vector3>();
	private GameObject[] mPoints;
	private float mTimeScale;
	
	public void Resume() {
		if(!this.mInGame) {
			this.mInGame = true;
			this.mCursorOn = true;
			if(this.mPause)
				Destroy(this.mPause);
			Time.timeScale = this.mTimeScale;
		}	
	}
	
	public void PauseGame() {
		this.mInGame = false;
		this.mCursorOn = false;
		this.mPause = (GameObject) Instantiate(this.mPauseObj, this.transform.position, Quaternion.identity);
		this.mPause.GetComponent<Canvas>().worldCamera = Camera.main;
		Time.timeScale = 0f;
	}
	
	public void Restart() {
		
	}
	
	public void CreateRobot(GameObject newEnemy){
		this.enemyPrefab = newEnemy;
	}
	
	public void StartGame(Vector3 startpoint) {
		if(this.mPlayer){
			this.mPlayer.SetActive(true);			
			this.mPlayer.transform.position = startpoint;
			this.mSpawnpoints.Remove(startpoint);
			Enemy e = null;
			if(this.enemy){
				this.enemy.SetActive(true);
				e = this.enemy.GetComponent<Enemy>();
				this.enemy.GetComponent<FieldOfView>().Initialize();
				if(e != null){
					e.InitializeWaypoints();
				}
				this.enemy.transform.position = this.mSpawnpoints[Random.Range(0, this.mSpawnpoints.Count)];
			}
			this.mSpawnpoints.Add(startpoint);
			foreach(GameObject p in this.mPoints){
				p.SetActive(false);
			}
			
			Destroy(this.mPlayer.GetComponent<SimpleRotation>());
			Destroy(this.mPlayer.GetComponent<RobotEditor>());
			Player holder = this.mPlayer.GetComponent<Player>();
			
			if(this.mRobotUI){
				Instantiate(this.mRobotUI, this.transform.position, Quaternion.identity);
				for(int i = 0; i < 4; i++){
					holder.GetPartObj(i).GetComponent<Part>().Initialize();
				}
			}
			
			Camera.main.GetComponent<CameraController>().Initialize(mPlayer.transform, holder);
			holder.isControllable = true;
			if(e != null)
				e.isControllable = true;
			
			this.mInGame = this.mCursorOn = true;
		}		
	}
	
	public void ChooseSpawnpoints(Scene oldScene, Scene newScene) {
		if(mLoader.mSceneName == newScene.name) {
			this.mPoints = GameObject.FindGameObjectsWithTag("Spawnpoints");
			foreach(GameObject p in this.mPoints){
				this.mSpawnpoints.Add(p.transform.position);
				Tooltip t = p.GetComponent<Tooltip>();
				t.StartOpen();
			}
		}
		
		if(this.mPlayer && this.enemy){
			this.mPlayer.SetActive(false);
			this.enemy.SetActive(false);
		}
	}
	
	void Awake() {
		this.mTimeScale = Time.timeScale;
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
		string robots = GameUtilities.ReadTextAsset ("Robots/robots");
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
		Cursor.lockState = (mCursorOn) ? CursorLockMode.Locked : CursorLockMode.None;
		
		if(this.mInGame && Input.GetButtonDown(this.mTagSettings.mCancel)){
			this.PauseGame();
		}
		
	}
}
