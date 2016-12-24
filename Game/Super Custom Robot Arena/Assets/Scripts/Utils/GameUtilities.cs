using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

// additional namespace 
using Boomlagoon.JSON;

// 

public static class GameUtilities {
	
	/// <summary>
	/// Loading text
	/// </summary>
	private static string loadProgress = "Loading...";
	/// <summary>
	/// The last loaded progress.
	/// </summary>
	private static string lastLoadProgress = null;

	/// <summary>
	/// Reads the file.
	/// </summary>
	/// <returns>The file.</returns>
	/// <param name="path">Path.</param>
	public static string ReadFile(string path) {
		//TextAss declared as public variable and drag dropped the text file in inspector
		TextAsset TxtAss = (TextAsset) Resources.Load(path, typeof(TextAsset));
		return (TxtAss != null) ? TxtAss.text : "";
	}
	
	public static bool CheckFileExists(string path, string fileName){
		path = Application.dataPath + "/Resources/" + path + fileName;
		return File.Exists(path);
	}
	
	public static bool DeleteFile(string path, string fileName){
		path = Application.dataPath + "/Resources/" + path + fileName;
		if(File.Exists(path)){
			File.Delete(path);
		}
		
		return File.Exists(path);
	}
	
	public static void WriteFile(string path, string fileName, string value){		
		path = Application.dataPath + "/Resources/" + path;
		File.WriteAllText(path + fileName, value);
	}
	
	/// <summary>
	/// Async loader for the level
	/// </summary>
	/// <param name="levelName">Level name</param>
	/// <returns>The routine.</returns>
	public static IEnumerator LoadLevelAsync(string levelName) {
		AsyncOperation op = SceneManager.LoadSceneAsync (levelName);
		op.allowSceneActivation = false;
		while (!op.isDone) {
			if (op.progress < 0.9f)
			{
				loadProgress = "Loading: " + (op.progress * 100f).ToString("F0") + "%";
			}
			else // if progress >= 0.9f the scene is loaded and is ready to activate.
			{
				if (Input.anyKeyDown)
				{
					op.allowSceneActivation = true;
				}
				loadProgress = "Loading ready for activation, Press any key to continue";
			}
			if (lastLoadProgress != loadProgress) { lastLoadProgress = loadProgress; Debug.Log(loadProgress); } // Don't spam console.
			yield return null;
		}
		loadProgress = "Load complete.";
		Debug.Log(loadProgress);
	}
}
