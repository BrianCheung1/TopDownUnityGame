using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorKey : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //set key to true in player script and destroy key object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            player.hasKey = true;
            Destroy(gameObject);
        }
    }
}
