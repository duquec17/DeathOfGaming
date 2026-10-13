using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_V2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; 
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Animator animator;
    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f;

    public Transform Aim;
    bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseController.IsGamePaused)
        {
            if (rb.velocity != Vector2.zero) 
            {
                rb.velocity = Vector2.zero; // Stop movement when paused
                StopMovementAnimations();
                StopFootsteps(); // Stops the audio
            }
            return;
        }

        // Apply speed to direction
        rb.velocity = moveInput * moveSpeed;
        animator.SetBool("isWalking", rb.velocity.magnitude > 0);

        if(rb.velocity.magnitude > 0 && !playingFootsteps)
        {
            StartFootsteps();
        }
        else if(rb.velocity.magnitude == 0)
        {
            StopFootsteps();
        }

        // Used to for deciding direction for melee combat aim while moving
        if(isWalking && moveInput.sqrMagnitude > 0.001f)
        {
            Vector3 vector3 = Vector3.left * moveInput.x + Vector3.down * moveInput.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }


    public void Move(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
            moveInput = Vector2.zero;
            isWalking = false;
            StopMovementAnimations();
            
            // Restore the last facing direction
            float lastMoveInputX = animator.GetFloat("LastInputX");
            float lastMoveInputY = animator.GetFloat("LastInputY");

            Vector3 vector3 = Vector3.left * lastMoveInputX + Vector3.down * lastMoveInputY;
            if(vector3.sqrMagnitude > 0.001f)
            {
                Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
            }

            Debug.Log("last direction X: " + moveInput.x + "last direction Y: " + moveInput.y);
            return;   
        }

        moveInput = context.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.001f)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            isWalking = true;
        }
    }

    public void AttackLight(InputAction.CallbackContext context)
    {
        animator.SetTrigger("LightAttack");
    }

    public void AttackHeavy(InputAction.CallbackContext context) 
    {
        animator.SetTrigger("HeavyAttack");
    }
    
    void StopMovementAnimations()
    {
        animator.SetBool("isWalking", false);
        animator.SetFloat("LastInputX", moveInput.x); // Helps determine which direction Player should be facing based on last input
        animator.SetFloat("LastInputY", moveInput.y);
    }

    void StartFootsteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);
    }

    void StopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstep));
    }

    void PlayFootstep()
    {
        SoundEffectManager.Play("Footstep", true);
    }
}
