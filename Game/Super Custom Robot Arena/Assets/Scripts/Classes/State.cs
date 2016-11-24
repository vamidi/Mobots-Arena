using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[System.Serializable]
public enum CHOICES {
	FindArmor, FindHealth, SeekCover, RUNAWAY
}

[System.Serializable]
public enum PRIORITY {
	LOW = 1, MEDIUM, HIGH
}

public abstract class State<T> {
	
	protected PRIORITY mPriority = PRIORITY.LOW;
	protected DistanceComparer mDistanceComparer;
	protected List<Transform>mCoverPositions = new List<Transform>();
	protected List<Transform>mIgnorePositions = new List<Transform>();
	protected bool mFindCover = false;
	protected bool mInTheBack = false;
	protected int mTries = 3;
	protected int mCurrentTries;
	
	public PRIORITY GetPriority(){
		return this.mPriority;
	}
	
	// Use this for initialization
	public abstract void Start (T mEnemy);
	
	// Update is called once per frame
    public abstract void Update (T mEnemy);
	
	public abstract void FixedUpdate (T mEnemy);
	
	public abstract void LateUpdate (T mEnemy);

	// Exit is called once the state is exitted
	public abstract void Exit (T mEnemy);

	protected abstract void Move (T mEnemy);
	
	protected abstract void Turn (T mEnemy);
	
	protected void CoverBehaviour (Enemy mEnemy) {
		if(!mFindCover){
			this.FindCover(mEnemy);		
		}
	}
	
	protected void CheckCover (Enemy mEnemy) {
		if(mEnemy.mCurrentCoverBase != null){			
//			Debug.Log("Evading");
			Vector3 distanceOfTarget = mEnemy.mPlayer.position - mEnemy.mCurrentCoverBase.mPositionObject.position;
			Vector3 coverForward = mEnemy.mCurrentCoverBase.mPositionObject.transform.TransformDirection(Vector3.forward);
			float dot = Vector3.Dot(coverForward, distanceOfTarget);
			if(dot < 0f && this.mInTheBack == false) { // Going negative
				// Debug.Log(Vector3.Dot(coverForward, distanceOfTarget));	
				mEnemy.mCurrentCoverBase.mOccupied = false;
				this.mIgnorePositions.Clear();
				this.mCurrentTries = 0;
				this.mFindCover = false;

			}else if(dot > 0f && this.mInTheBack){
				// Debug.Log(Vector3.Dot(coverForward, distanceOfTarget));			
				mEnemy.mCurrentCoverBase.mOccupied = false;
				this.mIgnorePositions.Clear();
				this.mCurrentTries = 0;
				this.mFindCover = false;
			}	
		}		
	}
	
	private void FindCover(Enemy mEnemy){
		if(this.mCurrentTries <= this.mTries){
			if(!mFindCover){
				mFindCover = true;
				mCurrentTries++;
				CoverBase targetCoverPosition = null;
				float distanceToTarget = Vector3.Distance(mEnemy.transform.position, mEnemy.mPlayer.position);

				mCoverPositions.Clear();
				Vector3 targetPosition = mEnemy.mPlayer.position;
				Collider[] colliders = Physics.OverlapSphere(mEnemy.transform.position, mEnemy.GetFieldOfView().mViewRadius);

				foreach(Collider col in colliders){
					if(col.GetComponent<CoverPosition>()){
						if(!this.mIgnorePositions.Contains(col.transform)){
							float distanceToCover = Vector3.Distance(mEnemy.transform.position, col.transform.position);
//							Debug.Log("col: " + col.transform.localPosition);
//							Debug.Log("Distance to cover: " + distanceToCover + " Distance to target: " + distanceToTarget );
							if(distanceToCover < distanceToTarget){
								this.mCoverPositions.Add(col.transform);
							}
						}
					}
				}

				if(this.mCoverPositions.Count > 0){
					SortCoverPositions(mEnemy, this.mCoverPositions);

					CoverPosition validatePosition = this.mCoverPositions[0].GetComponent<CoverPosition>();
					Vector3 distanceOfTarget = targetPosition - validatePosition.transform.position;
					Vector3 coverForward = validatePosition.transform.TransformDirection(Vector3.forward);
					if(Vector3.Dot(coverForward, distanceOfTarget) < 0f){
						for(int i = 0; i < validatePosition.mBackPositions.Count; i++){
							if(!validatePosition.mBackPositions[i].mOccupied){
								this.mInTheBack = true;
								targetCoverPosition = validatePosition.mBackPositions[i];
							}
						}
					}else{
						for(int i = 0; i < validatePosition.mFrontPositions.Count; i++){
							if(!validatePosition.mFrontPositions[i].mOccupied){
								this.mInTheBack = false;
								targetCoverPosition = validatePosition.mFrontPositions[i];
							}
						}
					}
				}

				if(targetCoverPosition == null){
					this.mFindCover = false;
					if(this.mCoverPositions.Count > 0){
						this.mIgnorePositions.Add(this.mCoverPositions[0]);
					}
				}else{
					targetCoverPosition.mOccupied = true;
					mEnemy.mCurrentCoverBase  = targetCoverPosition;
//					Debug.Log(this.mInTheBack);
//					Debug.Log("parent position: " + mEnemy.mCurrentCoverBase.mPositionObject.parent.position + " current: " +
//						mEnemy.mCurrentCoverBase.mPositionObject.localPosition);
				}
			}
		}else {
			Debug.Log("Max tries exceeded. Changing behaviour");
			mEnemy.GetFSM().ChangeState(AttackState.Instance());
		}
	}

	private void SortCoverPositions(Enemy mEnemy,List<Transform> positions){
		this.mDistanceComparer = new DistanceComparer(mEnemy.transform);
		this.mCoverPositions.Sort(this.mDistanceComparer);
	}
}
