using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bird : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;

    public Animator animator;



    public Transform hitBox;
    public LayerMask playerLayer;



    public int attackDamage = 40;
    public float attackRange = 0.5f;
    public int maxHealth = 100;
    int currentHealth;

    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    
    void Update()
    {
        if (IsFacingRight()) 
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }



        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            Attack();
        }


        
    }

    

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(hitBox.position, attackRange, playerLayer);



        foreach (Collider2D player in hitPlayer)
        {
            //player.GetComponent<PlayerController>().TakeDamage(attackDamage);

        }

    }


    void OnDrawGizmosSelected()
    {

        if (hitBox == null)
            return;

        Gizmos.DrawWireSphere(hitBox.position, attackRange);
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
        Debug.Log("Enemy died!");

        animator.SetBool("Death", true);

        
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 1);
    }


    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }
}
