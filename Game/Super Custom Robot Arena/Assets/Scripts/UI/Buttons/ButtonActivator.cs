using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonActivator : MonoBehaviour {

	public bool isActivated;

	// Use this for initialization
	void Start () {
		int i = 0;
		foreach(Transform child in this.transform){
			if(i != 0){
				if(child.GetComponent<Button>() != null){
					child.gameObject.SetActive(this.isActivated);
				}
			}

			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void ToggleButtons(){
		int i = 0;
		this.isActivated = !this.isActivated;
		foreach(Transform child in this.transform){
			if(i != 0){
				if(child.GetComponent<Button>() != null){
					child.gameObject.SetActive(this.isActivated);	
				}
			}

			i++;
		}
	}
}
