using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour {
	
	[SerializeField]
	private float speed;

    private Rigidbody2D myRigidBody;

	private Vector2 direction;

	// Use this for initialization
	void Start () {
		
		myRigidBody = GetComponent<Rigidbody2D> ();
        StartCoroutine(Destroy());
	}

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3);
        //Destroy(gameObject);
        StartCoroutine(Fadeout());
    }

    private IEnumerator Fadeout()
    {
        float startAlpha = GetComponent<SpriteRenderer>().color.a;
        float rate = 1.0f / 1f;

        float progress = 0.0f;


        while (progress < 1.0)
        {
            Color tmpColor = GetComponent<SpriteRenderer>().color;

            GetComponent<SpriteRenderer>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));

            progress += rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }

    void FixedUpdate()	{
		myRigidBody.velocity = direction * speed;
	}

	public void Initialize(Vector2 direction)
	{
		this.direction = direction;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnBecameInvisible()
	{
		//Destroy (gameObject);
	}
}
