using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {

	[SerializeField] private float jumpImpulse;
    [SerializeField] private float maxHorizontalspeed;
    [SerializeField] private float maxDownSpeed;
    [SerializeField] private float downSpeedPerSec;
    [SerializeField] private float innerDistBoxDetection = 0.99f;
    [SerializeField] private float wallJumpLatImpVal;
    [SerializeField] private float wallJumpDecreasePerSec;
    [SerializeField] private float percMidAirSpeedLost = 1;

    [SerializeField] private float percentageJumpImpLostWall = 1;

    private float nearObjectUp;
    private float nearObjectDown;
    private float nearObjectRight;
    private float nearObjectLeft;

    private bool isJumping;
    private float verticalSpeed;
    private float horizontalSpeed;
    private float wallJumpImpulse;

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


        nearObjectDown = -1;
        nearObjectUp = -1;
        nearObjectRight = -1;
        nearObjectLeft = -1;
    }
	
    void ApplyGravity()
    {
        if (nearObjectDown != 0)
            verticalSpeed = Mathf.Max(-maxDownSpeed, (verticalSpeed - downSpeedPerSec * Time.deltaTime));

    }

    bool IsTouchingSide (float sideDistance)
    {
        return sideDistance <= boxSize.x * (1 - innerDistBoxDetection) * 1.1f;
    }
    void Jump ()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsTouchingSide(nearObjectDown))
            {
                verticalSpeed = Mathf.Min(jumpImpulse, verticalSpeed + jumpImpulse);
                isJumping = true;
                wallJumpImpulse = 0;
                Debug.Log("down");
            }

            else if (IsTouchingSide(nearObjectLeft))
            {
                Debug.Log("left");
                verticalSpeed = Mathf.Min(jumpImpulse * percentageJumpImpLostWall, verticalSpeed + jumpImpulse * percentageJumpImpLostWall) ;
                isJumping = true;
                wallJumpImpulse = wallJumpLatImpVal;
            }
            else if (IsTouchingSide(nearObjectRight))
            {
                Debug.Log("right");
                verticalSpeed = Mathf.Min(jumpImpulse * percentageJumpImpLostWall, verticalSpeed + jumpImpulse * percentageJumpImpLostWall);
                isJumping = true;
                wallJumpImpulse = - wallJumpLatImpVal;
            }
            else
                wallJumpImpulse = Mathf.Sign(wallJumpImpulse) * 
                    Mathf.Max(0, Mathf.Abs(wallJumpImpulse) - wallJumpDecreasePerSec * Time.deltaTime);
        }
        else
            wallJumpImpulse = Mathf.Sign(wallJumpImpulse) *
                    Mathf.Max(0, Mathf.Abs(wallJumpImpulse) - wallJumpDecreasePerSec * Time.deltaTime);
    }

    void Mouvement()
    {
        Jump();
        ApplyGravity();
        horizontalSpeed =
            Input.GetAxis("Horizontal") * maxHorizontalspeed *
            (IsTouchingSide(nearObjectDown)? 1 : percMidAirSpeedLost) + wallJumpImpulse;

        Vector2 newPosition = position + new Vector2(horizontalSpeed, verticalSpeed) * Time.deltaTime;

        if (nearObjectDown != -1 && newPosition.y - position.y < - nearObjectDown + boxSize.x * (1 - innerDistBoxDetection))
        {
            newPosition.y = position.y - nearObjectDown + boxSize.x * (1 - innerDistBoxDetection);
            isJumping = false;
            verticalSpeed = 0;
        }
        else if (nearObjectUp != -1 && newPosition.y - position.y >  nearObjectUp - boxSize.x * (1 - innerDistBoxDetection))
        {
            newPosition.y = position.y + nearObjectUp - boxSize.x * (1 - innerDistBoxDetection);
            verticalSpeed = 0;

        }

        if (nearObjectLeft != -1 && newPosition.x - position.x < - nearObjectLeft + boxSize.x * (1 - innerDistBoxDetection))
        {
            newPosition.x = position.x - nearObjectLeft + boxSize.x * (1 - innerDistBoxDetection);
        }


        else if (nearObjectRight != -1 && newPosition.x - position.x >  nearObjectRight - boxSize.x * (1 - innerDistBoxDetection))
        {
            newPosition.x = position.x + nearObjectRight - boxSize.x * (1 - innerDistBoxDetection);

        }
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

        RaycastHit2D hitRightUp = Physics2D.Raycast(upRightCorner, Vector2.right, 100*maxHorizontalspeed);
        RaycastHit2D hitUpRight = Physics2D.Raycast(upRightCorner, Vector2.up, 100*jumpImpulse);
        RaycastHit2D hitUpLeft = Physics2D.Raycast(upLeftCorner, Vector2.up, 100*jumpImpulse);
        RaycastHit2D hitLeftUp = Physics2D.Raycast(upLeftCorner, Vector2.left, 100* maxHorizontalspeed);
        RaycastHit2D hitRightDown = Physics2D.Raycast(downRightCorner, Vector2.right, 100* maxHorizontalspeed);
        RaycastHit2D hitDownRight = Physics2D.Raycast(downRightCorner, Vector2.down, 100*maxDownSpeed);
        RaycastHit2D hitLeftDown = Physics2D.Raycast(downLeftCorner, Vector2.left, 100*maxHorizontalspeed);
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
            nearObjectUp = Mathf.Min(hitUpRight.distance, hitUpLeft.distance);
        else
            nearObjectUp = -1;

        if (hitRightUp.collider != null || hitRightDown.collider != null)
            nearObjectRight = Mathf.Min(hitRightUp.distance, hitRightDown.distance);
        else
            nearObjectRight = -1;

        if (hitLeftUp.collider != null || hitLeftDown.collider != null)
            nearObjectLeft = Mathf.Min(hitLeftUp.distance, hitLeftUp.distance);
        else
            nearObjectLeft = -1;

        if (hitDownRight.collider != null || hitDownLeft.collider != null)
        {
            nearObjectDown = Mathf.Min(hitDownRight.distance, hitDownLeft.distance);
        }
        else
            nearObjectDown = -1;
    }
    
    void Update () {
        updateCollisions();
        Mouvement();
	}
}
