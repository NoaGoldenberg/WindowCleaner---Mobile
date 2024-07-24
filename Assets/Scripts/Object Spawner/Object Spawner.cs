using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private ObjectSpawnerParameters parameters;
    
    [SerializeField] private GameObject Cloud;
    [SerializeField] private GameObject Bird;
    [SerializeField] private GameObject FallingMan;

    [SerializeField] private BuildingGenerator buildingGenerator; // this one is for aligning with the windows x,y values


    private void Start()
    {
        StartCoroutine(SpawnClouds());
        StartCoroutine(SpawnBirds());
        StartCoroutine(SpawnFallingMen());
    }
    
    IEnumerator SpawnClouds()
    {
        while (true)
        {
            if(GameManager.instance.lastGeneratedLevel >= parameters.levelToSpawnClouds)
            {
                GameObject cloud = Instantiate(Cloud, Vector2.zero, quaternion.identity);
                cloud.GetComponent<Cloud>().Init(null);
            }
            float timeToWait = Random.Range(parameters.minTimeBetweenCloudSpawns, parameters.maxTimeBetweenCloudSpawns);
            yield return new WaitForSeconds(timeToWait);
        }
    }

    IEnumerator SpawnBirds()
    {
        while (true)
        {
            if (GameManager.instance.lastGeneratedLevel >= parameters.levelToSpawnBirds)
            {
                GameObject bird = Instantiate(Bird, Vector2.zero, quaternion.identity);
                bird.GetComponent<Bird>().Init(buildingGenerator.windowsYValues);
            }
            float timeToWait = Random.Range(parameters.minTimeBetweenBirdSpawns, parameters.maxTimeBetweenBirdSpawns);
            yield return new WaitForSeconds(timeToWait);
        }
    }
    
    IEnumerator SpawnFallingMen()
    {
        while (true)
        {
            if (GameManager.instance.lastGeneratedLevel >= parameters.levelToSpawnFallingMan)
            {
                GameObject fallingMan = Instantiate(FallingMan, Vector2.zero, quaternion.identity);
                fallingMan.GetComponent<FallingMan>().Init(buildingGenerator.windowsXValues);
            }
            float timeToWait = Random.Range(parameters.minTimeBetweenFallingManSpawns, parameters.maxTimeBetweenFallingManSpawns);
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
