using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public LayerMask mLayer;
	public Transform mGunEnd, mOldPos;

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
		}else{
			
			// center point of the player
			Debug.Log(player.transform.position);
			
//			this.gameObject.SetActive(false);
		}
	}
}
