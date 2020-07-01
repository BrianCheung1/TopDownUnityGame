using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    Animator animator;

    public int trapDamage = 10;
    public bool triggered = true;
    float triggerTime;
    float triggerTimer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        triggerTime = Random.Range(3.0f, 9.0f);
        if (triggered)
        {
            triggerTimer -= Time.deltaTime;
            if (triggerTimer <= 0)
            {
                animator.SetTrigger("Triggered");
                GetComponent<BoxCollider2D>().enabled = true;
                triggerTimer = triggerTime;
                Invoke("EnableTraps", 1f);
            }
        }

    }

    //trigger traps if player enters
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            //make player take damage of traps
            player.TakeDamage(-trapDamage);
        }

    }

    private void EnableTraps()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
