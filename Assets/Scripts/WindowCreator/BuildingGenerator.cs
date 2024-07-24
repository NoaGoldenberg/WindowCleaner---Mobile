using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private WindowPlaceParameters windowParameters;

    private Camera mainCamera;
    private float cameraWidth;
    private float cameraHeight;
    
    private float windowScale;
    private Vector2 windowSize;
    
    private LinkedList<List<GameObject>> windowRows = new LinkedList<List<GameObject>>();
    private int nextLevel;

    public List<float> windowsXValues;
    public List<float> windowsYValues;
    



    void SetWindowScale()
    {
        cameraWidth = mainCamera.orthographicSize * mainCamera.aspect * 2f;
        cameraHeight = mainCamera.orthographicSize * 2;

        Bounds windowBounds = windowPrefab.GetComponent<SpriteRenderer>().bounds;
        windowSize.x = windowBounds.size.x;
        windowSize.y = windowBounds.size.y;
        windowScale = (cameraWidth * windowParameters.windowBlockWidthPercentage) / windowSize.x;
        GameManager.instance.gameScale = windowScale;
        windowSize = new Vector2(windowSize.x * windowScale, windowSize.y * windowScale);
        GameManager.instance.SetWindowSize(windowSize);
    }

    public void GenerateBuilding()
    {
        mainCamera = Camera.main;
        SetWindowScale();
        for (nextLevel = 1; nextLevel < cameraHeight / windowSize.y + 2; nextLevel++)
        {
            CreateWindowRow(nextLevel);
        }
        foreach (GameObject window in windowRows.First.Value)
        {
            windowsXValues.Add(window.transform.position.x);
        }
    }
    void CreateWindowRow(int level)
    {
        List<GameObject> windowRow = new List<GameObject>(); 
        
        float windowPositionY = level * windowSize.y - cameraHeight / 2 + windowSize.y / 2;
        for (int i = 0; i < windowParameters.windowBlocksPerRow; i++)
        {
            float windowPositionX = (i - windowParameters.windowBlocksPerRow / 2.0f + 0.5f) * windowSize.x;
            Vector2 windowPosition = new Vector2(windowPositionX, windowPositionY);
            GameObject window = Instantiate(windowPrefab, windowPosition, Quaternion.identity);
            window.transform.localScale = new Vector3(windowScale, windowScale, 1);
            windowRow.Add(window);
            
            Window windowComponent = window.GetComponent<Window>();
            windowComponent.Init(level, i + 1);
        }
        windowRows.AddLast(windowRow);
        windowsYValues.Add(windowPositionY);
    }

    private void Update()
    {
        float bottomCameraY = mainCamera.transform.position.y - cameraHeight / 2;
        if (windowRows.First.Value[0].transform.position.y < bottomCameraY - windowSize.y / 2)
        {
            MoveWindowRow(nextLevel++);
        }
    }

    void MoveWindowRow(int level)
    {
        List<GameObject> windowRow = windowRows.First.Value;
        windowRows.RemoveFirst();
        windowRows.AddLast(windowRow);
        float windowPositionY = level * windowSize.y - cameraHeight / 2 + windowSize.y / 2;
        int i = 1;
        foreach (GameObject window in windowRow)
        {
            window.transform.position = new Vector2(window.transform.position.x, windowPositionY);
            
            Window windowComponent = window.GetComponent<Window>();
            if (windowComponent.isDirty)
            {
                GameManager.instance.ReduceLife("MissedPoops");
                AudioManager.instance.PlayHitSound();
             
            }

            windowComponent.Init(level, i);
            GameManager.instance.lastGeneratedLevel = level;
            i++;
        }
        windowsYValues.RemoveAt(0);
        windowsYValues.Add(windowPositionY);
    }

    public Vector2 GetBottomCenterWindowLocation()
    {
        return windowRows.First.Value[windowParameters.windowBlocksPerRow / 2].transform.position;
    }
   
}