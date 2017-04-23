using UnityEngine;
using System.Collections;

public class simpleRotation : MonoBehaviour {

    public float speed;
    public float amplitude;
    private float y0;
	// Use this for initialization
	void Start () {
        y0 = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.World);
        transform.position = new Vector3(transform.position.x, y0 + amplitude * Mathf.Sin(5 * Time.time), transform.position.z);
    }
}
