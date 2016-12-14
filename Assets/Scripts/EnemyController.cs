using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{

    const float PATROL_WAIT_INVULNERABILITY = 0.5f;

    public GameObject player;

    public float speed;

    public bool patrol;

    [Tooltip("If patrol is checked, will go from the in order to the last point stored here and then from the last to the first")]
    public Vector2[] patrolPoints;

    [Tooltip("Time in seconds the enemy will wait before moving on to the next patrol point")]
    public float patrolWait;

    [Tooltip("Should the enemy guard its position instead of following the player?")]
    public bool guard;

    int currentPoint;

    bool isGoingRight;

    float lastTimePatrolStopped;

    //Should the enemy do anything
    bool dead;

    float deltaDistance = 10f;

    public bool isDead
    {
        get
        {
            return dead;
        }
        set
        {
            dead = value;
        }
    }

    void Start()
    {
        if (guard)
        {
            deltaDistance = 10000f;
        }
    }

    void Update()
    {
        if (!dead)
        {
            if (patrol)
            {
                bool isOnSpot = transform.position.x == patrolPoints[currentPoint].x;// && transform.position.y == patrolPoints[currentPoint].y;
                isGoingRight = patrolPoints[currentPoint].x >= transform.position.x;
                Vector3 target = new Vector3(patrolPoints[currentPoint].x, transform.position.y, transform.position.z);
               // Debug.Log(target);
                if (!isGoingRight && !isOnSpot)
                {
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }

                else
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }

                if (isOnSpot)
                {
                    if (currentPoint == patrolPoints.Length - 1)
                    {
                        currentPoint = 0;
                    }

                    else
                    {
                        currentPoint += 1;
                    }

                    if (Time.time - lastTimePatrolStopped > PATROL_WAIT_INVULNERABILITY)
                    {
                        lastTimePatrolStopped = Time.time;
                        StartCoroutine(WaitToKeepPatrolling());
                    }
                }

                transform.position = Vector2.MoveTowards(transform.position, target, speed);


                    VerifyIfShouldBeFollowingPlayer();
            }

            else
            {
                Vector3 target;
                //target = new Vector3(player.transform.position.x, patrolPoints[currentPoint].y, transform.position.z);
                target = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector2.MoveTowards(transform.position, target, speed);
                isGoingRight = target.x >= transform.position.x;
                if (!isGoingRight)
                {
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }

                else
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }

    //Follow player if he goes too far to the right
    void VerifyIfShouldBeFollowingPlayer()
    {
        patrol = player.transform.position.x - transform.position.x < deltaDistance;
    }

    IEnumerator WaitToKeepPatrolling()
    {
        patrol = false;
        yield return new WaitForSeconds(patrolWait);
        patrol = true;
    }

    void Die()
    {
        // Destroy(gameObject);
        if (!dead)
        {
            //animation["enemyDeath"].wrapMode
            dead = true;
            GetComponent<Animator>().Play(Animator.StringToHash("enemyDeath"));
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(boxCollider.size.x - 0.1f, boxCollider.size.y);
        }
    }
}
