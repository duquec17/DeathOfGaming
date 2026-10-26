using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Attack : MonoBehaviour
{
    // Variable List
    public GameObject Melee;
    private Animator animator;
    bool isAttacking = false;
    float atkDuration = 0.3f;
    float atkTimer = 0f;

    // Ranged variables
    public Transform Aim;
    public GameObject bullet;
    public float fireForce = 10f;
    float shootCooldown = 0.25f;
    float shootTimer = 0.5f;

    // Update is called once perframe
    private void Update()
    {
        // Check melee timer
        CheckMeleeTimer();
        shootTimer += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            // Melee
            OnAttack();
        }
        if (Input.GetMouseButton(1))
        {
            // Projectile
            OnShoot();

        }
    }

    public void OnShoot()
    {
        if (shootTimer > shootCooldown)
        {
            shootTimer = 0;
            GameObject intBullet = Instantiate(bullet, Aim.position, Aim.rotation);
            intBullet.GetComponent<Rigidbody2D>().AddForce(-Aim.up * fireForce, ForceMode2D.Impulse);
            Destroy(intBullet, 2f);
        }
    }

    public void OnAttack()
    {
        Melee.SetActive(true);
        isAttacking = true;
        // Call animator play melee attack
        Debug.Log("Is attacking:" + isAttacking);
    }

    void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if(atkTimer >= atkDuration)
            {
                atkTimer = 0;
                isAttacking= false;
                Melee.SetActive(false);
            }
        }
    }
}
