using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float repeatRate = 0.2f;
    [SerializeField] private float spawnChance = 50f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnTarget", 0f, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTarget()
    {
        // need to pass the random check
        int rand = Random.Range(0, 101);
        if (rand < spawnChance)
            return;

        // create the object
        Vector3 spawnPosition = spawnPos;
        spawnPosition.x += Random.Range(-spawnOffset.x, spawnOffset.x);
        spawnPosition.y += Random.Range(-spawnOffset.y, spawnOffset.y);
        spawnPosition.z += Random.Range(-spawnOffset.z, spawnOffset.z);
        GameObject ob = Instantiate(target, spawnPosition, Quaternion.Euler(-90f, 0f, 0f)); // rotate the model to compensate for Blender's coordinate system

        // cleanup target
        Destroy(ob, 10f);
    }

}
