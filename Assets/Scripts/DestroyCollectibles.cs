using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollectibles : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y <= -14f)
        {
            Destroy(gameObject);
        }
    }
}

