using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    enum State { Walk, Hit, Death, Daze}
    private State state;

    //speed and direciton the enemy is moving in
    public float defaultSpeed = 1.0f;
    public float speed;
    public float changeTime = 6.0f;
    float directionTimer;
    int direction = 1;

    //daze time when enemy gets hit
    public float dazeTime = 3.0f;
    float dazeTimer;
    bool daze;

    public HealthBar healthBar;
    public int maxHealth = 10;
    public int health;

    Rigidbody2D rb2D;
    Animator animator;

    //Attack range for the enemy sword
    public Transform attackPos;
    public LayerMask whatIsPlayer;
    public float attackRange;
    public int damage;

    //attack timer
    public float attackTime = 1.0f;
    float attackTimer;
    public bool attacked;

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
        //Enemy starts in walking state
        state = State.Walk;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Walk)
        {
            //timer for direction goes down
            directionTimer -= Time.deltaTime;
            if (directionTimer < 0)
            {
                //change direciton and set timer back to default
                direction = -direction;
                directionTimer = changeTime;
                transform.Rotate(0, 180, 0);
            }
        }

        if (state == State.Daze)
        {

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
                    state = State.Walk;
                }
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
        AttackPlayer();
    }

    private void FixedUpdate()
    {
        if (state == State.Walk)
        {
            //gets the position the enemy is in at the moment
            Vector2 position = transform.position;

            //sets the horizontal position of the player
            position.x = position.x + Time.deltaTime * speed * direction;
            //sets the direction the player will be looking in
            animator.SetFloat("Look X", direction);
            animator.SetFloat("Look Y", 0);
            //moves the player to those position
            rb2D.MovePosition(position);
        }
    }

    public void TakeDamage(int damage)
    {
        //since they take damange, set the state to being hit
        state = State.Hit;

        if (state == State.Hit)
        {
            //set daze to true when hit
            daze = true;
            //set timer to default
            dazeTimer = dazeTime;
            //play the hit animation
            animator.SetTrigger("Hit");
            //if health is greater than 0 than remove hp
            if (damage < 0)
            {
               
                //set daze to true when hit
                daze = true;
                //set timer to default
                dazeTimer = dazeTime;
                //play the hit animation
                animator.SetTrigger("Hit");
            }
            
            //set the health and healthbar ui
            health = Mathf.Clamp(health + damage, 0, maxHealth);
            healthBar.setHealth(health);

            //if health is less than or equal to 0 play the death animation and set rigid body off
            if (health <= 0)
            {
                animator.SetTrigger("Death");
                rb2D.simulated = false;
                healthBar.gameObject.SetActive(false);
            }
            state = State.Daze;
        }

        if (state == State.Daze)
        {
            if (daze)
            {
                //when dazed, set speed to 0 and play the dile animation
                speed = 0;
                animator.SetBool("idle", true);
                
            }
        }
    }

    //if player touchs enemy, they take damange
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if(player != null)
        {
            player.TakeDamage(-1);
        }
    }

    //if player stays in enemy hitbox, they continue to take damage
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if(player !=null)
        {
            player.TakeDamage(-2);
        }
    }
    
    //sphere collider to attack the enemy is they get into range
    private void AttackPlayer()
    {
        //sphere to detect player
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsPlayer);
        for(int i = 0; i < playerToDamage.Length; i++)
        {
            //if enemy already attacked, dont do anything
            if (attacked)
                return;
            //set attack to true, set timer back to default and remove hp from player
            attacked = true;
            attackTimer = attackTime;
            playerToDamage[i].GetComponent<PlayerController>().TakeDamage(-damage);
            animator.SetTrigger("Attack");
            //set speed of enemy to 0 and change bool to idle after attacking
            speed = 0;
            animator.SetBool("idle", true);
            directionTimer = changeTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}


