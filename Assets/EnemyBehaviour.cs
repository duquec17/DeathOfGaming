using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Goal: Enemy follows player and tackles player
    // - Additional: blows up after a delay

    // Base Variables
    public float moveSpeed = 2f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    float health, maxHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    private void Update()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction * moveSpeed;

            // float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            // rb.rotation = angle;


        }
    }

    private void FixedUpdate()
    {
        if(target)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }
}
