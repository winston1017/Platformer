using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreTag : MonoBehaviour {

    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 5, true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin" || other.gameObject.tag == "Crate")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);
        }
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
    //    {
    //        Physics2D.IgnoreCollision(platformCollider, other, false);
    //    }
    //}


    //private void IgnoreByTag (GameObject coin)
    //{
    //    if (coin.tag == "Coin")
    //    {
    //        Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    //    }
    //}





}
