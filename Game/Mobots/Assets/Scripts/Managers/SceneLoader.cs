using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour {
	
	public static SceneLoader instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

	public string mSceneName = "";

	public void LoadNewLevel(string levelname){
		this.mSceneName = levelname;
		SceneManager.LoadScene("loading_scene");
	}
	
	public void StartLevelAsync(string scenename = "", Slider slider = null, UnityAction<Scene, Scene> callback = null){
		if(scenename != "")
			StartCoroutine(GameUtilities.LoadLevelAsync(scenename, slider));
		else
			StartCoroutine(GameUtilities.LoadLevelAsync(this.mSceneName, slider));
		
		if(callback != null)
			SceneManager.activeSceneChanged += callback;
	}
	
	//Awake is always called before any Start functions
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
	}

	//Update is called every frame.
	void Update() {
		
	}
}
