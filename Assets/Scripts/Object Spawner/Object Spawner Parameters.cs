using UnityEngine;


[CreateAssetMenu]
public class ObjectSpawnerParameters : ScriptableObject
{
    public int levelToSpawnBirds = 0;
    public float minTimeBetweenBirdSpawns = 3f;
    public float maxTimeBetweenBirdSpawns = 10f;
    public int levelToSpawnClouds = 0;
    public float minTimeBetweenCloudSpawns = 5f;
    public float maxTimeBetweenCloudSpawns = 7f;
    public int levelToSpawnFallingMan = 0;
    public float minTimeBetweenFallingManSpawns = 5f;
    public float maxTimeBetweenFallingManSpawns = 20f;
}