using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour {
    public Item stats;

    private void Awake()
    {
        GetComponent<Rigidbody>().useGravity = false;
        stats.damage = 1;
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Customer" || collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }
}
