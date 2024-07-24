using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Controls;

public class ImageSwitcher : MonoBehaviour
{
    public List<Image> images;
    private int currentIndex = 0;
    private bool isTouching = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip moveSound;
    
    [SerializeField] RectTransform buttonRectTransform;

    private void Start()
    {
        images[0].gameObject.SetActive(true);
        for (int i = 1; i < images.Count; i++)
        {
            images[i].gameObject.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on this GameObject.");
        }
    }

    private void Update()
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count == 0) return;

        TouchControl touch = Touchscreen.current.touches[0];
        
        // Check if the touch is over a UI element
        if (IsTouchOverButton(touch.position.ReadValue()))
        {
            return;
        }

        if (!isTouching && touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
        {
            isTouching = true;
        }
        else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && isTouching)
        {
            isTouching = false;
            moveFrame();
        }
    }
    
    bool IsTouchOverButton(Vector2 touchPosition)
    {
        if (buttonRectTransform == null)
            return false;

        // Convert the touch position to a position relative to the button's RectTransform
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(buttonRectTransform, touchPosition, null, out localPoint);

        // Check if the local point is within the bounds of the button's RectTransform
        return buttonRectTransform.rect.Contains(localPoint);
    }

    private void moveFrame()
    {
        if (audioSource != null && moveSound != null)
        {
            audioSource.PlayOneShot(moveSound);
        }

        images[currentIndex].gameObject.SetActive(false);
        currentIndex++;
        if (currentIndex == images.Count)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            images[currentIndex].gameObject.SetActive(true);
        }
    }

    public void skipTutorial()
    {
        Debug.Log("entered");
        SceneManager.LoadScene("Game");
    }
}