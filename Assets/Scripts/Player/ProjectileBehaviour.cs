using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    public float speed = 14f;
    public float projectileDamage = 5;
    private float horizontal;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;

    private void Start() {
        this.name = "Projectile";
        player = GameObject.Find("Player").GetComponent<Transform>();
        horizontal = player.localScale.x < 0 ? -1 : 1;
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if(Mathf.Abs(rb.position.x - player.position.x) > 15)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<EnemyMovement>();
        if (enemy) enemy.TakeHit(projectileDamage);
        Destroy(gameObject);
    }
}
