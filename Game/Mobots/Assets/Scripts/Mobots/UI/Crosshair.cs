using System.Collections;
using System.Collections.Generic;
using Mobots.Robot;
using UnityEngine;

namespace Mobots.UI {
	public class Crosshair : MonoBehaviour {
		public GameObject mCrosshairPrefab;
		public Transform mCenterpoint;
		public float mRange = 50f, mMinimalRange = 15f, zMovement = -0.4f;
		public LayerMask mLayer;

		private Robot.Larm mLarm;

		// Toggle the prefab on and off
		public void ToggleCrosshair(bool active) {
			if (mCrosshairPrefab)
				mCrosshairPrefab.SetActive(active);
		}

		// Use this for initialization
		private void Start() {
			mLarm = (Robot.Larm) transform.root.GetComponent<Robot.Robot>().GetPartByType(PartType.Larm);
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
			var rayOrg = mCenterpoint.position;
			if (mLarm) {
				mCenterpoint.rotation = mLarm.transform.rotation;
			}

			if (Physics.Raycast(rayOrg, mCenterpoint.transform.forward, out hit, mLayer)) {
				if (mCrosshairPrefab) {
					ToggleCrosshair(true);
					mCrosshairPrefab.transform.position = hit.point;
					mCrosshairPrefab.transform.LookAt(Camera.main.transform);
				}
			} else {
				ToggleCrosshair(false);
			}

			if (Vector3.Distance(mCenterpoint.transform.position, mCrosshairPrefab.transform.position) < mMinimalRange) {
				mCenterpoint.transform.localPosition = new Vector3(zMovement, mCenterpoint.transform.localPosition.y, mCenterpoint.transform.localPosition.z);
			}
		}
	}
}
