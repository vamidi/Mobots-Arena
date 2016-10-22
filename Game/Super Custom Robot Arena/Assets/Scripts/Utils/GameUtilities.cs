using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// additional namespace 
using Boomlagoon.JSON;

public class GameUtilities {
	public static string ReadFile(string path) {
	//TextAss declared as public variable and drag dropped the text file in inspector
		TextAsset TxtAss = (TextAsset) Resources.Load(path, typeof(TextAsset));
		return (TxtAss != null) ? TxtAss.text : "";
	}

	public static Dictionary<string, JSONValue>GetJSONValues(string txt){
		Dictionary<string, JSONValue>objects = new Dictionary<string, JSONValue>();
		JSONObject json = JSONObject.Parse(txt);
		foreach (KeyValuePair<string, JSONValue> pair in json) {
			objects.Add(pair.Key, pair.Value);
		 	Debug.Log(pair.Key);
			Debug.Log(pair.Value);
		}

		return objects;
	}

	public static void GetJSONData(JSONObject obj){
		var nestedObject = obj.GetObject("nested");
		double number = nestedObject.GetNumber("key");
		Debug.Log(nestedObject);
		Debug.Log(number);
	}
}
