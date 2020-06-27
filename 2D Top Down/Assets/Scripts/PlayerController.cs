using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb2D;
    public GameObject projectilePrefab;
    public GameObject specialProjectilePrefab;

    //direction the player is looking for
    Vector2 lookDirection = new Vector2(0, -1);
    float horizontal;
    float vertical;

    //stats of the player
    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public float speed = 3.0f;

    //reload time of the player
    public float reloadTime = 1.0f;
    bool reloading;
    float reloaderTimer;

    //reload time of skills
    public bool tripleArrow;
    public float specialReloadTime = 5.0f;
    bool specialReloading;
    float specialReloadTimer;

    //invincible time of the player
    public float invincibleTime = 1.0f;
    public bool invincible;
    float invincibleTimer;

    

    // Start is called before the first frame update
    void Start()
    {


        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        reloaderTimer = reloadTime;
        specialReloadTimer = specialReloadTime;
        invincibleTimer = invincibleTime;
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

        //reload timer
        if (reloading)
        {
            reloaderTimer -= Time.deltaTime;
            if (reloaderTimer < 0)
            {
                reloading = false;
            }
        }

        //special skill cooldown
        if (specialReloading)
        {
            specialReloadTimer -= Time.deltaTime;
            if(specialReloadTimer < 0)
            {
                specialReloading = false;
            }
        }

        //attack key
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Attack();
        }

        //invincible timer
        if (invincible)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0)
            {
                invincible = false;
            }
        }

        //special dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SpecialAttack();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                    TreasureChest chest = hit.collider.GetComponent<TreasureChest>();
                    if (chest != null)
                    {
                        Debug.LogError("test");
                        chest.DisplayDialog();
                    }
            }
        }
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

    private void Attack()
    {
      
        //if player is reloading dont do anything
        if (reloading)
            return;

        //play the launch aniamtions
        animator.SetTrigger("Attack");
        //creates the projectileObject a little above the character model, this ensure that projectile comes out of the hands rather than the feet
        GameObject projectileObject = Instantiate(projectilePrefab, rb2D.position, Quaternion.identity);
        //get the compoents of the projectile
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        //Depending on where the player is looking, rotate the arrow that comes out
       
        projectile.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg);

        //launch the projectiles in the direciton the player is looking in for 300 newton force
        projectile.Launch(lookDirection, 500);
        //when arrow is fired, set reload to true, and set reload timer to default
        reloading = true;
        reloaderTimer = reloadTime;

    }

    //changes players health depending on the damage they take
    public void TakeDamage(int damage)
    {
        if(damage < 0)
        {
            if (invincible)
                return;
            //if they take damage set invincible to true
            invincible = true;
            //set the timer back to default
            invincibleTimer = invincibleTime;
            //play hit animation
            animator.SetTrigger("Hit");
        }

        //set current health between 0, 100 depedning on the amount of damage taking
        currentHealth = Mathf.Clamp(currentHealth + damage, 0, maxHealth);
        healthBar.setHealth(currentHealth);

        if (currentHealth <= 0)
        {
            transform.position = new Vector2(-2.69f, -0.56f);
            currentHealth = maxHealth;
            healthBar.setHealth(currentHealth);
        }
    }

    private void SpecialAttack()
    {
        if (tripleArrow)
        {
            if (specialReloading)
                return;
            //play the launch aniamtions
            animator.SetTrigger("Attack");
            //creates the projectileObject a little above the character model, this ensure that projectile comes out of the hands rather than the feet
            GameObject projectileObject = Instantiate(specialProjectilePrefab, rb2D.position, Quaternion.identity);
            //get the compoents of the projectile
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            //Depending on where the player is looking, rotate the arrow that comes out
            projectile.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg);

            //launch the projectiles in the direciton the player is looking in for 300 newton force
            projectile.Launch(lookDirection, 500);

            specialReloadTimer = specialReloadTime;
            specialReloading = true;
        }
    }

}
