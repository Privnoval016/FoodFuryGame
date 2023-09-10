using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Item stats;
    public Vector3 initialPosition;
    private bool isRespawning = false;
    private Rigidbody rb;
    private BoxCollider bc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;

        if (!gameObject.name.Contains("(Clone)"))
        {
            initialPosition = transform.position;
        }
    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isRespawning && (collision.gameObject.tag == "Customer" || collision.gameObject.tag == "Floor"))
        {
            isRespawning = true;
            rb.isKinematic = true;

            StartCoroutine(RespawnAfterDelay(10f));
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        // move obj away
        transform.position = new Vector3(0, -10, 0);

        yield return new WaitForSeconds(delay);

        // reset position
        transform.position = initialPosition;
        rb.useGravity = false;
        rb.isKinematic = false;
        bc.isTrigger = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        isRespawning = false;
    }
}