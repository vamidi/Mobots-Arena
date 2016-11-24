using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DistanceComparer : IComparer<Transform> {
	
	private Transform mReferenceObject;
	
	public DistanceComparer (Transform reference) {
		this.mReferenceObject = reference;
	}
	
	public int Compare (Transform x, Transform y) {
		float distX = Vector3.Distance(x.position, this.mReferenceObject.position);
		float distY = Vector3.Distance(y.position, this.mReferenceObject.position);
		
		int retVal = 0;
		
		if(distX < distY){
			retVal = -1;
		}else if(distX > distY){
			retVal = 1;
		}
		
		return retVal;
	}
	
	
}
