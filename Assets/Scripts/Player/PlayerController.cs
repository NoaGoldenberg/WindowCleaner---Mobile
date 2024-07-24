using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    
    private Camera mainCamera;
    private float cameraHeight;
    private float playerHeight;

    private Window _curWindow = null;

    private bool isMoving = false;
    [SerializeField] private float movementTime = 0.5f;

    public bool isInvincible = false;
    [SerializeField] private float invincibleTime = 0.5f;


    private void Start()
    {
        mainCamera = Camera.main;
        cameraHeight = mainCamera.orthographicSize * 2;
    }
    private void Update()
    {
        if(GameManager.instance.isGameOver) return;
        if (_curWindow)
        {
            CheckForPlayer();
        }
        // check if player is under camera. we multiply playerHeight by 1.75 because the collider only covers a part of the player
        playerHeight = transform.localScale.y;
        if (transform.position.y < mainCamera.transform.position.y - cameraHeight / 2 - playerHeight * 1.75f)
            GameManager.instance.GameOver("TooSlow");
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Window") || _curWindow!= null) 
            return;
        _curWindow = other.GetComponent<Window>();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Window")) return;
        _curWindow = null;
    }
    
    
    public void CleanWindow()
    {
        if (_curWindow)
        {
            if(_curWindow.clean())
                animator.SetTrigger("Clean");
        }
    }
    
    private void CheckForPlayer()
    {
        //don't kill player if he's currently on his way out of the window/got in just at the end of the animation
        if (!GameManager.instance.isGameOver && _curWindow.isLooking && !isMoving && !isInvincible)
        {
            GameManager.instance.ReduceLife("Shame");
            AudioManager.instance.PlayHitSound();
        }
    }
    
    public void MoveWindows(Vector2 windowsToJump)
    {
        if (windowsToJump == Vector2.zero || isMoving) return;
        Vector2 newPosition = transform.position;
        Vector2 topRightCorner = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector2 bottomLeftCorner = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 windowSize = GameManager.instance.GetWindowSize();
        if (windowsToJump.x != 0)
        {
            float newXPosition = transform.position.x + windowSize.x * windowsToJump.x;
            if (newXPosition < topRightCorner.x && newXPosition > bottomLeftCorner.x)
                newPosition.x = newXPosition;
        }

        if (windowsToJump.y != 0)
        {
            float newYPosition = transform.position.y + windowSize.y * windowsToJump.y;
            if (newYPosition < topRightCorner.y && newYPosition > bottomLeftCorner.y)
            {
                newPosition.y = newYPosition;
                GameManager.instance.ChangeCurFloor((int)windowsToJump.y);
            }
        }

        if ((Vector2)transform.position != newPosition)
        {
            MovePlayer(newPosition);
            GameManager.instance.PlayerMoved();
        }
    }
    
    void MovePlayer(Vector2 newPosition)
    {
        isMoving = true; 
        transform.DOMove(newPosition, movementTime).SetEase(Ease.OutQuad);
        isMoving = false;
    }

    public bool TakeDamage()
    {
        if(isInvincible) return false;
        StartCoroutine(TookDamage());
        return true;
    }
    
    IEnumerator TookDamage()
    {
        isInvincible = true;
        animator.SetBool("Invincible", true);
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
        animator.SetBool("Invincible", false);
    }
}
