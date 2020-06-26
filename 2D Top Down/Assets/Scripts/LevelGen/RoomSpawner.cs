using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 -- need right door

    private RoomTemplates templates;
    Rigidbody2D rb2D;

    private bool  spawned = false;
    private int rand;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    private void Spawn()
    {

        if (spawned == false)
        {

            if (openingDirection == 1)
            {
                //need to spawn a room with a bottom door
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], new Vector3(0, 5, 0), Quaternion.identity);
            }
            else if (openingDirection == 2)
            {
                //need to spawn a room with a top door
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], new Vector3(0, -5, 0), Quaternion.identity);
            }
            else if (openingDirection == 3)
            {
                //need to spawn a room with a left door
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], new Vector3(6, 0, 0), Quaternion.identity);
            }
            else if (openingDirection == 4)
            {
                //need to spawn a room with a right door
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], new Vector3(-6, 0, 0), Quaternion.identity);
            }
            spawned = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
