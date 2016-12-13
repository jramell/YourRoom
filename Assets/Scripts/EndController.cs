using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndController : MonoBehaviour {

    public Text namePlace;

    public Text pointPlace;

    public Text dateLost;

    public Text timeLost;

    public AudioSource appearSFX;

    float timeWait = 0.5f;

    bool charged;

    void Start()
    {
        StartCoroutine(ShowEnd());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("3_Credits");
            charged = true;
        }
    }

    IEnumerator ShowEnd()
    {
        appearSFX.Play();
        namePlace.text = "Name: " + GameModel.playerName;
        yield return new WaitForSeconds(timeWait);
        appearSFX.Play();
        pointPlace.text = "Points: " + GameModel.currentPoints + "/300";
        yield return new WaitForSeconds(timeWait);
        appearSFX.Play();
        dateLost.text = "Lost in: " + GameModel.birthDate;
        yield return new WaitForSeconds(timeWait);
        appearSFX.Play();
        timeLost.text = "Was lost for: " + (int)Time.time/60 + " minutes";
        yield return new WaitForSeconds(10f);
        if (!charged)
        {
            SceneManager.LoadScene("3_Credits");
        }
    }
}
