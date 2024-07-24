using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlyingObject : MonoBehaviour
{
    public abstract Vector2 speed { get; set; }
    public abstract string  deathReason { get; set; }

    public abstract Vector2 spawnOffsetFromCameraEdge { get; set;}
    
    private bool enteredScreen;
    private bool deletedIndicator;
    
    [SerializeField] protected GameObject indicatorPrefab;
    protected GameObject indicator = null;
    
    protected Camera mainCamera; // so we don't look for it every frame
    private float cameraHeight;
    private float cameraWidth;
    private float objectWidth;
    private float objectHeight;

    void Awake()
    {
        mainCamera = Camera.main;
        cameraHeight = mainCamera.orthographicSize * 2;
        cameraWidth = cameraHeight * mainCamera.aspect;
    }
    void Start()
    {
        enteredScreen = false;
        objectWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        objectHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    //note: if performance is ever an issue, this one can be optimized by not checking every frame.
    void Update()
    {
        transform.position += (Vector3)speed * Time.deltaTime;
        // if object entered the camera destroy indicator
        if (!enteredScreen && IsObjectInCamera())
        {
            enteredScreen = true;
            if(indicator)
                Destroy(indicator);
        }
        // if object leaves camera, destroy it
        if (enteredScreen && !IsObjectInCamera())
        {
            Destroy(gameObject);
        }
    }

    private bool IsObjectInCamera()
    {
        // Calculate the camera bounds
        float leftBound = mainCamera.transform.position.x - cameraWidth / 2 - objectWidth / 2;
        float rightBound = mainCamera.transform.position.x + cameraWidth / 2 + objectWidth / 2;
        float bottomBound = mainCamera.transform.position.y - cameraHeight / 2 - objectHeight / 2;
        float topBound = mainCamera.transform.position.y + cameraHeight / 2 + objectHeight / 2;
        

        // Check if the object's position is within the camera bounds
        bool isVisible = transform.position.x >= leftBound && transform.position.x <= rightBound &&
                         transform.position.y >= bottomBound && transform.position.y <= topBound;

        return isVisible;
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlayHitSound();
            GameManager.instance.ReduceLife(deathReason);
        }
    }

    public abstract void Init(List<float> spawnPositions);
}
