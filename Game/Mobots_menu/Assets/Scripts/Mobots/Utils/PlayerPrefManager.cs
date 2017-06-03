using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobots.managers {
	public enum PrefTypes {
		Float = 0,
		Int,
		String
	}
	public class PlayerPrefManager : MonoBehaviour {
		
		// Use this for initialization
		void Start() { }

		// Update is called once per frame
		void Update() { }

		public static object GetValue(string key, PrefTypes type) {
			if (PlayerPrefs.HasKey(key)) {
				switch (type) {
					case PrefTypes.Float:
						return PlayerPrefs.GetFloat(key);
					case PrefTypes.Int:
						return PlayerPrefs.GetInt(key);
					case PrefTypes.String:
						return PlayerPrefs.GetString(key);
				}
			}
			
			// If we have nothing
			return null;
		}

		public static void SetValue(string key, object value, PrefTypes type) {
			switch (type) {
				case PrefTypes.Float:
					var temp = (float) value;
					PlayerPrefs.SetFloat(key, temp);
					break;
				case PrefTypes.Int:
					var t = (int) value;
					PlayerPrefs.SetInt(key, t);
					break;
				case PrefTypes.String:
					var s = (string) value;
					PlayerPrefs.SetString(key, s);
					break;
			}
		}
	}
}