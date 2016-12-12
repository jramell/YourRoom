﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerFeetView : MonoBehaviour {

    PlayerController playerController;

    public AudioSource hitEnemySoundEffect;

    PointController pointController;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        pointController = FindObjectOfType<PointController>();
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            //If enemy was hit and we are over him
            if(col.transform.position.y < transform.position.y)
            {
                playerController.HitEnemy();
                col.gameObject.SendMessage("Die");
            }
        }
    }

    //This function should be implemented in the enemy, as it is better for each object in scene to detect and manage
    //its own state changes (such as death). However, for the jam this is enough.
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Enemy")
        {
            //If enemy was hit and we are over him
            if (col.transform.position.y < transform.position.y)
            {

                playerController.HitEnemy();
                if (!col.gameObject.GetComponent<EnemyController>().isDead)
                {
                    col.gameObject.SendMessage("Die");
                    pointController.AddPoints(col.transform.position, 10);
                }

                else
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    hitEnemySoundEffect.pitch = pitch;
                    hitEnemySoundEffect.Play();
                }

            }
        }
    }


}