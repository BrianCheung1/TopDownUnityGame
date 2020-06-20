using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    string[] powerUps = { "Triple Arrow"};
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
        Destroy(gameObject);
    }
}
