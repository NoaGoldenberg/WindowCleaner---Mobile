using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerTouchDetector : MonoBehaviour
{
    public float swipeThreshold = 50f;
    
    [SerializeField] private PlayerController playerController;
    private Vector2 startTouchPosition;
    private bool isTouching = false;
    
    private void Update()
    {
        if (GameManager.instance.isGameOver) return;
        DetectTouch();
    }

    void DetectTouch()
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count == 0) return;

        TouchControl touch = Touchscreen.current.touches[0];

        if (!isTouching && touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
        {
            isTouching = true;
            startTouchPosition = touch.position.ReadValue();
        }
        else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && isTouching)
        {
            isTouching = false;
            Vector2 movementDelta = touch.position.ReadValue() - startTouchPosition;
            HandleFinishedTouch(movementDelta);
        }
    }

    private void HandleFinishedTouch(Vector2 touchMovement)
    {
        // calculate if the touch was a swipe and means the player should move
        if(Mathf.Abs(touchMovement.x) > Mathf.Abs(touchMovement.y))
            touchMovement.y = 0;
        else
            touchMovement.x = 0;
        Vector2 windowsTojump = Vector2.zero;
        if (touchMovement.x > swipeThreshold)
            windowsTojump.x = 1;
        else if (touchMovement.x < -swipeThreshold)
            windowsTojump.x = -1;
        if (touchMovement.y > swipeThreshold)
            windowsTojump.y = 1;
        else if (touchMovement.y < -swipeThreshold)
            windowsTojump.y = -1;
        
        // if the touch was indeed a swipe, move the player, otherwise it was a tap and the player should clean the window
        if(windowsTojump != Vector2.zero)
            playerController.MoveWindows(windowsTojump);
        else
        {
            playerController.CleanWindow();
        }
    }
}
