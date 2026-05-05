using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    /* Get player input
    *  Apply  movement to sprite
    */

    public float speed;
    public Animator animator;
    private Vector3 direction;

    public GameObject popUpBox;
    int currentScene = 0;
    bool popOnScreen;

    private void Update()
    {
        // Gets direction player is moving in based on kep press
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Create normalized vector of the direction
        direction = new Vector3(horizontal, vertical, 0);

        AnimateMovement(direction);

        if (Input.GetKeyDown(KeyCode.L) && popOnScreen == true && currentScene == 0)
        {
            SceneManager.LoadScene("StoneCometLevel");
            currentScene = 5;
        }
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
