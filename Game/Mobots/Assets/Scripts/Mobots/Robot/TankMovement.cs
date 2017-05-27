using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobots.Robot {
	public class TankMovement : MonoBehaviour {
		public float mSpeed = 12f;                 // How fast the tank moves forward and back.
		public float mTurnSpeed = 180f;            // How fast the tank turns in degrees per second.
		public float mPitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

		private Rigidbody mRigidbody;              // Reference used to move the tank.
		private float mMovementInputValue;         // The current value of the movement input.
		private float mTurnInputValue;             // The current value of the turn input.

		readonly InputSettings mInput = new InputSettings();

		private void Awake () {
			mRigidbody = GetComponent<Rigidbody> ();
		}

		private void OnEnable() {
			mRigidbody.isKinematic = false;
			mMovementInputValue = 0f;
			mTurnInputValue = 0f;
		}

		private void OnDisable() {
			mRigidbody.isKinematic = true;
		}

		// Use this for initialization
		private void Start() {
		}

		// Update is called once per frame
		private void Update() {
			GetInput();

		}

		private void FixedUpdate() {
			Move();
			Turn();
		}

		private void GetInput() {
			mMovementInputValue = Input.GetAxis(mInput.mVertical);
			mTurnInputValue = Input.GetAxis(mInput.mHorizontal);
		}

		private void Move() {
			Vector3 movement = transform.forward * mMovementInputValue * mSpeed * Time.deltaTime;
			mRigidbody.MovePosition(mRigidbody.position + movement);
		}

		private void Turn() {
			float angle = mTurnSpeed * mTurnInputValue * Time.deltaTime;
			Quaternion turnRotation = Quaternion.Euler(0f, angle, 0f);
			mRigidbody.MoveRotation(mRigidbody.rotation * turnRotation);
		}
	}
}