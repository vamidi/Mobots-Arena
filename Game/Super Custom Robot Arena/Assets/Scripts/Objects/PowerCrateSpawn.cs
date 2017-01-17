using UnityEngine;
using System.Collections;

public class PowerCrateSpawn : MonoBehaviour {
    public GameObject PowerCrate;
    private GameObject[] spawnPoints;
    public float interval;
    private float internInterval;

    public int x;
	// Use this for initialization
	void Start () {
        spawnPoints = GameObject.FindGameObjectsWithTag("PowerCrateSpawn");
        SpawnPowerUp();
        Debug.Log(spawnPoints.Length);
    }
	
	// Update is called once per frame
	void Update () {
        internInterval -= Time.deltaTime;
        //Debug.Log(internInterval);
        if (internInterval <= 0)
        {
            SpawnPowerUp();
            internInterval = interval;
        }
    }

    void SpawnPowerUp()
    {
        x = Random.Range(0, spawnPoints.Length);
        if(spawnPoints[x].transform.childCount == 0)
        {
            GameObject newCube = Instantiate(PowerCrate, spawnPoints[x].transform.position, Quaternion.identity);
            newCube.transform.parent = spawnPoints[x].transform;
        }
    }
}
