using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    SpawnEnemy(quantity, range)                Spawns n enemies at distance range from the Player and add them to an entity list
                                               if range is not set picks random point inside entire spawn area 

    PickRandomPoint()                          Picks a random point inside the SpawnArea
    PickRandomPoint(origin)                    Picks a random point inside the given origin
    PickRandomPoint(origin, range)             Picks a random point at the given origin at a min range 
    PickRandomPoint(origin, range, range)      Picks a random at the given origin at a min range x and max range y

    @TODO
     - add y value to the range so spawn area can be in 3d
*/

public class SpawnArea : MonoBehaviour
{   
    public Transform playerTransform;
    public int initialSpawnAmount = 10;
    public int maxSpawnAmount = 1000;
    public GameObject enemy;
    ArrayList entList = new ArrayList();

    //Spawns n enemies at distance range from the Player and add them to an entity list
    //if range is not set picks random point inside entire spawn area
    public void SpawnEnemy(float min_range = 0f, float max_range = 0f, int n = 1)
    {   
        
        Vector3 spawnPoint;

        for (var i = 0; i < n; i++)
        {
            if(min_range > 0f && max_range > 0f && max_range > min_range)
            {
                //ranged around player spawn   
                spawnPoint = PickRandomPoint(playerTransform.position, min_range, max_range);
            } else {
                spawnPoint = PickRandomPoint(transform.position, min_range, max_range);
            }

        entList.Add( Instantiate(enemy, spawnPoint, Quaternion.identity ) );
        }
    }

    void Start()
    {   
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        SpawnEnemy(initialSpawnAmount);
    }

    void Update()
    {

        if(entList.Count < maxSpawnAmount)
        {
            //Debug.Log( entList.Count + " Enemies remaining...");
            SpawnEnemy(30f, 80f);
        }
        
    }

    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }


    public Vector3 PickRandomPoint(Vector3 origin, float minRange, float maxRange)
    {   

        Vector3 rangeVector;
        Vector3 randomRange;


        if(minRange <= 0f)
        {
            rangeVector = transform.localScale /  2.0f;
            randomRange = new Vector3(Random.Range(-rangeVector.x, rangeVector.x),
                                                                     1 , //  TO MAKE SURE ENEMY DOESNT CLIP ON THE GROUND <<< FIX THE HARDCODE TO enemy.HEIGHT
                                    Random.Range(-rangeVector.z, rangeVector.z));
        } 
        else
        {
            Vector3 minRangeVector = new Vector3(minRange, 0, minRange);
            Vector3 maxRangeVector = new Vector3(maxRange, 0, maxRange);
            
            randomRange = new Vector3(Random.Range(minRangeVector.x, maxRangeVector.x),
                                                                    1 , // here too
                                    Random.Range(-minRangeVector.z, maxRangeVector.z));
        }

        Vector3 randomCoordinate = origin + randomRange;

        return randomCoordinate;
    }
}
