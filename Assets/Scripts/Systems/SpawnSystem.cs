using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnSystem : MonoBehaviour {

    private bool hasSpawned;
    private Transform player;//We will use the player position to check if the player is near the spawn point

    public GameObject[] enemiesToSpawn;//Types of enemies that can be spawned

    public int maxSpawnAmount;

    //RANGES
    public float spawnRange;
    public float detectionRange;//How close the player should be so asenemies will not be spawned

    private void Start()
    {
        player = PlayerManager.instance.ourPlayer.transform; //Get player position
    }

    void Update()//Check if this spawnPoint hasSpawned and if player is inside the detection range
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance <= detectionRange)
        {
            if (!hasSpawned)
            {
                SpawnEnemies();
            }
        }
    }

    void SpawnEnemies()
    {
        hasSpawned = true;

        int spawnAmount = Random.Range(1, maxSpawnAmount+1);

        for (int i = 0; i < spawnAmount; i++) //Spawn an amount of enemies if no enemies have been spawned
        {
            //Creates a random position inside the spawn range
            float xSpawnPos = transform.position.x + Random.Range(-spawnRange, spawnRange);
            float zSpawnPos = transform.position.z + Random.Range(-spawnRange, spawnRange);

            //Creates the enemy in the above position
            Vector3 spawnPoint = new Vector3(xSpawnPos, 0, zSpawnPos);
            GameObject newEnemy = (GameObject)Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], spawnPoint, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        //We create a Handler which checks the Detection Range
        Handles.color = Color.blue;
        Handles.DrawWireArc(transform.position, transform.up, transform.right, 360, detectionRange);

        //Same for the Spawn Range
        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, transform.up, transform.right, 360, spawnRange);
    }

}
