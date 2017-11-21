using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Crate : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Vector2 startPos;

    //current time
    private float time;

    //The time to spawn the object
    private float spawnTime = 10;

    // Use this for initialization
    void Start()
    {
        time = 0;
        myRigidBody = GetComponent<Rigidbody2D>();
        startPos = myRigidBody.position;
    }

    void FixedUpdate()
    {
        if (transform.position.y <= -14f)
        {
            time += Time.deltaTime;
            if (time >= spawnTime)
            {
                RespawnCrate();
            }
        }
    }

    void RespawnCrate()
    {
        time = 0;
        myRigidBody.position = startPos;
    }

}
