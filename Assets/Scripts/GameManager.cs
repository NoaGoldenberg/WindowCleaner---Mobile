using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private BuildingGenerator buildingGenerator;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject groundImage;
    private PlayerController playerController;
    public pickUpManager pickUpManager;

    private Vector2 windowSize;

    private int maxLives = 3;
    private int playerLives = 3;
    private int curFloor = 0;

    public bool isPlaying = false;
    public bool isGameOver = false;
    public int lastGeneratedLevel = 0;

    //a bunch of scripts use this, not sure how to do it better.
    [Tooltip("The scale for all our assets, since screen size is dynamic, we need to scale everything to fit the screen.")]
    public float gameScale;
    [SerializeField] private  CameraShake  _cameraShake;
    

    private void Awake()
    {
        if (instance != null)
        {
            throw new Exception("GameManager singleton violation!");
        }

        instance = this;
        Application.targetFrameRate = 60;
        PoopTracker.init();
    }

    private void Start()
    {
        buildingGenerator.GenerateBuilding();
        // setting player scale here is a way to make sure it happens after the building is generated and the game scale
        // is set. otherwise we don't know which start method will be called first. I'm open to better solutions.
        player.transform.localScale = new Vector3(gameScale, gameScale, 1); 
        playerController = player.GetComponent<PlayerController>();
        
        //place player under middle window
        Vector2 windowAbovePlayerLocation = buildingGenerator.GetBottomCenterWindowLocation();
        Vector2 playerLocation = new Vector2(windowAbovePlayerLocation.x, windowAbovePlayerLocation.y - windowSize.y);
        player.transform.position = playerLocation;
        groundImage.transform.localScale = new Vector3(gameScale, gameScale, 1);
        groundImage.transform.position = playerLocation;


    }

    public void SetWindowSize(Vector2 size)
    {
        windowSize = size;
    }

    public Vector2 GetWindowSize()
    {
        return windowSize;
    }
    
    public void AddLife()
    {
        if(playerLives >= maxLives) return;
        playerLives++;
        UIManager.instance.ActivateLifeIcon(playerLives);
    }

    public void ReduceLife(string reason)
    {
        if(isGameOver) return; // helps in case of multiple reasons for losing life in the same frame(i.e. 2 poops)
        if (reason != "MissedPoops")
        {
            // this is to avoid reducing life for multiple frames for the same impact
            if (!playerController.TakeDamage()) 
                    return;
        }

        UIManager.instance.DeActivateLifeIcon(playerLives, reason);
        playerLives--;
        
        if (playerLives <= 0)
        {
            
            Debug.Log("Game Over!");
            GameOver(reason);
        }
        else
        {
            _cameraShake.TriggerShake();
            Debug.Log("Lives left: " + playerLives);
        }
    }

    public void PlayerMoved()
    {
        // start camera moving if it's the first move of the game
        if (!isPlaying)
        {
            isPlaying = true;
        }
        // possibly add sounds, other events relating to when the player moves. 
        // Ideally this method should be in some sort of observer
    }

    public void GameOver(string losingReason)
    {
        AudioManager.instance.PlayLoseSound();
        Time.timeScale = 0;
        isPlaying = false;
        isGameOver = true;
        UIManager.instance.GameOver(losingReason, curFloor, player);
        
    }

    public void ChangeCurFloor(int delta)
    {
        curFloor += delta;
        UIManager.instance.UpdateFloorNumberText(curFloor);
    }
    
    
}