using UnityEngine;
using System.Collections;

public class PowerCrateSpawn : MonoBehaviour {
    public GameObject PowerCrate;
    public float firstInterval;
    public float minInterval;
    public float maxInterval;

    private GameObject[] spawnPoints;
    private float internInterval;
    private int pointCount;

	void Start () {
        //seek out all gameobjects tagged with "PowerCrateSpawn", and push them to the array.
        spawnPoints = GameObject.FindGameObjectsWithTag("PowerCrateSpawn");
        //set the first interval
        internInterval = firstInterval;
    }
	
	void FixedUpdate () {
        //Countdown from the intervaltime to zero (substract time between fixed updates from time left)
        internInterval -= Time.deltaTime;
        //Debug.Log(internInterval);
        if (internInterval <= 0)
        {
            //Reset pointCount
            pointCount = 0;
            //foreach to check if there are free spawnpoints
            foreach (GameObject point in spawnPoints)
            {
                //If the spawnpoint has a child add 1 to the count of occupied spawnpoints
                if(point.transform.childCount != 0)
                    pointCount++;
            }
            //Debug.Log(pointCount);
            //If there is a free spawnpoint continue
            if (pointCount != spawnPoints.Length)
            {
                //spawn a power-up
                SpawnPowerUp();

                //reset timer, value between the minimal interval and the maximal interval.
                internInterval = Random.Range(minInterval, maxInterval);
                //Debug.Log(internInterval);
            }
        }
    }

    void SpawnPowerUp()
    {
        // generate a random integer between  and the amount of spawnpoints
        int x = Random.Range(0, spawnPoints.Length);
        //Get a spawnpoint check if it doesn't have a child
        if(spawnPoints[x].transform.childCount == 0)
        {
            //spawn a power-up cube and place it on the spawnpoint.
            GameObject newCube = Instantiate(PowerCrate, spawnPoints[x].transform.position, Quaternion.identity);
            //make the power-up cube a child of the spawnpoint.
            newCube.transform.parent = spawnPoints[x].transform;
        }
    }
}
