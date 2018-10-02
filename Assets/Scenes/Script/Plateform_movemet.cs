using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform_movemet : MonoBehaviour {

    [SerializeField]
    private GameObject startPoint;

    [SerializeField]
    private GameObject endPoint;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float t;

    [SerializeField]
    private bool back;

    private Vector2 startCoord;
    private Vector2 endCoord;

    private Vector2 velocity = Vector2.zero;
    

    // Use this for initialization
    void Start () {
        startCoord = startPoint.transform.position;
        Debug.Assert(startCoord != null);
        endCoord = endPoint.transform.position;
        Debug.Assert(endCoord != null);
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(t);
        Vector2 nextPosition = startCoord * (1 - t) + endCoord * t;
        velocity = nextPosition - (Vector2)gameObject.transform.position;
        Debug.Log(velocity);
        gameObject.transform.position = nextPosition;
        t = t + speed/100 * (back ? -1 : 1) * Time.deltaTime ;
        if (t >= 1) back = true;
        else if (t <= 0) back  = false;
	}

    Vector2 GetSpeed()
    {
        return velocity;
    }

}
