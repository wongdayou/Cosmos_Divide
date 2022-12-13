using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public EntityData data;
    
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) {
            Debug.LogError("Player does not have a rigidbody!");
        }
         
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb.AddForce(transform.up * data.speed);
        }

        if (Input.GetKey("a"))
        {
            this.transform.Rotate(Vector3.forward * data.rotationSpeed * Time.fixedDeltaTime);
        }

        if (Input.GetKey("s"))
        {
            rb.AddForce(-transform.up * data.speed);
        }

        if (Input.GetKey("d"))
        {
            this.transform.Rotate(Vector3.back * data.rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
