using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpManager : MonoBehaviour
{

    [SerializeField] private GameObject heartPickup;

    [SerializeField] private float heartPickupSpawnChance = 0.01f;
    [SerializeField] private int minWindowsBetweenPickups = 20;
    private int windowsFromLastPickup = 0;

    public GameObject GetPickup()
    {
        windowsFromLastPickup++;
        if (windowsFromLastPickup < minWindowsBetweenPickups) return null;
        float randVal = Random.value;
        if (randVal < heartPickupSpawnChance)
        {
            windowsFromLastPickup = 0;
            return heartPickup;
        }
        return null;
    }
}
