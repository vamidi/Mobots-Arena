using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public LayerMask mLayer;
	public Transform mGunEnd, mOldPos;

	public GameObject lArm;
	public GameObject player;

	
	private Player mPlayer;
	
	
	// Use this for initialization
	void Start () {
//		this.mPlayer = this.transform.root.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(this.mPlayer && this.mPlayer.isControllable){
			this.gameObject.SetActive(true);
			Vector3 rayOrg = this.mGunEnd.position;
			RaycastHit hit;
			Vector3 pos = mOldPos.position;
			float distance = Vector3.Distance(rayOrg, this.mOldPos.position);
			float compareDistance = 0f;
			if(Physics.Raycast(rayOrg, this.mGunEnd.transform.forward, out hit, this.mLayer) ) {
				if(hit.collider.tag != "Bullet"){
					compareDistance = Vector3.Distance(rayOrg, hit.point);
					if(distance > compareDistance ){
						pos = hit.point;
					}
				}
			}
			this.transform.position = pos;
			if(distance > compareDistance )
				this.transform.localPosition += new Vector3(0,0,-.3f);
				
			this.transform.LookAt(Camera.main.transform.position);
			this.transform.Rotate(0, 180f, 0);	
		}
		
		// center point of the player
//		Vector3 centerpointPlayer = Vector3.zero;
//		Vector3 centerpointLeftArm = Vector3.zero;
//		
//		if(player){
//			centerpointPlayer = player.transform.position;
//		}
//		if(lArm){
//			centerpointLeftArm = lArm.GetComponent<SphereCollider>().bounds.center;
//		}
//		
//		if(centerpointPlayer != Vector3.zero && centerpointLeftArm != Vector3.zero){
//			float distX = Vector3.Distance(centerpointPlayer, centerpointLeftArm);
//			var a = Mathf.Tan(2.5f) * distX;
////			Debug.Log(a);
//		}
		
		
	}
}
