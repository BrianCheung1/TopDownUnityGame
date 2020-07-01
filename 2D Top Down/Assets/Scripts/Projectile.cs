using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb2D;
    public int projectileDamage = 2;
    public int defaultProjectileDamage = 2;

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
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        RangedEnemyController rangedEnemy = collision.gameObject.GetComponent<RangedEnemyController>();
        BossController boss = collision.gameObject.GetComponent<BossController>();
        if(enemy != null)
        {
            enemy.TakeDamage(-projectileDamage);

        }
        if(rangedEnemy != null)
        {
            rangedEnemy.TakeDamage(-projectileDamage);
        }
        if(boss != null)
        {
            boss.TakeDamage(-projectileDamage);
        }

        //destory the game objects after they collided
        Destroy(gameObject);
    }

    public void setDamage(int attack)
    {
        projectileDamage += attack;
    }

    public void defaultDamage()
    {
        projectileDamage = defaultProjectileDamage;
    }
}

