using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;


public class RangedEnemyController : MonoBehaviour
{

    //speed and direciton the enemy is moving in
    public float defaultSpeed = 1.0f;
    public float speed = 1.0f;
    public float changeTime = 6.0f;
    float directionTimer;
    public int direction = 1;
    bool vertical;

    //daze time when enemy gets hit
    public float dazeTime = 0.75f;
    public float dazeTimer;
    public bool daze;

    public HealthBar healthBar;
    public int maxHealth = 10;
    public int health;

    Rigidbody2D rb2D;
    Animator animator;

    //Attack range for the enemy sword
    public LayerMask whatIsPlayer;
    public int damage = 5;

    //attack timer
    public float attackTime = 3.0f;
    float attackTimer;
    public bool attacked;

    //target and how far to stop from target
    private Transform target;
    public float stoppingDistance = 6f;
    public float lookingDistance = 8f;

    public bool dead;

    //objects enemy fires out
    public GameObject projectilePrefab;
    //creates blood splatters on hit
    public GameObject blood;

    public GameObject damagePopup;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //sets the timer to their respective times
        directionTimer = changeTime;
        dazeTimer = dazeTime;
        attackTimer = attackTime;
        //health of enemy character
        health = maxHealth;
        healthBar.SetMaxHealth(health);

        //set target to player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //timer for direction goes down
        directionTimer -= Time.deltaTime;
        if (directionTimer < 0)
        {
            //change direciton and set timer back to default
            //direction = -direction;
            direction = -direction;
            vertical = !vertical;
            directionTimer = changeTime;
        }


        if (daze)
        {
            //timer for daze goes down
            dazeTimer -= Time.deltaTime;

            if (dazeTimer < 0)
            {
                //since daze is no longer in effect change daze to false, set speed back to normal, and go to walking state
                daze = false;
                speed = defaultSpeed;
                animator.SetBool("idle", false);
            }
        }
  
        //if enemy has already attacked
        if (attacked)
        {
            //timer for attack goes down
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                //set attack to false, set enemy speed back to normal, and go back to walking animation
                attacked = false;
                speed = defaultSpeed;
                animator.SetBool("idle", false);

            }

        }

        //always look to attack player
        ChasePlayer();
    }

    private void FixedUpdate()
    {

        //gets the position the enemy is in at the moment
        Vector2 position = transform.position;

        //sets the horizontal position of the player
        if(!vertical)
            position.x = position.x + Time.deltaTime * speed * direction;
        else if(vertical)
            position.y = position.y + Time.deltaTime * speed * direction;
        //sets the direction the player will be looking in
        animator.SetFloat("Look X", direction);
        animator.SetFloat("Look Y", 0);
        //moves the player to those position
        rb2D.MovePosition(position);

    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            damagePopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            Instantiate(damagePopup, rb2D.position, Quaternion.identity);
            //spawns blood when hit
            Instantiate(blood, rb2D.position, Quaternion.identity);
            //set daze to true when hit
            daze = true;
            //set timer to default
            dazeTimer = dazeTime;
            //play the hit animation
            animator.SetTrigger("hit");
        }

        //if enemy is dazed, set their speed to 0 and play their idle animation
        if (daze)
        {
            speed = 0;
            animator.SetBool("idle", true);
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

    //if player touchs enemy, they take damange
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if(player != null)
        {
            player.TakeDamage(-5);
        }
        if(collision.gameObject.name == "Tilemap")
        {
            direction = -direction;
        }
        
    }

    //if player stays in enemy hitbox, they continue to take damage
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if(player !=null)
        {
            player.TakeDamage(-10);
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
        animator.SetTrigger("Attack");
        GameObject projectileObject = Instantiate(projectilePrefab, rb2D.position, Quaternion.identity);
        //get the compoents of the projectile
        EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();
        //attack direction is the player
        Vector2 attackDireciton = (target.transform.position - transform.position).normalized;
        projectile.Launch(attackDireciton, 200);
        //once attacked set speed to 0 and idle animation
        speed = 0;
        animator.SetBool("idle", true);
        directionTimer = changeTime;
    }

  

    private void ChasePlayer()
    {
       
        //if the target distance is less than 3
        if (Vector2.Distance(transform.position, target.position) < lookingDistance)
        {
            directionTimer = changeTime;
            if (daze)
                return;
            if (dead)
                return;
            
            //set the aniamtions for chasing to the correct side
            if (target.position.x < transform.position.x)
            {
                direction = -1;
                animator.SetFloat("Look X", direction);

            }
            else if(target.position.x > transform.position.x)
            {
                direction = 1;
                animator.SetFloat("Look X", direction);
            }
            //move towards the player
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //set direction timer back to default so enemy doesnt look back and forth
            //if distance is close enough to player, attack player 
            if (Vector2.Distance(transform.position, target.position) < stoppingDistance)
            {
                AttackPlayer();
            }
        }
 
    }

    //if enemy is dead, turn most of its componenets off
    private void Dead()
    {
        //if enemy died, turn on most of their componenets
        dead = true;
        speed = 0;
        animator.SetTrigger("Death");
        rb2D.simulated = false;
        healthBar.gameObject.SetActive(false);

    }
}


