using UnityEngine;
using System.Collections;

public class TutorialSimpleEnemy : MonoBehaviour {

    public float minDistance;
    GameObject player;
    SimpleEnemyController selfController;

    void Start()
    {
        player = GameObject.Find("Player");
        selfController = GetComponent<SimpleEnemyController>();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < minDistance)
        {
            if (!selfController.enabled)
            {
                selfController.enabled = true;
            }
        }
    }
}
