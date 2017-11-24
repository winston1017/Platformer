using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private static CameraFollow instance;
    public static CameraFollow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraFollow>();
            }

            return instance;
        }
    }
    [SerializeField]
	private float xMax;
	[SerializeField]
	private float yMax;
	[SerializeField]
	private float xMin;
	[SerializeField]
	private float yMin;

	private Transform target;

    public float XMax
    {
        get
        {
            return xMax;
        }

        set
        {
            xMax = value;
        }
    }

    public float YMax
    {
        get
        {
            return yMax;
        }

        set
        {
            yMax = value;
        }
    }

    public float XMin
    {
        get
        {
            return xMin;
        }

        set
        {
            xMin = value;
        }
    }

    public float YMin
    {
        get
        {
            return yMin;
        }

        set
        {
            yMin = value;
        }
    }

    // Use this for initialization
    void Start () {
		target = GameObject.Find ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {

        target = GameObject.Find("Player").transform;
        transform.position = new Vector3 (Mathf.Clamp (target.position.x, XMin, XMax), Mathf.Clamp (target.position.y, YMin, YMax), transform.position.z);

	}
}
