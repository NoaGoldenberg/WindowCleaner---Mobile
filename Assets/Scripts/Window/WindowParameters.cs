using UnityEngine;


[CreateAssetMenu]
public class WindowParameters : ScriptableObject
{
    
    [Range(0f, 1f)] public float DirtyChanceOnLevel1 = 0.15f;
    
    [Range(0f, 1f)] public float DirtyChanceOnLevel100 = 0.40f;
}