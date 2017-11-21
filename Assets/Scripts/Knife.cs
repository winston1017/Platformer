using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Knife : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField]
    private float fadeTime;

    [SerializeField]
    private bool doFade;

    [SerializeField]
    private float waitTime;

    private Rigidbody2D myRigidBody;

    private Vector2 direction;

    // Use this for initialization
    void Start()
    {

        myRigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(waitTime);
        //
        if (doFade)
        {
            StartCoroutine(Fadeout());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Fadeout()
    {
        float startAlpha = GetComponent<SpriteRenderer>().color.a;
        float rate = 1.0f / fadeTime;
        float progress = 0.0f; // edit this in the future

        while (progress < 1.0)
        {
            Color tmpColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));
            progress += rate * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        myRigidBody.velocity = direction * speed;
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }
    

    }
