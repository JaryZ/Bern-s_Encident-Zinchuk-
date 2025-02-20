using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
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
            Death();
        }
    }

    void Death()
    {
        Debug.Log("Player died!");

        animator.SetBool("Death", true);


        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        //Destroy(gameObject, 1);

    }
}
