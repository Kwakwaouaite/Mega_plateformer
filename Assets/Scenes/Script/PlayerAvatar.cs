﻿using System.Collections;
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

    private bool isJumping;
    private float verticalAcceleration;

    private Vector2 position;
    private Vector2 boxSize;
    private Collider2D playerCollider;
    
    void Start () {
        position = gameObject.transform.position;
        isJumping = false;
        boxSize = gameObject.GetComponent<BoxCollider2D>().size;
        playerCollider = gameObject.GetComponent<Collider2D>();

        nearObjectDown = -1;
        nearObjectUp = -1;
        nearObjectRight = -1;
        nearObjectLeft = -1;
    }
	
    void ApplyGravity()
    {
        Debug.Log(nearObjectDown);
        if (nearObjectDown >= 0)
        {
            verticalAcceleration = 0;
            isJumping = false;
        }
        else
            verticalAcceleration = Mathf.Max(-maxDownSpeed, verticalAcceleration - Time.deltaTime * downSpeedPerSec);
    }
    void Jump ()
    {
        if (/*!isJumping && */Input.GetButtonDown("Jump"))
        {
            verticalAcceleration = jumpImpulse;
            isJumping = true;
            Debug.Log("jump" + verticalAcceleration);
        }
    }

    void Mouvement()
    {
        Jump();
        ApplyGravity();
        Debug.Log(verticalAcceleration);
        position = position + new Vector2(Input.GetAxis("Horizontal") * speed, verticalAcceleration);
        transform.position = position;
        
    }
	// Update is called once per frame

    void updateCollisions()
    {
        RaycastHit2D hitUpRight = Physics2D.Raycast(position + boxSize / 2, Vector2.up, jumpImpulse);
        RaycastHit2D hitUpLeft = Physics2D.Raycast(position + new Vector2(-1, 1) * boxSize / 2, Vector2.up, jumpImpulse);
        RaycastHit2D hitLeftUp = Physics2D.Raycast(position + new Vector2(-1, 1) * boxSize / 2, Vector2.left, speed);
        RaycastHit2D hitLeftDown = Physics2D.Raycast(position - boxSize / 2, Vector2.left, speed);
        RaycastHit2D hitDownRight = Physics2D.Raycast(position + new Vector2(1, -1) * boxSize / 2, Vector2.down, maxDownSpeed);
        RaycastHit2D hitDownLeft = Physics2D.Raycast(position - boxSize / 2, Vector2.down, maxDownSpeed);
        RaycastHit2D hitRightUp = Physics2D.Raycast(position + boxSize / 2, Vector2.right, speed);
        RaycastHit2D hitRightDown = Physics2D.Raycast(position + new Vector2(1, -1) * boxSize / 2, Vector2.right, speed);

        /*if (hitUpRight.collider != null || hitUpLeft.collider != null)
            nearObjectUp = Mathf.Max(hitUpRight.distance, hitUpLeft.distance);
        else
            nearObjectUp = -1;

        if (hitRightUp.collider != null || hitRightDown.collider != null)
            nearObjectRight = Mathf.Max(hitRightUp.distance, hitRightDown.distance);
        else
            nearObjectRight = -1;

        if (hitLeftUp.collider != null || hitLeftDown.collider != null)
            nearObjectLeft = Mathf.Max(hitUpRight.distance, hitUpLeft.distance);
        else
            nearObjectLeft = -1;*/

        if (hitDownRight.collider != null || hitDownLeft.collider != null)
        {
            nearObjectDown = Mathf.Max(hitDownRight.distance, hitDownLeft.distance);
            Debug.Log(hitDownRight.distance);
            Debug.Log(hitDownLeft.distance);
        }
        else
            nearObjectDown = -1;
    }
    /*void OnCollisionStay2D(Collision2D other)
    {
        nearObjectDown = 0;
        nearObjectUp = 0;
        nearObjectRight = 0;
        nearObjectLeft = 0;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        nearObjectDown = -1;
        nearObjectUp = -1;
        nearObjectRight = -1;
        nearObjectLeft = -1;
    }*/
    void Update () {
        updateCollisions();
        Mouvement();
	}
}
