using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUtilities {

	public static string ReadFile(string path) {
		//TextAss declared as public variable and drag dropped the text file in inspector
		TextAsset TxtAss = (TextAsset) Resources.Load(path, typeof(TextAsset));
		return (TxtAss != null) ? TxtAss.text : "";
	}

	public static Dictionary<string, object>GetJSONData(JSONObject o){
		Dictionary<string, object> objects = new Dictionary<string, object> ();
		switch(o.type){
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < o.list.Count; i++){
				string key = (string)o.keys[i];
				JSONObject j = (JSONObject)o.list[i];
//				Debug.Log(key);
				GetData(objects, j, key);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach(JSONObject j in o.list){
				GetJSONData(j);
			}
			break;
		}

		return objects;
	}

	//access data (and print it)
	private static void GetData(Dictionary<string, object>data, JSONObject obj, string key){
		switch(obj.type){
		case JSONObject.Type.STRING:
//			Debug.Log (obj.str);
			data.Add (key, obj.str);
			break;
		case JSONObject.Type.NUMBER:
//			Debug.Log(obj.n);
			data.Add (key, obj.n);
			break;
		case JSONObject.Type.BOOL:
//			Debug.Log(obj.b);
			data.Add (key, obj.b);
			break;
		case JSONObject.Type.NULL:
//			Debug.Log("NULL");
			break;

		}
	}
}
