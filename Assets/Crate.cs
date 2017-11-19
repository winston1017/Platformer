using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Crate : MonoBehaviour
{

    private Rigidbody2D myRigidBody;
    private Vector2 startPos;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        startPos = myRigidBody.position;
    }

    void FixedUpdate()
    {
        if (transform.position.y <= -14f)
        {
            myRigidBody.position = startPos;
        }
    }
}
