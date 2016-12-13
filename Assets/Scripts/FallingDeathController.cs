using UnityEngine;
using System.Collections;

public class FallingDeathController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().Die();
        }

        else if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.SendMessage("Die");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().Die();
        }

        else if (col.collider.gameObject.tag == "Enemy")
        {
            col.gameObject.SendMessage("Die");
            Destroy(col.gameObject, 0.3f);
        }
    }
}
