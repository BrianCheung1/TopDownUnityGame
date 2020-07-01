using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float specialReloadTimer;

    //invincible time of the player
    public float invincibleTime = 1.0f;
    public bool invincible;
    float invincibleTimer;

    //key to doors
    public bool hasKey = false;

    //creates blood splatters on hit
    public GameObject blood;

    public GameObject damagePopup;

    public GameObject deathPanel;

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

        projectilePrefab.GetComponent<Projectile>().defaultDamage();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused)
            return;
        //gets the inputs from the player
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //
        Vector2 move = new Vector2(horizontal, vertical);

        //if the player x || y does not equal 0, it means they are moving
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            //set their directions
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            //lookDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
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

        /*if (Input.GetMouseButtonDown(0))
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rb2D.position, Quaternion.identity);
            //get the compoents of the projectile
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            lookDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            projectile.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg);
            //launch the projectiles in the direciton the player is looking in for 300 newton force
            projectile.Launch(lookDirection, 500);
        }*/

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

        //chest dialouge
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                    TreasureChest chest = hit.collider.GetComponent<TreasureChest>();
                    if (chest != null)
                    {
                        chest.DisplayDialog();
                    }
            }
        }

        //reloads the scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
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

            damagePopup.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            Instantiate(damagePopup, rb2D.position, Quaternion.identity);
           

            Instantiate(blood, rb2D.position, Quaternion.identity);
            //if they take damage set invincible to true
            invincible = true;
            //set the timer back to default
            invincibleTimer = invincibleTime;
            //play hit animation
            //animator.SetTrigger("Hit");
        }

        //set current health between 0, 100 depedning on the amount of damage taking
        currentHealth = Mathf.Clamp(currentHealth + damage, 0, maxHealth);
        healthBar.setHealth(currentHealth);

        if (currentHealth <= 0)
        {
            deathPanel.SetActive(true);
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
            
            for (int i = 0; i < 3; i++)
            {
                //creates the projectileObject a little above the character model, this ensure that projectile comes out of the hands rather than the feet
                GameObject projectileObject = Instantiate(specialProjectilePrefab, rb2D.position, Quaternion.identity);
                //get the compoents of the projectile
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                //Depending on where the player is looking, rotate the arrow that comes out
                projectile.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg);
                if(i == 0)
                {
                    if(lookDirection.x >= 0 || lookDirection.x <= 0)
                    {
                        projectile.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, 0);
                    }
                    if (lookDirection.y == 1 || lookDirection.y == -1)
                    {
                        projectile.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, 0);
                    }
                    
                }
                if (i == 1)
                {
                    if (lookDirection.x >= 0 || lookDirection.x <= 0)
                    {
                        projectile.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, 0);
                    }
                    if (lookDirection.y == 1 || lookDirection.y == -1)
                    {
                        projectile.transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, 0);
                    }
                }
                if (i == 2)
                {
                    projectile.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                }

                //launch the projectiles in the direciton the player is looking in for 300 newton force
                projectile.Launch(lookDirection, 500);
            }
            specialReloadTimer = specialReloadTime;
            specialReloading = true;
        }
    }

    public bool Haskey()
    {
        return hasKey;
    }

}
