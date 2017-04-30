using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobots.UI {
	public class Crosshair : MonoBehaviour {
		public GameObject mCrosshairPrefab;
		public Transform mGunEnd;
		public float mRange = 50f;
		public LayerMask mLayer;
		// Toggle the prefab on and off
		public void ToggleCrosshair(bool active) {
			if (mCrosshairPrefab)
				mCrosshairPrefab.SetActive(active);
		}

		// Use this for initialization
		private void Start() {
			if (mCrosshairPrefab != null) {
				mCrosshairPrefab = Instantiate(mCrosshairPrefab);
			}
		}

		// Update is called once per frame
		private void Update() {
			PositionCrosshair();
		}

		// position the crosshai to where the player is aiming
		private void PositionCrosshair() {
			RaycastHit hit;
			Vector3 rayOrg = this.mGunEnd.position;
			if (Physics.Raycast(rayOrg, mGunEnd.transform.forward, out hit, this.mLayer)) {
				if (mCrosshairPrefab) {
					ToggleCrosshair(true);
					mCrosshairPrefab.transform.position = hit.point;
					mCrosshairPrefab.transform.LookAt(Camera.main.transform);
				}
			} else {
				ToggleCrosshair(false);
			}
		}
	}
}
