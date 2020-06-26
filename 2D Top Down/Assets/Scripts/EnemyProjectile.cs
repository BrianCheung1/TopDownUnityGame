using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb2D;
    public int projectileDamage = 2;

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Destroys the projectile if they go too far away from the start position
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    //moves the projection in a certian direction and force that is used in the playercontroller script
    public void Launch(Vector2 direction, float force)
    {
        rb2D.AddForce(direction * force);

    }



    //if the projectile collides with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(-projectileDamage);
        }
        //destory the game objects after they collided
        Destroy(gameObject);
    }
}

