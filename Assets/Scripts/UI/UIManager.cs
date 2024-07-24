using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Canvas canvas;
    
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject MuteButton;
    [SerializeField] private GameObject live1;
    [SerializeField] private GameObject live2;
    [SerializeField] private GameObject live3;
    [SerializeField] private TextMeshProUGUI floorNumberText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    public List<GameObject> list;
    [SerializeField] private GameObject missedPoopsMenu;
    [SerializeField] private GameObject playerHitBySuicideMenu;
    [SerializeField] private GameObject tooSlowMenu;
    [SerializeField] private GameObject ShameMenu;
    [SerializeField] private GameObject birdAttackMenu;
    private const string HighScoreKey = "HighScore";
    
    [SerializeField] private GameObject poopIndicator;
    [SerializeField] private GameObject birdIndicator;
    [SerializeField] private GameObject suicideIndicator;

    [SerializeField] private Image spotlight;
    [SerializeField] private float spotlightTime = 0.5f;
    
    

    private void Awake()
    {
        if (instance != null)
        {
            throw new Exception("UIManager singleton violation!");
        }
        
        instance = this;
        HidePauseMenu();
        ShowLives();
        HideAllLosingMenus();
        
    }

    private void ShowLives()
    {
        live1.SetActive(true);
        live2.SetActive(true);
        live3.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        PauseMenu.SetActive(true);
        MuteButton.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void HidePauseMenu()
    {
        PauseMenu.SetActive(false);
        MuteButton.SetActive(false);
        PauseButton.SetActive(true);
    }


    public void GameOver(string losingReason, int floor, GameObject player)
    {
        PauseButton.SetActive(false); // avoid unintended interactions
        Debug.Log("GameOver triggered with reason: " + losingReason); // Add debug statement
        switch (losingReason)
        {
            case "MissedPoops":
                missedPoopsMenu.SetActive(true);
                Debug.Log("MissedPoops menu activated");
                break;
            case "PlayerHitBySuicide":
                playerHitBySuicideMenu.SetActive(true);
                Debug.Log("PlayerHitBySuicide menu activated");
                break;
            case "TooSlow":
                tooSlowMenu.SetActive(true);
                Debug.Log("TooSlow menu activated");
                break;
            case "Shame":
                StartCoroutine(ShameGameOver(player, floor));
                return; // becuase of the coroutine, we will show score manually after the time for the spotlight passed
            case "BirdAttack":
                birdAttackMenu.SetActive(true);
                Debug.Log("BirdAttack menu activated");
                break;
            default:
                Debug.LogError("Unknown losing reason: " + losingReason);
                break;
        }
        ShowScore(floor);
    }

    private void HideAllLosingMenus()
    {
        missedPoopsMenu.SetActive(false);
        playerHitBySuicideMenu.SetActive(false);
        tooSlowMenu.SetActive(false);
        ShameMenu.SetActive(false);
        birdAttackMenu.SetActive(false);
        birdIndicator.SetActive(false);
        poopIndicator.SetActive(false);
        suicideIndicator.SetActive(false);
    }
    
    

    public void DeActivateLifeIcon(int lifeIndex, string reason)
    {
        switch (lifeIndex)
        {
            case 1:
                live1.SetActive(false);
                break;
            case 2:
                live2.GetComponent<Animator>().SetBool("Active",false);
                break;
            case 3:
                live3.GetComponent<Animator>().SetBool("Active",false);
                break;
            default:
                Debug.LogError("Invalid life index: " + lifeIndex);
                break;
        }
        ShowIndicator(reason);
        
    }
    
    
    public void ShowIndicator(string losingReason)
    {
        if (losingReason == "MissedPoops")
        {
            StartCoroutine(ShowIndicatorCoroutine(poopIndicator));
        }
        else if (losingReason == "BirdAttack")
        {
            StartCoroutine(ShowIndicatorCoroutine(birdIndicator));
        }
        else if (losingReason == "PlayerHitBySuicide")
        {
            StartCoroutine(ShowIndicatorCoroutine(suicideIndicator));
        }
        else
        {
            return;
        }
    }
    
    private IEnumerator ShowIndicatorCoroutine(GameObject indicator)
    {
        indicator.SetActive(true);
        yield return new WaitForSeconds(2f);
        indicator.SetActive(false);
    }
    

    public void ActivateLifeIcon(int lifeIndex)
    {
        switch (lifeIndex)
        {
            case 2:
                live2.GetComponent<Animator>().SetBool("Active",true);
                break;
            case 3:
                live3.GetComponent<Animator>().SetBool("Active",true);
                break;
            default:
                Debug.LogError("Invalid life index: " + lifeIndex);
                break;
        }
        
    }
    
    public void UpdateFloorNumberText(int floor)
    {
        floorNumberText.text = floor.ToString();
    }

    public void ShowScore(int score)
    {
        int highScore = LoadHighScore();
        if (score > highScore)
        {
            highScoreText.text = "High Score: " + score.ToString() + " - You broke the high score!";
            SaveHighScore(score);
        }
        else
        {
            scoreText.text = "Score: " + score.ToString();
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }
    
    public void HideScore()
    {
        scoreText.text = "";
        highScoreText.text = "";
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(HighScoreKey, score);
        PlayerPrefs.Save();
    }

    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private IEnumerator ShameGameOver(GameObject player, int floor)
    {
        //center spotlight on player for intended duration
        spotlight.gameObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, canvas.worldCamera, out canvasPos);
        spotlight.rectTransform.localPosition = canvasPos;
        
        yield return new WaitForSecondsRealtime(spotlightTime);
        
        spotlight.gameObject.SetActive(false);
        ShameMenu.SetActive(true); 
        ShowScore(floor);
    }

}