using UnityEngine;
using System.Collections;

//Handles detection/raycasting that has to do with the Player Game Object
public class PlayerView : MonoBehaviour {

    //The number of rays to be casted to detect collision each time the player moves
    private const int NUMBER_OF_RAYS = 5;
    private const int NUMBER_OF_VERTICAL_RAYS = 4;

    PlayerController playerController;
    public LayerMask groundElements;
    BoxCollider2D boxCollider;
    //So the Box Collider's bounds can be updated independently from it
    Bounds bounds;
    Vector2 colliderCorner;
    public float skinWidth;
    bool isGoingRight;
    float verticalDistanceBetweenRays;
    float horizontalDistanceBetweenRays;
    RaycastOrigins raycastOrigins;
    float gravity;
    public CollisionState collisionState;

    void Start()
    {
        //playerController = app.controllers.playerController;
        //groundElements = app.models.playerModel.groundElements;
        boxCollider = GetComponent<BoxCollider2D>();
        //skinWidth = app.models.playerModel.skinWidth;
        bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);
        verticalDistanceBetweenRays = bounds.size.y / (NUMBER_OF_RAYS - 1);
        horizontalDistanceBetweenRays = bounds.size.x / (NUMBER_OF_VERTICAL_RAYS - 1);
    }

    public void CheckPossibleMovement(Vector2 translate)
    {
        UpdateRaycastOrigins();
        collisionState.Reset();
        if(translate.x != 0)
        {
            CheckHorizontalMovement(ref translate);

        }


        if(translate.y != 0)
        {
            CheckVerticalMovement(ref translate);

        }

        //playerAnimator.SetFloat("SpeedX", translate.x);
        //playerAnimator.SetFloat("SpeedY", translate.y);
        transform.Translate(translate);
    }

    void CheckVerticalMovement(ref Vector2 translate)
    {
        float directionY = Mathf.Sign(translate.y);
        float rayLength = Mathf.Abs(translate.y) + skinWidth;

        for (int i = 0; i < NUMBER_OF_VERTICAL_RAYS; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (horizontalDistanceBetweenRays * i + translate.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, groundElements);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                translate.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (directionY == 1)
                {
                    collisionState.above = true;
                }
                else
                {
                    collisionState.below = true;
                }
                //collisionState.above = directionY == 1;
                //collisionState.below = directionY == -1;
            }
        }
    }

    void CheckHorizontalMovement(ref Vector2 translate)
    {
        float directionX = Mathf.Sign(translate.x);
        float rayLength = Mathf.Abs(translate.x) + skinWidth;

        for (int i = 0; i < NUMBER_OF_RAYS; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (verticalDistanceBetweenRays * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, groundElements);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);

            if (hit)
            {
                translate.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                if (directionX == 1)
                {
                    collisionState.right = true;
                }

                else
                {
                    collisionState.left = true;
                }
                //collisionState.right = directionX == 1;
                //collisionState.left = directionX == -1;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);
        Vector2 boundsMax = bounds.max;
        Vector2 boundsMin = bounds.min;
        raycastOrigins.topLeft = new Vector2(boundsMin.x, boundsMax.y);
        raycastOrigins.topRight = boundsMax;
        raycastOrigins.bottomLeft = boundsMin;
        raycastOrigins.bottomRight = new Vector2(boundsMax.x, boundsMin.y);
    }

    public CollisionState GetCollisionState()
    {
        return collisionState;
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionState
    {
        //True if there's a collision above the player, false if not
        public bool above;

        //True if there's a collision below the player, false if not
        public bool below;

        //True if there's a collision to the left of the player, false if not
        public bool left;

        //True if there's a collision to the right of the player, false if not
        public bool right;

        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
        }
    }

//    void CalculateCollisionParameters(Vector2 translate)
//    {
//        isGoingRight = translate.x > 0;
//        Vector2 currentOrigin;
//        Vector2 rayDirection;
//        if (translate.y < 0)
//        {
//            colliderCorner = boxCollider.bounds.min;
//            rayDirection = -Vector2.up;
//            for (int i = 0; i < NUMBER_OF_VERTICAL_RAYS; i++)
//            {
//                currentOrigin = new Vector2(colliderCorner.x + i * horizontalDistanceBetweenRays, colliderCorner.y - skinWidth);
//                raysToCastVertically[i] = new Ray2D(currentOrigin, rayDirection);
//            }
//        }
//        else
//        {
//            colliderCorner = boxCollider.bounds.max;
//            rayDirection = Vector2.up;
//            for (int i = 0; i < NUMBER_OF_VERTICAL_RAYS; i++)
//            {
//                currentOrigin = new Vector2(colliderCorner.x - i * horizontalDistanceBetweenRays, colliderCorner.y + skinWidth);
//                raysToCastVertically[i] = new Ray2D(currentOrigin, rayDirection);
//            }
//        }

//        if (translate.x != 0)
//        {
//            if (isGoingRight)
//            {
//                colliderCorner = boxCollider.bounds.max;
//                for (int i = 0; i < NUMBER_OF_RAYS; i++)
//                {
//                    currentOrigin = new Vector2(colliderCorner.x + skinWidth, colliderCorner.y - i * verticalDistanceBetweenRays);
//                    raysToCast[i] = new Ray2D(currentOrigin, Vector2.right);
//                }

//            }
//            else
//            {
//                colliderCorner = boxCollider.bounds.min;
//                for (int i = 0; i < NUMBER_OF_RAYS; i++)
//                {
//                    currentOrigin = new Vector2(colliderCorner.x - skinWidth, colliderCorner.y + i * verticalDistanceBetweenRays);
//                    raysToCast[i] = new Ray2D(currentOrigin, -Vector2.right);
//                }
//            }
//        }
//    }

//    public void CheckForObstaclesToMovement(ref Vector2 translate)
//    {
//        CalculateCollisionParameters(translate);
//        int count = raysToCast.Length;
//        Ray2D currentRay;
//        for(int i = 0; i < count; i++)
//        {
//            currentRay = raysToCast[i];
//            Debug.DrawRay(currentRay.origin, currentRay.direction, Color.green);
//            RaycastHit2D hit2d = Physics2D.Raycast(currentRay.origin, currentRay.direction, translate.x, groundElements);
//           // float slope = Vector2.Angle(hit2d.normal, Vector2.up);
//            if (hit2d.collider != null)
//            {
//                //Handle horizontal movement
//                if(isGoingRight)
//                {
//                     translate.x = Mathf.Min(translate.x, hit2d.point.x - currentRay.origin.x);
//                }

//                else
//                {
//                    translate.x = Mathf.Max(translate.x, hit2d.point.x - currentRay.origin.x);
//                }


//                //if (Mathf.Abs(translate.x) < skinWidth)
//                //{
//                //    //Direct hit, no need to check the other rays
//                //    break;
//                //}
//            }
//        }
//        CheckVerticalObstacles(ref translate);
//    }

//    private void CheckVerticalObstacles(ref Vector2 translate)
//    {
//        CalculateCollisionParameters(translate);
//        int count = raysToCastVertically.Length;
//        Ray2D currentRay;
//        for (int i = 0; i < count; i++)
//        {
//            currentRay = raysToCastVertically[i];
//            Debug.DrawRay(currentRay.origin, currentRay.direction, Color.red);
//            RaycastHit2D hit2d = Physics2D.Raycast(currentRay.origin, currentRay.direction, translate.y, groundElements);
//            // float slope = Vector2.Angle(hit2d.normal, Vector2.up);
//            if (hit2d.collider != null)
//            {
//                //Handle horizontal movement
//                if (translate.y > 0)
//                {
//                    translate.y = Mathf.Min(translate.x, hit2d.point.x - currentRay.origin.x);
//                }

//                else
//                {
//                    translate.y = Mathf.Max(translate.x, hit2d.point.x - currentRay.origin.x);
//                }


//                //if (Mathf.Abs(translate.x) < skinWidth)
//                //{
//                //    //Direct hit, no need to check the other rays
//                //    break;
//                //}
//            }
//        }
//    }
}
