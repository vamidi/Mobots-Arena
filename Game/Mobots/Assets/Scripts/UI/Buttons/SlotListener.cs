using UnityEngine;
using System.Collections;
using MBA.UI;

namespace Mobots.UI {

	public class SlotListener : DynamicListener {

		protected override void SetListener() {
			if (b) {
				if (mParameter) {
					mObjectListening.GetComponent<MBAEditor>().mStartImmidiatly = true;
					b.onClick.AddListener(() => mObjectListening.GetComponent<MBAEditor>().SaveToSlot(this.mMessageParameter));
				}
			} else {
				Debug.LogError("Dynamics listeners belongs to this button");
			}
		}
	}
}

