using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CoverBase {
	public bool mOccupied;
	public Transform mPositionObject;
}

public class CoverPosition : MonoBehaviour {

	public List<CoverBase>mFrontPositions = new List<CoverBase>();
	public List<CoverBase>mBackPositions = new List<CoverBase>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
