using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {

	public float jumpImpulse;
    public float speed;
    public float maxDownSpeed;
    public float downSpeedPerSec;
    public float innerDistBoxDetection = 0.99f;

    private float nearObjectUp;
    private float nearObjectDown;
    private float nearObjectRight;
    private float nearObjectLeft;

    private bool isJumping;
    private float verticalAcceleration;

    private Vector2 position;
    private Vector2 boxSize;

    private Vector2 upLeftCorner;
    private Vector2 upRightCorner;
    private Vector2 downRightCorner;
    private Vector2 downLeftCorner;
    
    void Start () {
        position = gameObject.transform.position;
        isJumping = false;
        boxSize = gameObject.GetComponent<SpriteRenderer>().size * innerDistBoxDetection;
        downLeftCorner = position - boxSize / 2;
        upRightCorner = position + boxSize / 2;
        downRightCorner = position + new Vector2(-1, 1) * boxSize / 2;
        upLeftCorner = position + new Vector2(1, -1) * boxSize / 2;

        Debug.Log("size" + boxSize);

        nearObjectDown = -1;
        nearObjectUp = -1;
        nearObjectRight = -1;
        nearObjectLeft = -1;
    }
	
    void ApplyGravity()
    {
        if (isJumping)
            verticalAcceleration = Mathf.Max(-maxDownSpeed, verticalAcceleration - Time.deltaTime * downSpeedPerSec);
    }
    void Jump ()
    {
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            verticalAcceleration = jumpImpulse;
            isJumping = true;
        }
    }

    void Mouvement()
    {
        Jump();
        ApplyGravity();
        Debug.Log(verticalAcceleration);
        Vector2 newPosition = position + new Vector2(Input.GetAxis("Horizontal") * speed, verticalAcceleration);

        if (nearObjectDown != -1 && newPosition.y < position.y - nearObjectDown)
        {
            newPosition.y = position.y - nearObjectDown;
            isJumping = false;
            verticalAcceleration = 0;
        }
        else if (nearObjectUp != -1 && newPosition.y > position.y + nearObjectUp)
        {
            newPosition.y = position.y + nearObjectUp;
            verticalAcceleration = 0;
        }

        if (nearObjectLeft != -1 && newPosition.x < position.x - nearObjectLeft)
            newPosition.x = position.x - nearObjectLeft;
        
        else if (nearObjectRight != -1 && newPosition.x > position.x + nearObjectRight)
            newPosition.x = position.x + nearObjectRight;

        transform.position = newPosition;
        position = newPosition;
        
    }
	// Update is called once per frame

        void DebugDrawRay(RaycastHit2D ray, Vector2 startingPoint)
    {
        if (ray.collider != null)
        {
            Debug.DrawLine(startingPoint, ray.point);
        }
    }
    
    void updateCollisions()
    {
        downLeftCorner = position - boxSize / 2;
        upRightCorner = position + boxSize / 2;
        downRightCorner = position + new Vector2(1, -1) * boxSize / 2;
        upLeftCorner = position + new Vector2(-1, 1) * boxSize / 2;

        RaycastHit2D hitRightUp = Physics2D.Raycast(upRightCorner, Vector2.right, 100*speed);
        RaycastHit2D hitUpRight = Physics2D.Raycast(upRightCorner, Vector2.up, 100*jumpImpulse);
        RaycastHit2D hitUpLeft = Physics2D.Raycast(upLeftCorner, Vector2.up, 100*jumpImpulse);
        RaycastHit2D hitLeftUp = Physics2D.Raycast(upLeftCorner, Vector2.left, 100*speed);
        RaycastHit2D hitRightDown = Physics2D.Raycast(downRightCorner, Vector2.right, 100*speed);
        RaycastHit2D hitDownRight = Physics2D.Raycast(downRightCorner, Vector2.down, 100*maxDownSpeed);
        RaycastHit2D hitLeftDown = Physics2D.Raycast(downLeftCorner, Vector2.left, 100*speed);
        RaycastHit2D hitDownLeft = Physics2D.Raycast(downLeftCorner, Vector2.down, 100*maxDownSpeed);

        DebugDrawRay(hitRightUp, upRightCorner);
        DebugDrawRay(hitUpRight, upRightCorner);
        DebugDrawRay(hitUpLeft, upLeftCorner);
        DebugDrawRay(hitLeftUp, upLeftCorner);
        DebugDrawRay(hitDownRight, downRightCorner);
        DebugDrawRay(hitRightDown, downRightCorner);
        DebugDrawRay(hitDownLeft, downLeftCorner);
        DebugDrawRay(hitLeftDown, downLeftCorner);

        if (hitUpRight.collider != null || hitUpLeft.collider != null)
            nearObjectUp = Mathf.Max(hitUpRight.distance, hitUpLeft.distance);
        else
            nearObjectUp = -1;

        if (hitRightUp.collider != null || hitRightDown.collider != null)
            nearObjectRight = Mathf.Max(hitRightUp.distance, hitRightDown.distance);
        else
            nearObjectRight = -1;

        if (hitLeftUp.collider != null || hitLeftDown.collider != null)
            nearObjectLeft = Mathf.Max(hitLeftUp.distance, hitLeftUp.distance);
        else
            nearObjectLeft = -1;

        if (hitDownRight.collider != null || hitDownLeft.collider != null)
        {
            nearObjectDown = Mathf.Max(hitDownRight.distance, hitDownLeft.distance);
        }
        else
            nearObjectDown = -1;
    }
    
    void Update () {
        updateCollisions();
        Mouvement();
        Debug.Log(isJumping);
	}
}
