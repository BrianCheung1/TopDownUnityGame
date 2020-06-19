using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    enum State { Walk, Hit, Death, Daze}
    private State state;

    public float defaultSpeed = 1.0f;
    public float speed;
    public float changeTime = 6.0f;
    float directionTimer;
    int direction = 1;

    public float dazeTime = 3.0f;
    public float dazeTimer;
    bool daze;

    public float health = 5;

    Rigidbody2D rb2D;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //sets the timer to their respective times
        directionTimer = changeTime;
        dazeTimer = dazeTime;

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
            if (health > 0)
            {
                health -= damage;
            }
            //if health is less than or equal to 0 play the death animation and set rigid body off.
            if (health <= 0)
            {
                animator.SetTrigger("Death");
                rb2D.simulated = false;
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
}


