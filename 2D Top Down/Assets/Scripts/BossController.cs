using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public LayerMask whatIsPlayer;
    public int damage = 5;

    private Transform target;
    public float stoppingDistance = 0.8f;
    public float lookingDistance = 6f;

    Animator animator;
    Rigidbody2D rb2D;
    public GameObject projectilePrefab;
    public GameObject blood;

    //attack timer
    public float attackTime = 1.0f;
    float attackTimer;
    public bool attacked;

    public int health;
    public int maxHealth = 100;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        //sets the timer to their respective times
        attackTimer = attackTime;

        health = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if enemy has already attacked
        if (attacked)
        {
            //timer for attack goes down
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                //set attack to false, set enemy speed back to normal, and go back to walking animation
                attacked = false;
                //animator.SetBool("idle", false);

            }

        }
        LookForPlayer();
    }

    private void LookForPlayer()
    {
        if (Vector2.Distance(transform.position, target.position) < lookingDistance)
        {
            Debug.Log("Player in sight");
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        //if enemy already attacked, dont do anything
        if (attacked)
            return;
        //set attack to true, set timer back to default and remove hp from player
        attacked = true;
        attackTimer = attackTime;
        //animator.SetTrigger("Attack");

        for (int rotate = -45; rotate <= 45; rotate += 15)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rb2D.position, Quaternion.identity);
            //get the compoents of the projectile
            EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();
            //attack direction is the player
            Vector2 attackDireciton = (target.transform.position - transform.position).normalized;

            if(rotate == -45)
            {
                attackDireciton = (target.transform.position - (transform.position + new Vector3(0, 2, 0))).normalized;
            }
            else if(rotate == -15)
            {
                attackDireciton = (target.transform.position - (transform.position + new Vector3(0, 1, 0))).normalized;
            }
            else if(rotate == 0)
            {
                attackDireciton = (target.transform.position - transform.position).normalized;
            }
            else if (rotate == 15)
            {
                attackDireciton = (target.transform.position - (transform.position + new Vector3(0, -1, 0))).normalized;
            }
            else if(rotate == 45)
            {
                attackDireciton = (target.transform.position - (transform.position + new Vector3(0, -2, 0))).normalized;
            }

            projectile.Launch(attackDireciton, 200);
        }
        //once attacked set speed to 0 and idle animation
        //animator.SetBool("idle", true);
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            //spawns blood when hit
            Instantiate(blood, rb2D.position, Quaternion.identity);
            //play the hit animation
        }


        //set the health and healthbar ui
        health = Mathf.Clamp(health + damage, 0, maxHealth);
        healthBar.setHealth(health);

        //if health is less than or equal to 0 play the death animation and set rigid body off
        if (health <= 0)
        {
            Dead();
        }

    }

    public void Dead()
    {

    }
}
