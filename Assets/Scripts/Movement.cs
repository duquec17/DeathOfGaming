using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /* Get player input
    *  Apply  movement to sprite
    */

    public float speed;
    public Animator animator;
    private Vector3 direction;

    private void Update()
    {
        // Gets direction player is moving in based on kep press
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Create normalized vector of the direction
        direction = new Vector3(horizontal, vertical, 0);

        AnimateMovement(direction);

    }

    private void FixedUpdate()
    {
        // Move the player
        this.transform.position += direction * speed * Time.deltaTime;
    }

    void AnimateMovement(Vector3 direction)
    {
        if(animator != null)
        {
            if(direction.magnitude > 0)
            {
                animator.SetBool("isMoving", true);

                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}
