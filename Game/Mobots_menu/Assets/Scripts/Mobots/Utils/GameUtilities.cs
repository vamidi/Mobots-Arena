﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;

// additional namespace
using Boomlagoon.JSON;

//
using System.IO.IsolatedStorage;

public delegate void CallBack(Scene previousScene, Scene newScene);

namespace Utils {
	public delegate void JSONCallback(JSONObject json);

	public static class GameUtilities {
		public static UnityAction<Scene, Scene> sceneLoaded;

		/// <summary>
		/// Loading text
		/// </summary>
		private static string loadProgress = "Loading...";

		/// <summary>
		/// The last loaded progress.
		/// </summary>
		private static string lastLoadProgress = null;

		public static UnityEngine.Object ReadResourceFile(string Path) {
			return Resources.Load(Path);
		}

		public static string ReadTextAsset(string Path) {
			TextAsset txtAss = (TextAsset) Resources.Load(Path, typeof(TextAsset));
			return (txtAss != null) ? txtAss.text : "";
		}

		/// <summary>
		/// Reads the file.
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="path">Path.</param>
		public static string ReadFile(string path, string fileName) {
			try {
				//TextAss declared as public variable and drag dropped the text file in inspector
				StreamReader sr;
#if UNITY_EDITOR
				sr = new StreamReader(Application.dataPath + "/Resources/" + path + fileName);
#else
			sr = new StreamReader(Application.persistentDataPath + "/Resources/" + path + fileName);
			#endif
				string fileContents = sr.ReadToEnd();
				sr.Close();
				return fileContents;
			} catch (FileNotFoundException ex) {
				Debug.Log(ex.Message);
				return "";
			}
		}

		public static bool CheckFileExists(string path, string fileName) {
#if UNITY_EDITOR
			path = Application.dataPath + "/Resources/" + path + fileName;
#else
		path = Application.persistentDataPath + "/Resources/" + path + fileName;
		#endif
			return File.Exists(path);
		}

		public static bool DeleteFile(string path, string fileName) {
#if UNITY_EDITOR
			path = Application.dataPath + "/Resources/" + path + fileName;
#else
		path = Application.persistentDataPath + "/Resources/" + path + fileName;
		#endif
			if (File.Exists(path)) {
				File.Delete(path);
			}

			return File.Exists(path);
		}

		// Runtime code here
		public static void WriteFile(string path, string fileName, string value) {
#if UNITY_EDITOR
			WriteEditor(path, fileName, value);
#else
		WriteStandalone(path, fileName, value);
		#endif
		}

		private static void WriteEditor(string path, string fileName, string value) {
			path = Application.dataPath + "/Resources/" + path;
			try {
				File.WriteAllText(path + fileName, value);
			} catch (Exception ex) {
				Debug.Log(ex.Message);
			}
		}

		private static void WriteStandalone(string path, string fileName, string value) {
			string checkPath = Application.persistentDataPath + "/Resources";
			try {
				if (!Directory.Exists(checkPath)) {
					Directory.CreateDirectory(checkPath);
				}

				path = Application.persistentDataPath + "/Resources/" + path;
				if (!Directory.Exists(path)) {
					Directory.CreateDirectory(path);
				} else {
					File.WriteAllText(path + fileName, value);
				}
			} catch (IsolatedStorageException ex) {
				Debug.Log(ex.Message);
			}
		}


		/// <summary>
		/// Async loader for the level
		/// </summary>
		/// <param name="levelName">Level name</param>
		/// <returns>The routine.</returns>
		public static IEnumerator LoadLevelAsync(string levelName, Slider slider) {
			AsyncOperation op = SceneManager.LoadSceneAsync(levelName);
			op.allowSceneActivation = false;
			while (!op.isDone) {
				if (op.progress < 0.9f) {
					slider.value = op.progress;
					loadProgress = "Loading: " + (op.progress * 100f).ToString("F0") + "%";
				} else // if progress >= 0.9f the scene is loaded and is ready to activate.
				{
					slider.value = 1f;
					loadProgress = "Loading ready for activation, Press any key to continue";
					op.allowSceneActivation = true;
				}
				if (lastLoadProgress != loadProgress) {
					lastLoadProgress = loadProgress; /* Debug.Log(loadProgress); */
				} // Don't spam console.
				yield return null;
			}
//		loadProgress = "Load complete.";
//		Debug.Log(loadProgress);
		}

		//Breadth-first search
		public static Transform FindDeepChild(this Transform aParent, string aName) {
			var result = aParent.Find(aName);
			if (result != null)
				return result;
			foreach (Transform child in aParent) {
				result = child.FindDeepChild(aName);
				if (result != null)
					return result;
			}
			return null;
		}


		public static Sprite GetImageSprite(string path) {
			Texture2D t2d = null;
			Sprite holder = null;

			t2d = Resources.Load<Texture2D>(path);

			if (t2d)
				holder = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));

			return holder;
		}
	}
}

public static class GameUtilities {
	public static UnityAction<Scene, Scene> sceneLoaded;

	/// <summary>
	/// Loading text
	/// </summary>
	private static string loadProgress = "Loading...";

	/// <summary>
	/// The last loaded progress.
	/// </summary>
	private static string lastLoadProgress = null;

	public static UnityEngine.Object ReadResourceFile(string Path) {
		return Resources.Load(Path);
	}

	public static string ReadTextAsset(string path) {
		TextAsset txtAss = (TextAsset) Resources.Load(path, typeof(TextAsset));
		return (txtAss != null) ? txtAss.text : "";
	}

	/// <summary>
	/// Reads the file.
	/// </summary>
	/// <returns>The file.</returns>
	/// <param name="path">Path.</param>
	public static string ReadFile(string path, string fileName) {
		try {
			//TextAss declared as public variable and drag dropped the text file in inspector
			StreamReader sr;
#if UNITY_EDITOR
			sr = new StreamReader(Application.dataPath + "/Resources/" + path + fileName);
#else
			sr = new StreamReader(Application.persistentDataPath + "/Resources/" + path + fileName);
			#endif
			string fileContents = sr.ReadToEnd();
			sr.Close();
			return fileContents;
		} catch (FileNotFoundException ex) {
			Debug.Log(ex.Message);
			return "";
		}
	}

	public static bool CheckFileExists(string path, string fileName) {
#if UNITY_EDITOR
		path = Application.dataPath + "/Resources/" + path + fileName;
#else
		path = Application.persistentDataPath + "/Resources/" + path + fileName;
		#endif
		return File.Exists(path);
	}

	public static bool DeleteFile(string path, string fileName) {
#if UNITY_EDITOR
		path = Application.dataPath + "/Resources/" + path + fileName;
#else
		path = Application.persistentDataPath + "/Resources/" + path + fileName;
		#endif
		if (File.Exists(path)) {
			File.Delete(path);
		}

		return File.Exists(path);
	}

	// Runtime code here
	public static void WriteFile(string path, string fileName, string value) {
#if UNITY_EDITOR
		WriteEditor(path, fileName, value);
#else
		WriteStandalone(path, fileName, value);
		#endif
	}

	private static void WriteEditor(string path, string fileName, string value) {
		path = Application.dataPath + "/Resources/" + path;
		try {
			File.WriteAllText(path + fileName, value);
		} catch (Exception ex) {
			Debug.Log(ex.Message);
		}
	}

	private static void WriteStandalone(string path, string fileName, string value) {
		string checkPath = Application.persistentDataPath + "/Resources";
		try {
			if (!Directory.Exists(checkPath)) {
				Directory.CreateDirectory(checkPath);
			}

			path = Application.persistentDataPath + "/Resources/" + path;
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			} else {
				File.WriteAllText(path + fileName, value);
			}
		} catch (IsolatedStorageException ex) {
			Debug.Log(ex.Message);
		}
	}


	/// <summary>
	/// Async loader for the level
	/// </summary>
	/// <param name="levelName">Level name</param>
	/// <returns>The routine.</returns>
	public static IEnumerator LoadLevelAsync(string levelName, Slider slider) {
		AsyncOperation op = SceneManager.LoadSceneAsync(levelName);
		op.allowSceneActivation = false;
		while (!op.isDone) {
			if (op.progress < 0.9f) {
				slider.value = op.progress;
				loadProgress = "Loading: " + (op.progress * 100f).ToString("F0") + "%";
			} else // if progress >= 0.9f the scene is loaded and is ready to activate.
			{
				slider.value = 1f;
				loadProgress = "Loading ready for activation, Press any key to continue";
				op.allowSceneActivation = true;
			}
			if (lastLoadProgress != loadProgress) {
				lastLoadProgress = loadProgress; /* Debug.Log(loadProgress); */
			} // Don't spam console.
			yield return null;
		}
//		loadProgress = "Load complete.";
//		Debug.Log(loadProgress);
	}

	//Breadth-first search
	public static Transform FindDeepChild(this Transform aParent, string aName) {
		var result = aParent.Find(aName);
		if (result != null)
			return result;
		foreach (Transform child in aParent) {
			result = child.FindDeepChild(aName);
			if (result != null)
				return result;
		}
		return null;
	}


	public static Sprite GetImageSprite(string path) {
		Texture2D t2d = null;
		Sprite holder = null;

		t2d = Resources.Load<Texture2D>(path);

		if (t2d)
			holder = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));

		return holder;
	}
}