using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraLifePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if(collision.tag == "Player")
        {
            AudioManager.instance.PlayBonusSound();
            GameManager.instance.AddLife();
            Destroy(gameObject);
        }
    }
}
