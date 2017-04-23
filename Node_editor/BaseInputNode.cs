using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputNode : BaseNode {

	protected string mNodeResult = "None";
	
	public virtual string GetResult() {
		return this.mNodeResult;
	}
	
	public override void DrawCurves() {
		
	}
}
