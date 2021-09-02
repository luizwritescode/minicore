using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{   
    public int spawnAmount = 10;
    public GameObject enemy;
    ArrayList entList = new ArrayList();

    //Spawns n enemies and add them to an entity list
    public void SpawnEnemy(int n = 1)
    {   
        for (var i = 0; i < n; i++)
        {
        Vector3 spawnPoint = pickRandomPoint();
        entList.Add( Instantiate(enemy, spawnPoint, Quaternion.identity ) );

        }
    }
    void Start()
    {
        SpawnEnemy(spawnAmount);
    }

    void Update()
    {
        
    }

    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    //Picks a random point inside the SpawnArea
    public Vector3 pickRandomPoint()
    {
        Vector3 origin = transform.position;
        Vector3 range = transform.localScale / 2.0f;
        Vector3 randomRange = new Vector3(Random.Range(-range.x, range.x),
                                   1 , //  TO MAKE SURE ENEMY DOESNT CLIP ON THE GROUND <<< FIX THE HARDCODE TO enemy.HEIGHT
                                    Random.Range(-range.z, range.z));
        Vector3 randomCoordinate = origin + randomRange;

        return randomCoordinate;
    }
}
