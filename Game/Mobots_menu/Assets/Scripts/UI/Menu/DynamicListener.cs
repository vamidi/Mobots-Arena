using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Mobots.UI {

	public class DynamicListener : MonoBehaviour {

		public string mObjectListeningTag = "Enter Gameobject's tag";
		public bool thisIsListener = false;
		public bool mParameter = false;
		public string mSendMassage = "Enter GameObject's method name";
		public string mMessageParameter;
		public Animator mAnimator;

		protected Button b;
		protected GameObject mObjectListening;

		// Use this for initialization
		void Start() {
			if (!b)
				b = this.GetComponent<Button>();
			mAnimator = GetComponent<Animator>();
			GetObjectListening();
		}

		// Update is called once per frame
		void Update() {
			if (!mObjectListening)
				GetObjectListening();
		}

		void GetObjectListening() {
			if (thisIsListener)
				mObjectListening = this.gameObject;
			else
				this.mObjectListening = GameObject.FindGameObjectWithTag(this.mObjectListeningTag);

			if (mObjectListening) {
				SetListener();
				b.onClick.AddListener(() =>
				{
					b.enabled = false;
					b.enabled = true;
					mAnimator.SetTrigger("Normal");
				});
			}
		}

		protected virtual void SetListener() {
			if (b) {
				if (!mParameter)
					b.onClick.AddListener(() => mObjectListening.SendMessage(this.mSendMassage));
				else
					b.onClick.AddListener(() => mObjectListening.SendMessage(this.mSendMassage, this.mMessageParameter));
			} else {
				Debug.LogError("Dynamics listeners belongs to this button");
			}
		}
	}
}
