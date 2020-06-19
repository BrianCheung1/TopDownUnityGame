using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    Animator animator;

    public int trapDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //trigger traps if player enters
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            //make player take damage of traps
            player.TakeDamage(-trapDamage);
            //play trap animations
            animator.SetTrigger("Triggered");
        }

    }
}
