using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public Transform playerPos;
    public LayerMask whatIsPlayer;
    public float attackRange;

    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;
    BoxCollider2D bc2D;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        bc2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
        PlayerInRoom();
    }

    //displays the dialog
    public void DisplayDialog()
    {
        dialogBox.SetActive(true);
        timerDisplay = displayTime;
    }

    //if player collides with door and has no key, display dialog, if they have a key destroy door
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            if (player.Haskey() == true)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                bc2D.enabled = false;
                player.hasKey = false;
            }
            else
            {
                DisplayDialog();
            }
        }
    }

    private void PlayerInRoom()
    {
        //sphere to detect player
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(playerPos.position, attackRange, whatIsPlayer);
        for (int i = 0; i < playerToDamage.Length; i++)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            bc2D.enabled = true;
            Destroy(dialogBox);
        }
        if (playerToDamage.Length <= 0)
        {
           
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerPos.position, attackRange);
    }
}
