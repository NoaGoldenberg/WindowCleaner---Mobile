using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : FlyingObject
{
    [Tooltip("value for a cloud going from left to right")]
    public override Vector2 speed { get; set; } = new Vector2(0.5f, 0);
    public override string deathReason { get; set; } = "clouds shouldn't kill";
    [Tooltip("value for a cloud going from left to right")]
    public override Vector2 spawnOffsetFromCameraEdge { get; set; } = new Vector2(0, 1);

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        //it's a cloud, it does nothing - this code is just in case but I probably didn't give it a collider anyway
        return;
    }

    public override void Init(List<float> unused)
    {
        int leftOrRight = Random.Range(0, 2);
        transform.position = (Vector2)Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 1 , 0)) + spawnOffsetFromCameraEdge;
        if (leftOrRight == 1) // change dir to  right
        {
            speed = new Vector2(-1*speed.x , 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        transform.localScale *= GameManager.instance.gameScale;
    }
}
