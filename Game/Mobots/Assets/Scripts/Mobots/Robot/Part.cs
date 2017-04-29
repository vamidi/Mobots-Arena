using System.Collections;
using UnityEngine;

using Boomlagoon.JSON;

namespace Mobots.Robot
{
	/// <summary>
	/// The parts of the robot
	/// </summary>
	public enum PartType {
		Head, Car, Larm, Rarm, Unassigned
	}


	[System.Serializable]
	public abstract class Part : MonoBehaviour {
		public string mRobotName = "";
		public Material mFlashMaterial;
		/// <summary>
		///
		/// </summary>
		protected Player mRobot;
		/// <summary>
		/// The Tags
		/// </summary>
		public TagSettings mTags = new TagSettings();
		[SerializeField]
		protected PartType mPartType = PartType.Unassigned;
		protected float mHitPoints = 100f;
		protected float mMaxHitPoints = 100f;
		protected int mWeight;
		protected bool mLocked = true, isFlashing;
		protected Material mMaterial;
		protected HealthBar mHealthBar;
		protected Texture2D mThumbnail;
		protected JSONObject mJsonObject;

		/// <summary>
		/// Sets the health.
		/// </summary>
		/// <param name="health">Health.</param>
		public void SetHealth(float health) {
			mHitPoints = mMaxHitPoints = health;
		}

		/// <summary>
		/// Sets the weight.
		/// </summary>
		/// <param name="weight">Weight.</param>
		public void SetWeight(int weight) {
			mWeight = weight;
		}

		public float HitPoints {
			get { return mHitPoints; }
			set { mHitPoints = value; }
		}

		public int Weight {
			get { return mWeight; }
			set { mWeight = value; }
		}

		public PartType PartType {
			get {
				if (mPartType == PartType.Unassigned)
					CheckPartType();
				return mPartType;
			}
			set { mPartType = value; }
		}

		public void LoadStatsByRobotName(string robotName = "") {
			mJsonObject = JSONObject.Parse(GameUtilities.ReadTextAsset("Robots/" + robotName + "/" + robotName.ToLower() + "_json"));
			SetStats();
		}

		public void SetStats() {
			if (mPartType == PartType.Unassigned)
				return;

			JSONObject stats;

			switch (mPartType) {
				case PartType.Head:
					stats = mJsonObject.GetObject("parts").GetObject("head").GetObject("stats");
					gameObject.SendMessage("SetHealth", (float)stats.GetNumber("hitpoints"));
					gameObject.SendMessage("SetArmor", (float)stats.GetNumber("shieldhitpoints"));
					gameObject.SendMessage("SetStrength", (float)stats.GetNumber("shieldstrength"));
					gameObject.SendMessage("SetWeight", (int)stats.GetNumber("weight"));
					break;
				case PartType.Larm:
					stats = mJsonObject.GetObject("parts").GetObject("left").GetObject("stats");
					gameObject.SendMessage("SetHealth", (float)stats.GetNumber("hitpoints"));
					gameObject.SendMessage("SetWeight", (int)stats.GetNumber ("weight"));
					gameObject.SendMessage("SetDamagePerRound", (float)stats.GetNumber ("damageperround"));
					gameObject.SendMessage("SetRoundsPerSecond", (float)stats.GetNumber ("roundspersecond"));
					gameObject.SendMessage("SetAccuracy", (float)stats.GetNumber ("accuracy"));
					break;
				case PartType.Rarm:
					stats = mJsonObject.GetObject("parts").GetObject("right").GetObject("stats");
					mRobot.SetValue (mPartType, "SetHealth", (float)stats.GetNumber ("hitpoints"));
					mRobot.SetValue (mPartType, "SetWeight", (int)stats.GetNumber ("weight"));
					mRobot.SetValue (mPartType, "SetDamagePerRound", (float)stats.GetNumber ("damageperround"));
					mRobot.SetValue (mPartType, "SetRoundsPerSecond", (float)stats.GetNumber ("roundspersecond"));
					mRobot.SetValue (mPartType, "SetAccuracy", (float)stats.GetNumber("accuracy"));
					break;
				case PartType.Car:
					stats = mJsonObject.GetObject("parts").GetObject("car").GetObject("stats");
					mRobot.SetValue (mPartType, "SetHealth", (float)stats.GetNumber ("hitpoints"));
					mRobot.SetValue (mPartType, "SetWeight", (int)stats.GetNumber ("weight"));
					mRobot.SetValue (mPartType, "SetSpeed", (float)stats.GetNumber ("speed"));
					mRobot.SetValue (mPartType, "SetJumpStrength", (float)stats.GetNumber ("jumpstrength"));
					break;
			}

			SetName();
		}

		protected void CheckPartType() {
			if (mPartType != PartType.Unassigned)
				return;

			if (gameObject.CompareTag(mTags.mHeadTag)) {
				mPartType = PartType.Head;
			} else if (gameObject.CompareTag(mTags.mCarTag)) {
				mPartType = PartType.Car;
			}else if (gameObject.CompareTag(mTags.mLarmTag)) {
				mPartType = PartType.Larm;
			} else {
				mPartType = PartType.Rarm;
			}
		}

		protected void SearchParent() {
			if (mRobot)
				return;
			this.mRobot = this.transform.root.GetComponent<Mobots.Robot.Player>();
		}

		protected void SetName() {
			mRobotName = mJsonObject.GetString("robotname");
		}

		protected virtual void Awake() {
			CheckPartType();
			SearchParent();
		}

		protected virtual void Start() {
			// Test case robot
			if(mRobot != null && mRobot.mDebug)
				LoadStatsByRobotName("Samurai");

			mMaterial = GetComponent<Renderer>().material;
			mFlashMaterial = new Material(Shader.Find("Mobile/Particles/Additive"));
		}

		protected virtual void Update() { }

		protected IEnumerator Flash() {
			isFlashing = true;
			yield return new  WaitForSeconds(0.1F);
			gameObject.GetComponent<Renderer>().material = mFlashMaterial;
			yield return new  WaitForSeconds(0.1F);
			gameObject.GetComponent<Renderer>().material = mMaterial;
			yield return new  WaitForSeconds(0.1F);
			gameObject.GetComponent<Renderer>().material = mFlashMaterial;
			yield return new  WaitForSeconds(0.1F);
			gameObject.GetComponent<Renderer>().material = mMaterial;
			yield return new  WaitForSeconds(0.1F);
			gameObject.GetComponent<Renderer>().material = mFlashMaterial;
			yield return new  WaitForSeconds(0.1F);
			gameObject.GetComponent<Renderer>().material = mMaterial;
			yield return new  WaitForSeconds(0.1F);
			isFlashing = false;
		}
	}
}