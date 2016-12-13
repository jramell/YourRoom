using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorController : MonoBehaviour {

    public AudioSource doorSFX;

    void OnCollisionEnter2D(Collision2D col)
    {
        doorSFX.Play();
        SceneManager.LoadScene("5_Home");
        GameModel.gameState = 3;
    }

}
