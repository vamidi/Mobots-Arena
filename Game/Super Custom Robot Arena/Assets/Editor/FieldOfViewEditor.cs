using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof( FieldOfView ))]
public class FieldOfViewEditor : Editor {
	
	void OnSceneGUI(){
		FieldOfView fov = (FieldOfView)target;
		Handles.color = fov.mRadiusColor;
		Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.mViewRadius);
		Vector3 viewAngleA = fov.DirectionFromAngle(-fov.mViewAngle / 2, false); 
		Vector3 viewAngleB = fov.DirectionFromAngle(fov.mViewAngle / 2, false); 
		
		Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.mViewRadius);
		Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.mViewRadius);
		
		Handles.color = fov.mTargetColor;
		foreach(Transform t in fov.mVisibleTargets){
			Handles.DrawLine(fov.transform.position, t.position);
		}
	}
}
 