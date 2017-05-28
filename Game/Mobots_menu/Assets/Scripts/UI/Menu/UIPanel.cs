using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobots.UI {
	public class UIPanel : MonoBehaviour {
		public GameObject mGameObject;
		public Vector3 mTransform;

		public void Start() {
			mTransform = mGameObject.GetComponent<Transform>().eulerAngles;
		}
		
		public void OnEnterState() {
			if (mGameObject) {
				mGameObject?.SetActive(true);
			}
		}
		
		public void OnExitState() {
			if (mGameObject) {
				mGameObject.transform.eulerAngles = mTransform;
				mGameObject?.SetActive(false);
			}
		}
	}
}
