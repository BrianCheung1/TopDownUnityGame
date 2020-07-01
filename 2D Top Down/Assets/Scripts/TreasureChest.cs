using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;
    public Projectile projectile;
    bool opened = false;

    int projectileDamageUpgrade = 1;
    
    //set dialouge to false
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;

    }

    //timer for chest dialouge
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
    }

    //displays chest dialog
    public void DisplayDialog()
    {
        if (!opened)
        {
            dialogBox.SetActive(true);
            timerDisplay = displayTime;
            projectile.setDamage(projectileDamageUpgrade);
            opened = true;
        }
    }


    //if player touches the chest, they get the powerup
    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            
        }
    }
}
