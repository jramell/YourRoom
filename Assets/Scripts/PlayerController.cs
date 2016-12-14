using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

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
    public LayerMask everythingButThePlayer;
    public float defaultGravity;
    float currentGravity;
    private float verticalDistanceBetweenRays;
    public float maxFallingVelocity;
    Vector2 input;
    Animator playerAnimator;
    bool dead;
    public AudioSource jumpSoundEffect;

    bool enemyWasHit;
    //bool invulnerable;
    //public float invulnerabilityTime;
    public float enemyHitImpulse;
    DeathController deathController;
    float alteredGravity;

    void Start()
    {
        Application.targetFrameRate = 60;
        playerViewScript = GetComponent<PlayerView>();
        playerAnimator = GetComponent<Animator>();
        deathController = GetComponent<DeathController>();
        //currentGravity = defaultGravity;
        jumpApexVelocity = jumpStrength * 0.5f;
        alteredGravity = defaultGravity * INCREASED_GRAVITY_SCALE;
        currentGravity = alteredGravity;
    }

    void Update()
    {
        if (!dead)
        {
            input = new Vector2(Input.GetAxis(HORIZONTAL_AXIS), 0);
            MoveThePlayer(input);
            //if (transform.position.y < -12.8f)
            //{
            //    Die();
            //}
            if (transform.position.x < -3.3f)
            {
                transform.position = new Vector3(-3.3f, transform.position.y, transform.position.z);
            }
        }
    }

    //Attemps to move the player with the translation vector, but does not move if it detects it will cause a collision
    public void MoveThePlayer(Vector2 input)
    {
        if (playerViewScript.collisionState.above || playerViewScript.collisionState.below)
        {
            translation.y = 0;
            currentGravity = defaultGravity;
        }

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
            float pitch = Random.Range(0.9f, 1.1f);
            jumpSoundEffect.pitch = pitch;
            jumpSoundEffect.Play();
            translation.y = jumpStrength;
            currentGravity = defaultGravity;
            playerAnimator.Play(Animator.StringToHash("jump"));
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            currentGravity = alteredGravity;
        }

        //if (translation.y > 0 && translation.y < jumpApexVelocity)
        if(translation.y < jumpApexVelocity)
        {
            currentGravity = alteredGravity;
        }

        if (enemyWasHit)
        {
            translation.y = enemyHitImpulse;
            enemyWasHit = false;
        }

        if (!playerViewScript.collisionState.below)
        {
            playerAnimator.Play(Animator.StringToHash("jump"));
        }

        //Gravity
        translation.y += currentGravity * Time.deltaTime;

        if (translation.y < maxFallingVelocity)
        {
            translation.y = maxFallingVelocity;
        }

        playerViewScript.CheckPossibleMovement(translation * Time.deltaTime);
    }

    public void Die()
    {
        if (!dead)
        {
            dead = true;
            GetComponent<SpriteRenderer>().enabled = false;
            deathController.Die();
        }
    }

    public void HitEnemy()
    {
        enemyWasHit = true;
    }

    public void Jump()
    {
        if (playerViewScript.collisionState.below)
        {
            translation.y = jumpStrength;
        }
    }

    //The enemy should implement the "killing the player" detection function, but as there's no time and this already works I'll do it here
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Enemy")
        {
            //If you are considered to not be 'lateral' to the enemy and you touch him, you die

            EnemyController enemyCont = col.gameObject.GetComponent<EnemyController>();

            if (enemyCont != null)
            {
                if (Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(col.gameObject.transform.position.y)) < 0.3f)
                {
                    //Debug.Log("diff: " + (Mathf.Abs(transform.position.y) - Mathf.Abs(col.gameObject.transform.position.y)));
                    if (!col.gameObject.GetComponent<EnemyController>().isDead)
                    {
                        Die();
                    }
                }

            }
            else
            {
                //Debug.Log("difference: " + (transform.position.y - col.gameObject.transform.position.y));
                if (Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(col.gameObject.transform.position.y)) < 0.6f)
                {
                    if (!col.gameObject.GetComponent<SimpleEnemyController>().isDead)
                    {
                        Die();
                    }
                }
            }
        }

        //else if (col.collider.tag == "SimpleEnemy")
        //{
        //   if (transform.position.y - col.gameObject.transform.position.y < 0.1f)
        //    {
        //        if (!col.gameObject.GetComponent<SimpleEnemyController>().isDead)
        //        {
        //            Die();
        //        }
        //    }
        //}
    }
}
