using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;


    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public Joystick joystick;

    [HideInInspector] public bool isMoving;

    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 3f;
    float nextAttackTime = 0f;
    float gravityScaleAtStart;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeet;

    public Health_Bar health_Bar;
    public int maxHealth = 100;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        health_Bar.SetMaxHealth(maxHealth);
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        
    }

    void Update()
    {
        Walk();
        Jump();
        FlipSprite();
    }



    //--------------------Attack---------------------//
    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);


        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy_Controller>().TakeDamage(attackDamage);
            enemy.GetComponent<Crate_Break>().TakeDamage(attackDamage);
        }
    }
    public void AttackButton()
    {
        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;   
        }
    }


    //--------------------Walk---------------------//
    private void Walk()
    {
        float controlThrow = joystick.Horizontal;
        //float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Walk", playerHasHorizontalSpeed);


        isMoving = playerHasHorizontalSpeed;
    }


   


    //---------------------------Jump-----------------------------//
    public void Jump()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetBool("Jump", playerHasVerticalSpeed);
        }

        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        /*if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
            myAnimator.SetBool("Jump", playerHasVerticalSpeed);
        }*/
    }

    public void JumpButton()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
        myRigidBody.velocity += jumpVelocityToAdd;
        myAnimator.SetBool("Jump", playerHasVerticalSpeed);
    }



    //--------------------TakeDamage/Death---------------------//
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Death();
        }
        health_Bar.SetHealth(currentHealth);
    }

    void Death()
    {
        Debug.Log("Player died!");
        animator.SetBool("Death", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        Destroy(gameObject, 1);
    }
    //----------------------------------------------------------//

    void OnDrawGizmosSelected()
    {

        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }


    public void AddHealth(int extraHealth)
    {
        currentHealth += extraHealth;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
        //UpdateView();
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }
    }
}

