using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Mobots.UI {

	[RequireComponent(typeof(AudioSource))]
	public class DynamicListener : MonoBehaviour {

		public string mObjectListeningTag = "Enter Gameobject's tag";
		public bool thisIsListener = false;
		public bool mParameter = false;
		public string mSendMassage = "Enter GameObject's method name";
		public string mMessageParameter;

		protected Button b;
		protected GameObject mObjectListening;
		protected AudioSource mSource;

		// Use this for initialization
		void Start() {
			mSource = GetComponent<AudioSource>();
			if (!b)
				b = this.GetComponent<Button>();
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
					if (this.mSource) this.GetComponent<AudioSource>().Play();
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
