using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private HealthBarBehaviour HealthBar;
    [Header ("Abilities")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float jumpingPower = 10f;
    [SerializeField] private int airJumpsAllowed = 1;
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    [Header ("Attack")]
    [SerializeField] private ProjectileBehaviour ProjectilePrefab;
    [SerializeField] private Transform launchLocation;
    [Header ("IFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numOfFlashes;
    private SpriteRenderer sprite;

    private float enemiesKilled = 0f;
    private int jumpCount = 0;
    private float horizontal;
    private float lastDirection;
    private bool isFacingRight = true;
    private bool dead = false;
    private UIManager uiManager;

    
    private void Start() {
        health = maxHealth;
        HealthBar.SetHealth(health,maxHealth);
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        uiManager = FindObjectOfType<UIManager>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal != 0)
        {
            lastDirection = horizontal;
        }
        Flip();
        CheckLaunchRotation();

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if(Input.GetButtonDown("Jump") && jumpCount < airJumpsAllowed || Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpCount++;
            player.velocity = new Vector2(player.velocity.x, jumpingPower);
        }

        if(Input.GetButtonUp("Jump") && player.velocity.y > 0f)
        {
            player.velocity = new Vector2(player.velocity.x, player.velocity.y * 0.5f);
        }

        if(Input.GetButtonDown("Fire3"))
        {
            Instantiate(ProjectilePrefab,launchLocation.position,launchLocation.rotation);
        }

        if(IsGrounded())
        {
            jumpCount = 0;
        }
    }

    private void FixedUpdate()
    {
        player.velocity = new Vector2(horizontal * speed, player.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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

    private void CheckLaunchRotation(){
        launchLocation.rotation = lastDirection < 0 ? Quaternion.Euler(0,180,0) : Quaternion.identity;
    }

    public void TakeHit(float damage)
    {
        health -= damage;
        HealthBar.SetHealth(health,maxHealth);
        if(health <= 0 )
        {
            if(!dead)
            {
            animator.SetTrigger("die");
            GetComponent<PlayerMovement>().enabled = false;
            uiManager.GameOver();
            dead = true;
            }
        }
        else
        {
            StartCoroutine(Invulnerability());
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision( 6, 8, true);
        for (int i = 0; i < numOfFlashes; i++)
        {
            sprite.color = new Color( 1, 0, 0, 0.75f);
            yield return new WaitForSeconds(iFramesDuration / (numOfFlashes * 2));
            sprite.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision( 6, 8, false);
    }

    public void KilledEnemy()
    {
        enemiesKilled++;
        if(enemiesKilled % 10 == 0)
        {
            health += 10;
            if(health > maxHealth) health = maxHealth;
            HealthBar.SetHealth(health,maxHealth);
        }
    }
}
