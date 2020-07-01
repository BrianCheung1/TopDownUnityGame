using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillCooldown : MonoBehaviour
{
    PlayerController player;
    public GameObject specialSkill;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if(player.specialReloadTimer <= 0 || player.specialReloadTimer == player.specialReloadTime)
        {
            transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
        }
    }
}
