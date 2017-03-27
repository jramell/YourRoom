using UnityEngine;
using System.Collections;

public class SimpleEnemyController : MonoBehaviour {

    [Tooltip("Should he go right? If unchecked, he'll go left")]
    public bool goRight;

    public float speed;

    bool dead;

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

    Animator selfAnimator;

    void Start()
    {
        if (goRight)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        else
        {
            speed *= -1;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        selfAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!dead)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, transform.position.z));
            selfAnimator.Play(Animator.StringToHash("raptor_walk"));
        }
    }

    void Die()
    {
        if (!dead)
        {
            dead = true;
            selfAnimator.Play(Animator.StringToHash("raptor_death"));
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(boxCollider.size.x - 0.1f, boxCollider.size.y);

        }
    }
}
