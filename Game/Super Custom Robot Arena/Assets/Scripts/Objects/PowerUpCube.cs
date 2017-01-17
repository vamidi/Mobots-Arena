using UnityEngine;
using System.Collections;

public class PowerUpCube : MonoBehaviour {
    private float amplitude = 0.2f;
    private float y0;
    public GameObject hitParticle;

    public string[] powerUps = {"Health", "Shield", "Special", "Boost"};
    private int x;
    

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        y0 = transform.position.y;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Head" || col.tag == "Left" || col.tag == "Right" || col.tag == "Car")
        {
            Destroy(this.gameObject);
            x = Random.Range(0, powerUps.Length);
            //Debug.Log(col.tag);
            GameObject particle = (GameObject)Instantiate(hitParticle, this.transform.position, Quaternion.identity);
            Destroy(particle, 1.2f);
            switch (powerUps[x])
            {
                case "Health":
                    Health();
                    break;
                case "Shield":
                    Shield();
                    break;
                case "Special":
                    Special();
                    break;
                case "Boost":
                    Boost();
                    break;
                default:
                    Debug.Log("Power-Up");
                    break;
            }
        }
    }
	
	void FixedUpdate () {
        transform.Rotate(Vector3.up * Time.deltaTime * 45, Space.World);
        transform.position = new Vector3(transform.position.x, y0 + amplitude * Mathf.Sin(5 * Time.time),transform.position.z);
    }

    void Health()
    {
        Debug.Log("Health pickup");
    }

    void Shield()
    {
        Debug.Log("Shield pickup");
    }

    void Special()
    {
        Debug.Log("Special pickup");
    }

    void Boost()
    {
        Debug.Log("Speed Boost pickup");
    }
}   
