using UnityEngine;
using System.Collections;

public class PowerUpCube : MonoBehaviour {
    private float amplitude = 0.2f;
    private float y0;
    private int x;

    public GameObject hitParticle;
    public string[] powerUps = {"Health", "Shield", "Special", "Boost"};
    

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        y0 = transform.position.y;
        x = Random.Range(0, powerUps.Length);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Head" || col.tag == "Left" || col.tag == "Right" || col.tag == "Car")
        {
            Destroy(this.gameObject);
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

    //TODO: function for the "Health" pick-up [effect, set to UI, etc]
    void Health()
    {
        Debug.Log("Health pickup");
    }

    //TODO: function for the "Shield" pick-up [effect, set to UI, etc]
    void Shield()
    {
        Debug.Log("Shield pickup");
    }

    //TODO: function for te "Special weapon" pick-up [effect, which special weapon, set to UI, etc]
    void Special()
    {
        Debug.Log("Special pickup");
    }

    //TODO: function for te "Speed boost" pick-up [effect, set to UI, etc]
    void Boost()
    {
        Debug.Log("Speed Boost pickup");
    }
}   
