using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : FlyingObject
{
    [Tooltip("value for a bird flying from left to right")]
    public override Vector2 speed { get; set; } = new Vector2(1f, 0);
    public override string deathReason { get; set; } = "BirdAttack";
    [Tooltip("value for a bird flying from left to right")]
    public override Vector2 spawnOffsetFromCameraEdge { get; set; } = new Vector2(-2f, 0);

    public override void Init(List<float> ySpawnPositions)
    {
        // +2 from start as the bottom floors will likely be off screen by the time the bird gets there
        float yPosition = ySpawnPositions[Random.Range(2, ySpawnPositions.Count)];
        int leftOrRight = Random.Range(0, 2);
        if (leftOrRight == 0) // go left to right
        {
            float xPosition = mainCamera.ViewportToWorldPoint(Vector3.zero).x;
            transform.position = new Vector2(xPosition, yPosition) + spawnOffsetFromCameraEdge;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            float xPosition = mainCamera.ViewportToWorldPoint(Vector3.right).x;
            transform.position = new Vector2(xPosition,yPosition) - spawnOffsetFromCameraEdge;
            speed = new Vector2(-1*speed.x , 0);
        }
        //place indicator at left or right of the screen where bird is about to fly
        indicator = Instantiate(indicatorPrefab, Vector2.zero, indicatorPrefab.transform.rotation);
        float indicatorY = yPosition;
        if(leftOrRight == 0) // going left to right
        {
            float indicatorX = mainCamera.ViewportToWorldPoint(Vector3.zero).x + transform.localScale.x / 2;
            indicator.transform.rotation = Quaternion.Euler(0, 180, indicator.transform.rotation.eulerAngles.z);
            indicator.transform.position  = new Vector2(indicatorX, indicatorY);
        }
        else
        {
            float indicatorX = mainCamera.ViewportToWorldPoint(Vector3.right).x - transform.localScale.x / 2;
            indicator.transform.position  = new Vector2(indicatorX, indicatorY);
        }
        transform.localScale *= GameManager.instance.gameScale;
    }
}
