using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private const string HORIZONTAL_AXIS = "Horizontal";
    //If the player is at this velocity or less while jumping he's considered to be in its apex or falling and gravity is increased
    private float jumpApexVelocity = 3.5f;

    //Controls how fast the player falls at free fall or once his jumping is finished
    private const float INCREASED_GRAVITY_SCALE = 3f;

    public float speed;
    public float jumpStrength;
    Vector2 translation;
    private PlayerView playerViewScript;
   // private PlayerModel playerModel;
    public  LayerMask everythingButThePlayer;
   public  float defaultGravity;
    float currentGravity;
    private float verticalDistanceBetweenRays;
    public float maxFallingVelocity;
    Vector2 input;
    Animator playerAnimator;
    public bool isJumping;

    void Start()
    {
       // speed = app.models.playerModel.speed;
      //  playerView = app.views.playerView;
      //  playerTransform = playerView.transform;
       // playerModel = app.models.playerModel;
        //playerRigidbody = playerView.GetComponent<Rigidbody2D>();
        playerViewScript = GetComponent<PlayerView>();
        playerAnimator = GetComponent<Animator>();
        //defaultGravity = playerModel.gravity;
        currentGravity = defaultGravity;
        //Half the jump strenght because that's what feels good
        jumpApexVelocity = jumpStrength * 0.5f;
    }

    void Update()
    {
        input = new Vector2(Input.GetAxis(HORIZONTAL_AXIS), 0);
        MoveThePlayer(input);
    }

    //Attemps to move the player with the translation vector, but does not move if it detects it will cause a collision
    public void MoveThePlayer(Vector2 input)
    {
        if (playerViewScript.collisionState.above || playerViewScript.collisionState.below)
        {
            translation.y = 0;
            currentGravity = defaultGravity;
        }
        //Debug.Log("input this frame: " + input.x);
        translation.x = input.x * speed;

        if (translation.x != 0)
        {
            if (translation.x < 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }

            else
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }

            if (playerViewScript.collisionState.below)
            {
                playerAnimator.Play(Animator.StringToHash("walk"));
            }
        }

        else if (playerViewScript.collisionState.below)
        {
            playerAnimator.Play(Animator.StringToHash("idle"));
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerViewScript.collisionState.below)
        {
            translation.y = jumpStrength;
                playerAnimator.Play(Animator.StringToHash("jump"));
           
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            currentGravity = defaultGravity * INCREASED_GRAVITY_SCALE;
        }

        if (translation.y > 0 && translation.y < jumpApexVelocity)
        {
            currentGravity = defaultGravity * INCREASED_GRAVITY_SCALE;
        }

        //Gravity
        translation.y += currentGravity * Time.deltaTime;

        if (translation.y < maxFallingVelocity)
        {
            translation.y = maxFallingVelocity;
        }

        playerViewScript.CheckPossibleMovement(translation * Time.deltaTime);
    }

    public void Jump()
    {
        if (playerViewScript.collisionState.below)
        {
            translation.y = jumpStrength;
        }
    }

    public void IncreaseGravity()
    {
        if (!playerViewScript.collisionState.below)
        {

        }
    }
}
