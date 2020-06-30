using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoors : MonoBehaviour
{
    public Transform enemyPos;
    public Transform playerPos;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsPlayer;
    public float attackRange;
    public float attackRange2;

    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;

        //set the gameobjects to false until player passes thorugh them
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
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

        EnemyStillAlive();
        PlayerInRoom();
    }

    public void DisplayDialog()
    {
        dialogBox.SetActive(true);
        timerDisplay = displayTime;
    }

    //detects if enemies is in the room, if they are leave doors closed
    private void EnemyStillAlive()
    {
        //sphere to detect player
        Collider2D[] EnemiesToDamage = Physics2D.OverlapCircleAll(enemyPos.position, attackRange, whatIsEnemy);
        for (int i = 0; i < EnemiesToDamage.Length; i++)
        {
            
        }
       
        if (EnemiesToDamage.Length <= 0)
        {
            Destroy(gameObject);
        }


    }

    //detects if player is in the room, if they are, turn on doors
    private void PlayerInRoom()
    {
        //sphere to detect player
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(playerPos.position, attackRange2, whatIsPlayer);
        for (int i = 0; i < playerToDamage.Length; i++)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        if(playerToDamage.Length <= 0)
        {
            
        }

    }
    //draw drange of circle
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyPos.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerPos.position, attackRange2);
    }
    //if player collides with door display the dialog
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            DisplayDialog();
        }
    }

}
