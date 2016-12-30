using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Boomlagoon.JSON;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {
	
	public GameObject mContent;
	public GameObject mLevelslotPrefab;	
	public float mSteps = 100f;
	
	private Dictionary<string, JSONObject>mLevels = new Dictionary<string,JSONObject>();
	
	public void SelectLevel(string levelname) {
		GameUtilities.LoadLevelAsync(this.mLevels[levelname].GetString("scene"));
	}

	void Awake() {
		Vector3 v = GameObject.FindGameObjectWithTag("Robot").transform.position;
		v.y = 100f;
		GameObject.FindGameObjectWithTag("Robot").transform.position = v;
		GameObject.Find("Cylinder").GetComponent<Renderer>().enabled = false;
	}

	// Use this for initialization
	void Start () {
		string levels = GameUtilities.ReadResource ("Arenas/levels");
		JSONArray arr = JSONObject.Parse(levels).GetArray("levels");
		
		foreach(JSONValue o in arr){
			this.mLevels.Add(o.Obj.GetObject("level").GetString("levelname"), o.Obj.GetObject("level"));
		}
		
		float rows = Mathf.Floor(arr.Length / 3);
		int columns = 3;
		List<Vector3> positions = new List<Vector3>();
		for(int row = 0; row <= rows; row++) {
			for(int column = 0; column < columns; column++) {
				// float r = column * rows + row;				
				Vector3 targetPos = new Vector3(145f, -90f, 0);
				targetPos.x = targetPos.x + (column * 300f) + (column * this.mSteps);
				targetPos.y = targetPos.y - (155f * row) - (row * this.mSteps);
				targetPos.z = 0;
				positions.Add(targetPos);

			}
		}		

		for(int i = 0; i < arr.Length; i++){
			JSONValue o = arr[i];
			GameObject b = Instantiate(this.mLevelslotPrefab as GameObject);
			if(this.mLevelslotPrefab){
				string name = o.Obj.GetObject("level").GetString("levelname");
				b.transform.SetParent(this.mContent.transform, false);		
				RectTransform rect = b.GetComponent<RectTransform>();
				b.GetComponent<DynamicListener>().mMessageParameter = name;
				b.GetComponentInChildren<Text>().text = name;
				if(i != 0)
					rect.anchoredPosition = positions[i];

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
