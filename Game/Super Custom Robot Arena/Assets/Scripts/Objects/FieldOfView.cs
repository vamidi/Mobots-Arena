using UnityEngine;
// Runtime code here
#if UNITY_EDITOR
// Editor specific code here
using UnityEditor;
#endif
// Runtime code here
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {
	
	public Color mRadiusColor = Color.black, mTargetColor = Color.red;
	public Turret mTurret;
	public float mViewRadius;
	[Range(0, 360)]
	public float mViewAngle;
	public LayerMask mTargetMask, mObstaclesMask, mPowerUpsMask, mPlayerMask;
	public List<Transform> mVisibleTargets = new List<Transform>(); 
	public Transform player;
	public float mMeshResolution;
	public MeshFilter mMeshFilter;
	public int mEdgeResolveIteration;
	public float mEdgeDstTreshold;
	
	private Mesh mViewMesh;
	
	public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal){
		if(!angleIsGlobal){
			angleInDegrees += transform.eulerAngles.y; 	
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public struct ViewCastInfo {
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;
		
		public ViewCastInfo(bool hit, Vector3 point, float dst, float angle){
			this.hit = hit;
			this.point = point;
			this.dst = dst;
			this.angle = angle;
		}
	}
	
	public struct EdgeInfo {
		public Vector3 pointA;
		public Vector3 pointB;
		
		public EdgeInfo(Vector3 a, Vector3 b){
			this.pointA = a;
			this.pointB = b; 
		}
	}
	
	// Use this for initialization
	void Start () { 
		this.mViewMesh = new Mesh();
		this.mViewMesh.name = "View Mesh";
		this.mMeshFilter.mesh = this.mViewMesh;
		StartCoroutine(this.FindTargetsWithDelay(.2f));
	}
	
	// Update is called once per frame
	void Update () { }
	
	void LateUpdate(){
		DrawFieldOfView(); 
	}
	
	private IEnumerator FindTargetsWithDelay( float delay ){
		while(true){
			yield return new WaitForSeconds(delay);
			this.FindVisibleTargets();
			this.FindPlayerTarget();
		}
		
	}
	
	private ViewCastInfo ViewCast(float globalAngle){
		Vector3 direction = DirectionFromAngle(globalAngle, true);
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position, direction, out hit, this.mViewRadius, this.mObstaclesMask)){
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}else{
			return new ViewCastInfo(false, transform.position + direction * this.mViewRadius, this.mViewRadius, globalAngle);
		}
	}
	
	private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast){
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;
		
		for(int i = 0; i < this.mEdgeResolveIteration; i++){
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo view = this.ViewCast(angle);
			
			bool edgeDistanceTresholdExceeded = Mathf.Abs(minViewCast.dst - view.dst) > this.mEdgeDstTreshold;
			if(view.hit == minViewCast.hit && !edgeDistanceTresholdExceeded){
				minAngle = angle;
				minPoint = view.point; 
			}else{
				maxAngle = angle;
				maxPoint = view.point;
			}
			
		}
		
		return new EdgeInfo(minPoint, maxPoint);
		
	}
	
	private void FindVisibleTargets(){
		this.mVisibleTargets.Clear();
		Collider[] targetsInViewRadius = Physics.OverlapSphere(this.transform.position, this.mViewRadius, this.mTargetMask);
		
		foreach(Collider target in targetsInViewRadius){
			Transform t = target.transform;
			Vector3 direction = (t.position - this.transform.position).normalized;
			
			if(Vector3.Angle( this.transform.forward, direction ) < this.mViewAngle / 2){
				float distanceToTarget = Vector3.Distance(this.transform.position, t.position);
				
				if(!Physics.Raycast(this.transform.position, direction, distanceToTarget, this.mObstaclesMask)){
					this.mVisibleTargets.Add(t);
				}
			}
		}
	}
	
	private void FindPlayerTarget(){
		if(this.mTurret != null && this.mTurret.hasTarget)
			return;
		
		Collider[] targetsInViewRadius = Physics.OverlapSphere(this.transform.position, this.mViewRadius, this.mPlayerMask);
	
		foreach(Collider target in targetsInViewRadius){
			Transform t = target.transform;
			Vector3 direction = (t.position - this.transform.position).normalized;

			if(Vector3.Angle( this.transform.forward, direction ) < 360f){
				float distanceToTarget = Vector3.Distance(this.transform.position, t.position);

				if(!Physics.Raycast(this.transform.position, direction, distanceToTarget, this.mObstaclesMask)){
					this.mTurret.target = t;
					this.mTurret.hasTarget = true;
				}
			}
		}		
	}
	
	private void DrawFieldOfView(){
		int stepCount = Mathf.RoundToInt(this.mViewAngle * this.mMeshResolution);
		float stepAngleSize = this.mViewAngle / stepCount;
		List<Vector3>viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo ();
		
		for(int i = 0; i <= stepCount; i++){
			float angle = transform.eulerAngles.y - this.mViewAngle / 2 + stepAngleSize * i;
			ViewCastInfo viewCastInfo = ViewCast(angle);
			
			if(i > 0){
				bool edgeDistanceTresholdExceeded = Mathf.Abs(oldViewCast.dst - viewCastInfo.dst) > this.mEdgeDstTreshold;
				if(oldViewCast.hit != viewCastInfo.hit || (oldViewCast.hit && viewCastInfo.hit && edgeDistanceTresholdExceeded)){
					EdgeInfo edge = FindEdge(oldViewCast, viewCastInfo);
					if(edge.pointA != Vector3.zero){
						viewPoints.Add(edge.pointA);
					}
					
					if(edge.pointB != Vector3.zero){
						viewPoints.Add(edge.pointB);
					}
				}
			}
			
			viewPoints.Add(viewCastInfo.point);
			oldViewCast = viewCastInfo;
		}
		
		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];
		 
		vertices[0] = Vector3.zero;
		for(int i = 0; i < vertexCount-1 ; i++){
			vertices[i + 1] = this.transform.InverseTransformPoint(viewPoints[i]);			
			if( i < vertexCount - 2){
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}
				
		this.mViewMesh.Clear();
		this.mViewMesh.vertices = vertices;
		this.mViewMesh.triangles = triangles;
		this.mViewMesh.RecalculateNormals();
	}
}
