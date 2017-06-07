using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour {

    [SerializeField]
    private string targetTag;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == targetTag)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
