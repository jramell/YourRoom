using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointObject : MonoBehaviour {

    int pointsWorth;

    Text pointsText;

    public float speed;

    float moveTime = 1f;

    float deathTime = 2f;

    float totalTimeMoved;

    public int points
    {
        get
        {
            return pointsWorth;
        }
        set
        {
            pointsWorth = value;
            GetComponent<Text>().text = pointsWorth + "";
        }
    }

    void Start()
    {
        Destroy(gameObject, deathTime);
    }
    
    void Update()
    {
        if (totalTimeMoved < moveTime)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            totalTimeMoved += Time.deltaTime;
        }
    }
}
