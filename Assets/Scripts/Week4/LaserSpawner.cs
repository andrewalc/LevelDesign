using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform target;
    public bool active;
    [Range(0f, 5f)]
    public float spawnInterval;

    public float startDelay;

    private void Start()
    {
        active = true;
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(SpawnLaser());
    }

    IEnumerator SpawnLaser()
    {
        while (active)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.transform.LookAt(target);
            yield return new WaitForSeconds(spawnInterval);
        }

    }
}
