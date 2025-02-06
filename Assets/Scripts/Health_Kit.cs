using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Kit : MonoBehaviour
{
    PlayerController playerController;
    public int healthPoints = 10;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    void OnCollisionEnter2D(Collision2D kit)
    {
        if (kit.gameObject.CompareTag("Player"))
        {
            playerController.AddHealth(healthPoints);
            Destroy(gameObject);
        }
    }
}
