using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public CameraParameters parameters;
    private bool accelerating = true;
    private float moveSpeed;
    
    private void Start()
    {
        accelerating = true;
        moveSpeed = parameters.initialSpeed;
    }
    
    void Update()
    {
        if(GameManager.instance.isPlaying == false) return;
        //removed feature to speed up when no poops on screen because people didn't like it.
        /*if (PoopTracker.PoopCount == 0) // if there are no poops on the screen, speed up to max(not permanent)
        {
            transform.position += Vector3.up * (Mathf.Min(2*moveSpeed, parameters.maxSpeed) * Time.deltaTime);
            return;
        }*/
        if (accelerating)
        {
            moveSpeed += parameters.cameraAcceleration * Time.deltaTime;
            if (moveSpeed >= parameters.maxSpeed)
            {
                moveSpeed = parameters.maxSpeed;
                accelerating = false;
            }
        }
        transform.position += Vector3.up * (moveSpeed * Time.deltaTime);
    }
}
