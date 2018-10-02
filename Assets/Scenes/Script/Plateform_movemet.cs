using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform_movemet : PlateformBase
{

    [SerializeField]
    private GameObject startPoint;

    [SerializeField]
    private GameObject endPoint;

    [SerializeField]
    private float timeToTravel;

    [SerializeField]
    private float t;

    [SerializeField]
    private bool back;

    private Vector2 startCoord;
    private Vector2 endCoord;
    

    // Use this for initialization
    void Start ()
    {
        Debug.Assert(startPoint != null);
        startCoord = startPoint.transform.position;
        Debug.Assert(endPoint != null);
        endCoord = endPoint.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        t = t + 1 / timeToTravel * (back ? -1 : 1) * Time.deltaTime;
        if (t >= 1) back = true;
        else if (t <= 0) back = false;
        Vector2 nextPosition = startCoord * (1 - t) + endCoord * t;
        velocity = nextPosition - (Vector2)gameObject.transform.position;
        gameObject.transform.position = nextPosition;
	}

    public override Vector2 GetSpeed()
    {
        return velocity;
    }

}
