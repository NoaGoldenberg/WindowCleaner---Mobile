using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingMan : FlyingObject
{
    // Start is called before the first frame update
    public override Vector2 speed { get; set; } = new Vector2(0, -1.5f);
    public override string deathReason { get; set; } = "PlayerHitBySuicide";
    public override Vector2 spawnOffsetFromCameraEdge { get; set; } = new Vector2(0, 4);

    public override void Init(List<float> xSpawnPositions)
    {
        float xPosition = xSpawnPositions[Random.Range(0, xSpawnPositions.Count)];
        float yPosition = mainCamera.ViewportToWorldPoint(Vector3.up).y;
        transform.position = new Vector2(xPosition, yPosition) + spawnOffsetFromCameraEdge;
        transform.localScale *= GameManager.instance.gameScale;
        //place indicator at the top of the screen where person is about to fall
        indicator = Instantiate(indicatorPrefab, Vector2.zero, indicatorPrefab.transform.rotation);
        float indicatorY = mainCamera.ViewportToWorldPoint(Vector3.up).y - (transform.localScale.y * 1.5f);
        indicator.transform.position  = new Vector2(xPosition, indicatorY);
        indicator.transform.SetParent(mainCamera.transform); // so it stays at the top of the screen as it rises
    }
}
