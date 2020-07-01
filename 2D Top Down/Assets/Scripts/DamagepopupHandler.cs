using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagepopupHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
        float randomLocation = Random.Range(-0.5f, 0.5f);
        transform.localPosition += new Vector3(randomLocation, randomLocation, 0);
    }

    
}
