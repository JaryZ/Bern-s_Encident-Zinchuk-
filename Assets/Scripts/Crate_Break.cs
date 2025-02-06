using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_Break : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 50;
    int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Break();

            /*for (var i = 0; i < 10; i++)
            {
                Instantiate(Coin, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            }*/
        }
    }

    void Break()
    {
        Debug.Log("Crate Break!");

        animator.SetBool("Break", true);


        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;


        Destroy(gameObject, 5);
    }
}
