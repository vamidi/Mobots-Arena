using UnityEngine;
using System.Collections;
using MBA.UI;

public class SlotListener : DynamicListener {
	
	protected override void SetListener () {
		if(this.b){
			if(this.mParameter){
				this.mObjectListening.GetComponent<MBAEditor>().mStartImmidiatly = true;
				b.onClick.AddListener(() => this.mObjectListening.GetComponent<MBAEditor>().SaveToSlot(this.mMessageParameter));
			}
		}else{
			Debug.LogError("Dynamics listeners belongs to this button");	
		}
	}
}

