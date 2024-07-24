using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Window : MonoBehaviour
{

    [SerializeField] private WindowParameters windowParameters;

    // all window components for random generation. window assets hold list of objects to randomize from so they're not all held locally on each window.
    [SerializeField] private WindowAssets windowAssets;
    [SerializeField] private SpriteRenderer frameSprite;
    [SerializeField] private SpriteRenderer dirtSprite;
    [SerializeField] private OfficePersonAnimCtrl personAnimCtrl;
    [SerializeField] private SpriteRenderer backgroundSprite;
    
    ParticleSystem cleaningEffect;

    private List<Sprite> dirtSpriteList;
    private int dirtSpriteIndex = 0;

    public bool isDirty;
    public bool isLooking;

    public void Init(int floor, int windowInFloor)
    {
        //reset all variables and stop animation coroutines to be safe.
        isDirty = false;
        isLooking = false;
        // randomly select window components from given lists.
        frameSprite.sprite = windowAssets.windowFrames[Random.Range(0, windowAssets.windowFrames.Count)];
        backgroundSprite.sprite = windowAssets.backgroundSprites[Random.Range(0, windowAssets.backgroundSprites.Count)];
        InitAnimation(floor, windowInFloor);
        bool hasPickup = InitPickup();
        if (!hasPickup) // window shouldn't be dirty and have a pickup at the same time.
            InitDirt(floor);
        else
        {
            isDirty = false; dirtSprite.sprite = null;
        }

        cleaningEffect = FindObjectOfType<ParticleSystem>();
        // Debug.Log();
    }

    private void InitAnimation(int floor, int windowInFloor)
    {
        RuntimeAnimatorController randomAnimator = windowAssets.personAnimatorControllers[Random.Range(0, windowAssets.personAnimatorControllers.Count)];
        // logic for this part is in the png in the window folder, essentially we're desynching the window animations so they don't detect player simultaniously
        int fifthToStartAnimation = (3 * floor + windowInFloor) % 5;
        float animationStartPoint = Random.Range(0.2f*fifthToStartAnimation, 0.2f * (fifthToStartAnimation+1));
        personAnimCtrl.InitializeAnimation(randomAnimator, animationStartPoint);
        
    }

    private bool InitPickup()
    {
        GameObject pickupPrefab = GameManager.instance.pickUpManager.GetPickup();
        if(pickupPrefab == null)
            return false;
        Instantiate(pickupPrefab, transform);
        return true;
    }
    private void InitDirt(float floor)
    {
        dirtSpriteIndex = 0;
        isDirty = PoopTracker.ShouldAddPoop();
        if (!isDirty)
        {
            dirtSprite.sprite = null;
            return;
        }

        PoopTracker.PoopCount++;
        dirtSpriteList = windowAssets.dirtSprites;
        dirtSprite.sprite = dirtSpriteList[dirtSpriteIndex];
    }
    
    public bool clean() // returns if it actually cleaned
    {
        if(dirtSprite.sprite == null)
            return false;
        if (dirtSpriteIndex >= dirtSpriteList.Count - 1)
        {
            dirtSprite.sprite = null;
            isDirty = false;
            Vector3 particleSysOffset = new Vector3(0, dirtSprite.size.y * GameManager.instance.gameScale / 4, 0);
            cleaningEffect.transform.position = dirtSprite.transform.position + particleSysOffset;
            cleaningEffect.Play();
            AudioManager.instance.PlayFinishCleanSound();
            Handheld.Vibrate();
            PoopTracker.PoopCount--;
            return true;
        }
        AudioManager.instance.PlayCleanSound();
        dirtSprite.sprite = dirtSpriteList[++dirtSpriteIndex];
        return true;
    }


    public void StartLooking()
    {
        isLooking = true;
    }

    public void StopLooking()
    {
        isLooking = false;
    }
}
