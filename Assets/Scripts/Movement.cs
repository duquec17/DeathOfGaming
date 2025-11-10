using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /* Get player input
    *  Apply  movement to sprite
    */

    public float speed;
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector2(horizontal, vertical);

        transform.position += direction * speed * Time.deltaTime;


    }
}
