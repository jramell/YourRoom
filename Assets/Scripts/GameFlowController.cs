using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameFlowController : MonoBehaviour
{

    NarrationController narrationController;

    //[Tooltip("Default time to wait between dialog chunks. May be different in specific cases for narration purposes")]
    //public float timeToWaitBetweenDialog;

    string[] firstDialogSequence;

    public string[] second;

    void Start()
    {
        firstDialogSequence = new string[3];
        narrationController = FindObjectOfType<NarrationController>();
        firstDialogSequence[0] = "As a kid, you arrive at a park in the morning.";
        GameModel.birthDate = System.DateTime.Now.ToShortDateString();
        //firstDialogSequence[1] = "You were born in a loving family.|0.3|";
        firstDialogSequence[1] = "You get lost in the park!|0.3|";
        firstDialogSequence[2] = "You have to go back to your room!|0.9|";
        second = new string[2];
        second[0] = "\"" + GameModel.playerName + "?|0.4|";
        second[1] = "\"Where have you been?\"|1|";
       //GameModel.gameState = 2;
    }

    void Update()
    {
        if (GameModel.gameState == 0)
        {
            GameModel.gameState = 1;
            StartCoroutine(ManageEvents());
        }

        if (GameModel.gameState == 3)
        {
            GameModel.gameState = 4;
            StartCoroutine(ManageEvents());
        }
    }

    IEnumerator ManageEvents()
    {
        yield return new WaitForSeconds(1.5f);
        if (GameModel.gameState == 1)
        {
            for (int i = 0; i < firstDialogSequence.Length; i++)
            {
                narrationController.ShowText(firstDialogSequence[i]);
                while (narrationController.isIntroducingText)
                {
                    yield return new WaitForSeconds(1f);
                }
            }
            SceneManager.LoadScene("4_FirstPath");
            GameModel.gameState = 2;
        }

        if (GameModel.gameState == 4)
        {
            for (int i = 0; i < second.Length; i++)
            {
                narrationController.ShowText(second[i]);
                while (narrationController.isIntroducingText)
                {
                    yield return new WaitForSeconds(1f);
                }
            }
            SceneManager.LoadScene("6_Points");
        }
    }
}
