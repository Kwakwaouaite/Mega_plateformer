using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {

	[SerializeField] private float jumpImpulse;
    [SerializeField] private float maxTimePressJumpSec;
    [SerializeField] private float maxHorizontalspeed;
    [SerializeField] private float maxDownSpeed;
    [SerializeField] private float downSpeedPerSec;
    [SerializeField] private float innerDistBoxDetection = 0.99f;
    [SerializeField] private float wallJumpLatImpVal;
    [SerializeField] private float wallJumpDecreasePerSec;
    [SerializeField] private float percMidAirSpeedLost = 1;
    [SerializeField] private float percFrictionWall = 0;

    [SerializeField] private float percentageJumpImpLostWall = 1;

    [SerializeField] private int maxNumberJump = 2;

    [SerializeField] private LayerMask hitRayUp;
    [SerializeField] private LayerMask hitRayDown;
    [SerializeField] private LayerMask hitRayRight;
    [SerializeField] private LayerMask hitRayLeft;

    private float nearObjectUp;
    private float nearObjectDown;
    private float nearObjectRight;
    private float nearObjectLeft;

    private GameObject objectUp;
    private GameObject objectDown;
    private GameObject objectRight;
    private GameObject objectLeft;



    private float verticalSpeed;
    private float horizontalSpeed;
    private float wallJumpImpulse;

    private int currentNumberJump = 0;
    private float timeStartJump = 0;

    private Vector2 position;
    private Vector2 boxSize;

    private Vector2 upLeftCorner;
    private Vector2 upRightCorner;
    private Vector2 downRightCorner;
    private Vector2 downLeftCorner;
    
    void Start () {
        position = gameObject.transform.position;
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

    bool IsTouchingSide(float sideDistance)
    {
        return sideDistance <= boxSize.x * (1 - innerDistBoxDetection) * 1.1f;
    }
    void ApplyGravity()
    {
        if (nearObjectDown != 0)
        {
            verticalSpeed = Mathf.Max(-maxDownSpeed, (verticalSpeed - downSpeedPerSec * Time.deltaTime));

            if (!Input.GetButtonDown("Jump") && ((IsTouchingSide(nearObjectLeft) && Input.GetAxisRaw("Horizontal") < 0) || 
                (IsTouchingSide(nearObjectRight) && Input.GetAxisRaw("Horizontal") > 0)))
                verticalSpeed *= (1 - percFrictionWall);
        }


    }

    void Jump ()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsTouchingSide(nearObjectLeft) && !IsTouchingSide(nearObjectDown))
            {
                verticalSpeed = jumpImpulse * percentageJumpImpLostWall; // Mathf.Min(jumpImpulse * percentageJumpImpLostWall, verticalSpeed + jumpImpulse * percentageJumpImpLostWall) ;
                wallJumpImpulse = wallJumpLatImpVal;
            }
            else if (IsTouchingSide(nearObjectRight) && !IsTouchingSide(nearObjectDown))
            {
                verticalSpeed = jumpImpulse * percentageJumpImpLostWall;// Mathf.Min(jumpImpulse * percentageJumpImpLostWall, verticalSpeed + jumpImpulse * percentageJumpImpLostWall);
                wallJumpImpulse = - wallJumpLatImpVal;
            }
            else if (IsTouchingSide(nearObjectDown) || currentNumberJump < maxNumberJump)
            {
                verticalSpeed = jumpImpulse;//Mathf.Min(jumpImpulse, verticalSpeed + jumpImpulse);    
                currentNumberJump += 1;
                timeStartJump = Time.time;
            }
            else
                wallJumpImpulse = Mathf.Sign(wallJumpImpulse) * 
                    Mathf.Max(0, Mathf.Abs(wallJumpImpulse) - wallJumpDecreasePerSec * Time.deltaTime);
        } else if (Input.GetButton("Jump") && Time.time - timeStartJump < maxTimePressJumpSec)
        {
            verticalSpeed = jumpImpulse;
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
            Input.GetAxisRaw("Horizontal") * maxHorizontalspeed *
            (IsTouchingSide(nearObjectDown)? 1 : percMidAirSpeedLost) + wallJumpImpulse;

        Vector2 newPosition = position + new Vector2(horizontalSpeed, verticalSpeed) * Time.deltaTime;

        if (nearObjectDown != -1 && newPosition.y - position.y < - nearObjectDown + boxSize.x * (1 - innerDistBoxDetection))
        {
            //Debug.Log(objectDown.name + " :" + objectDown.GetComponent<PlateformBase>().GetSpeed());
            Debug.Assert(objectDown.GetComponent<PlateformBase>());
            Vector2 pushSpeed = objectDown.GetComponent<PlateformBase>().GetSpeed();
            newPosition.y = position.y - nearObjectDown + boxSize.x * (1 - innerDistBoxDetection) + Mathf.Max(0, pushSpeed.y);
            newPosition.x += pushSpeed.x;
            wallJumpImpulse = 0;
            currentNumberJump = 0;
        }
        else if (nearObjectUp != -1 && newPosition.y - position.y >  nearObjectUp - boxSize.x * (1 - innerDistBoxDetection))
        {
            Vector2 pushSpeed = objectUp.GetComponent<PlateformBase>().GetSpeed();
            newPosition.y = position.y + nearObjectUp - boxSize.x * (1 - innerDistBoxDetection) + Mathf.Min(0, pushSpeed.y);
            timeStartJump = -1;
            newPosition.x += pushSpeed.x;
            verticalSpeed = 0;

        }

        if (nearObjectLeft != -1 && newPosition.x - position.x < - nearObjectLeft + boxSize.x * (1 - innerDistBoxDetection))
        {
            newPosition.x = position.x - nearObjectLeft + boxSize.x * (1 - innerDistBoxDetection);
            wallJumpImpulse = 0;
        }


        else if (nearObjectRight != -1 && newPosition.x - position.x >  nearObjectRight - boxSize.x * (1 - innerDistBoxDetection))
        {
            newPosition.x = position.x + nearObjectRight - boxSize.x * (1 - innerDistBoxDetection);
            wallJumpImpulse = 0;

        }
        transform.position = newPosition;
        position = newPosition;
        
    }

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

        RaycastHit2D hitRightUp = Physics2D.Raycast(upRightCorner, Vector2.right, 100*maxHorizontalspeed, layerMask: hitRayRight);
        RaycastHit2D hitUpRight = Physics2D.Raycast(upRightCorner, Vector2.up, 100*jumpImpulse, layerMask: hitRayUp);
        RaycastHit2D hitUpLeft = Physics2D.Raycast(upLeftCorner, Vector2.up, 100*jumpImpulse, layerMask: hitRayUp);
        RaycastHit2D hitLeftUp = Physics2D.Raycast(upLeftCorner, Vector2.left, 100* maxHorizontalspeed, layerMask: hitRayLeft);
        RaycastHit2D hitRightDown = Physics2D.Raycast(downRightCorner, Vector2.right, 100* maxHorizontalspeed, layerMask: hitRayRight);
        RaycastHit2D hitDownRight = Physics2D.Raycast(downRightCorner, Vector2.down, 100*maxDownSpeed, layerMask: hitRayDown);
        RaycastHit2D hitLeftDown = Physics2D.Raycast(downLeftCorner, Vector2.left, 100*maxHorizontalspeed, layerMask: hitRayLeft);
        RaycastHit2D hitDownLeft = Physics2D.Raycast(downLeftCorner, Vector2.down, 100*maxDownSpeed, layerMask: hitRayDown);

        DebugDrawRay(hitRightUp, upRightCorner);
        DebugDrawRay(hitUpRight, upRightCorner);
        DebugDrawRay(hitUpLeft, upLeftCorner);
        DebugDrawRay(hitLeftUp, upLeftCorner);
        DebugDrawRay(hitDownRight, downRightCorner);
        DebugDrawRay(hitRightDown, downRightCorner);
        DebugDrawRay(hitDownLeft, downLeftCorner);
        DebugDrawRay(hitLeftDown, downLeftCorner);

        /*
        if (hitUpRight.collider != null || hitUpLeft.collider != null)
            nearObjectUp = Mathf.Min(hitUpRight.distance, hitUpLeft.distance);
        else
            nearObjectUp = -1;*/

        if (hitUpRight.collider != null || hitUpLeft.collider != null)
        {
            if (hitUpRight && hitUpLeft)
            {
                nearObjectUp = Mathf.Min(hitUpRight.distance, hitUpLeft.distance);
                if (hitUpRight.distance < hitUpLeft.distance)
                {
                    objectUp = hitUpRight.transform.gameObject;
                } else
                {
                    objectUp = hitUpLeft.transform.gameObject;
                }

            }else if (hitUpRight)
            {
                objectUp = hitUpRight.transform.gameObject;
            } else
            {
                objectUp = hitUpLeft.transform.gameObject;
            }
        }
        else
        {
            nearObjectUp = -1;
            objectUp = null;
        }

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
            if (hitDownRight && hitDownLeft)
            {
                nearObjectDown = Mathf.Min(hitDownRight.distance, hitDownLeft.distance);
                if (hitDownRight.distance < hitDownLeft.distance)
                {
                    objectDown = hitDownRight.transform.gameObject;
                } else
                {
                    objectDown = hitDownLeft.transform.gameObject;
                }

            }else if (hitDownRight)
            {
                objectDown = hitDownRight.transform.gameObject;
            } else
            {
                objectDown = hitDownLeft.transform.gameObject;
            }
        }
        else
        {
            nearObjectDown = -1;
            objectDown = null;
        }
    }
    
    void Update () {
        updateCollisions();
        Mouvement();
	}
}
