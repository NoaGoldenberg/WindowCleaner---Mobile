using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CameraParameters : ScriptableObject
{
    public float initialSpeed = 0.5f;
    
    public float cameraAcceleration = 1f;

    public float maxSpeed = 1.5f;
}
