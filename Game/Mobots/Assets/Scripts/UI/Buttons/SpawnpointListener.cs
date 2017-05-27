using UnityEngine;
using System.Collections;

namespace Mobots.UI {

	public class SpawnpointListener : DynamicListener {

		public Transform mSpawnpointParameter;

		protected override void SetListener() {
			if (this.b) {
				if (!this.mSpawnpointParameter)
					b.onClick.AddListener(() => this.mObjectListening.SendMessage(this.mSendMassage));
				else
					b.onClick.AddListener(
						() => this.mObjectListening.SendMessage(this.mSendMassage, this.mSpawnpointParameter.position));
			} else {
				Debug.LogError("Dynamics listeners belongs to this button");
			}
		}
	}
}