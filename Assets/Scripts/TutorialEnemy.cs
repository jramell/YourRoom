using UnityEngine;
using System.Collections;

public class TutorialEnemy : MonoBehaviour {

    public float minDistance;
    GameObject player;
    EnemyController selfController;

    void Start()
    {
        player = GameObject.Find("Player");
        selfController = GetComponent<EnemyController>();
    }

	void Update()
    {
       if(Vector2.Distance(transform.position, player.transform.position) < minDistance)
        {
            if(!selfController.enabled)
            {
                selfController.enabled = true;
            }
        }
    }
}
