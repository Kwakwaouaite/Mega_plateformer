using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {

	public float jumpImpulse;
    public float speed;
    public float maxDownSpeed;
    public float downSpeedPerSec;
    private float nearObjectUp;
    private float nearObjectDown;
    private float nearObjectRight;
    private float nearObjectLeft;

    private Vector2 position;
    void Start () {
        position = gameObject.transform.position;
	}
	
    void Jump ()
    {

    }

    void Mouvement()
    {

    }
	// Update is called once per frame
	void Update () {
		
	}
}
