using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;

    string[] powerUps = { "Triple Arrow"};

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
        dialogBox.SetActive(true);
        timerDisplay = displayTime;
    }

    public string randomPowerUp()
    {
        int index = Random.Range(0, powerUps.Length);
        if(powerUps[index] == "Triple Arrow")

        Debug.LogError(powerUps[index]);
        return powerUps[index];
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            if(randomPowerUp() == "Triple Arrow")
            {
                player.tripleArrow = true;
            }
        }
        //Destroy(gameObject);
    }
}
