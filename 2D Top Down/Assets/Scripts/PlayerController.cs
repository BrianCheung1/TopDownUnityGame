using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2D;

    Vector2 lookDirection = new Vector2(0, -1);
    float horizontal;
    float vertical;

    public int maxHealth = 100;
    int currentHealth;
    public float speed = 3.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //gets the inputs from the player
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //
        Vector2 move = new Vector2(horizontal, vertical);

        //if the player x || y does not equal 0, it means they are moving
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            //set their directions
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        //plays the animations in which the direciton the palyer is moving in
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
    }

    private void FixedUpdate()
    {
        //position of the player
        Vector2 position = rb2D.position;
        //move the player depending on the inputs
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        //move the player to those positions
        rb2D.MovePosition(position);
    }
}
