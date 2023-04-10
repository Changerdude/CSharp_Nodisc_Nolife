using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float attackDamage = 1;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float jumpingPower = 10f;
    private bool isFacingRight = true;
    private float prevEnemyXVel;
    [SerializeField] private HealthBarBehaviour HealthBar;
    private GameObject Player;
    private PlayerMovement playerM;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    private bool dead = false;

    private void Start()
    {
        health = maxHealth;
        this.name = "Enemy";
        Player = GameObject.Find("Player");
        playerRB = Player.GetComponent<Rigidbody2D>();
        playerM = Player.GetComponent<PlayerMovement>();
        HealthBar.SetHealth(health, maxHealth);
    }
    
    // Update is called once per frame
    void Update()
    {
        DespawnCheck();
        if(rb.position.x > playerRB.position.x + .3)
        {
            horizontal = -1;
        }
        else if(rb.position.x < playerRB.position.x - .3)
        {
            horizontal = 1;
        }
        else
        {
            horizontal = 0;
        }
        Flip();

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if(rb.velocity.x == 0f && horizontal != 0 && prevEnemyXVel != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if(rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        prevEnemyXVel = rb.velocity.x;
    }

    private void Flip()
    {
        if( isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void TakeHit(float damage)
    {
        health -= damage;
        HealthBar.SetHealth(health, maxHealth);

        if(health <= 0 )
        {
            playerM.KilledEnemy();
            if(!dead)
            {
                GetComponent<EnemyMovement>().enabled = false;
                Physics2D.IgnoreCollision( Player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
                StartCoroutine(Die());
                dead = true;
            }
        }
    }
    //Attack Player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<PlayerMovement>();
        if (player) player.TakeHit(attackDamage);
    }

    private void DespawnCheck()
    {
        if(rb.position.y < playerRB.position.y - 20 || rb.position.x < playerRB.position.x - 12)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("die");
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
