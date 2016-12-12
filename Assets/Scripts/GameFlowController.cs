using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameFlowController : MonoBehaviour
{

    NarrationController narrationController;

    //[Tooltip("Default time to wait between dialog chunks. May be different in specific cases for narration purposes")]
    //public float timeToWaitBetweenDialog;

    public string[] firstDialogSequence;

    void Start()
    {
        narrationController = FindObjectOfType<NarrationController>();
        firstDialogSequence[0] = "Welcome to Earth, " + GameModel.playerName + "|1|";
        GameModel.birthDate = System.DateTime.Now.ToShortDateString();
        firstDialogSequence[1] = "You were born in a loving family and grew up a strong,|0.1| independent kid.|0.3|";
        firstDialogSequence[2] = "You did not go out of your room very often.|0.3|";
        firstDialogSequence[3] = "But today you got lost in a park. Scared, you quickly know what must be done.|0.3|";
        firstDialogSequence[4] = "You have to go back to your room!";
        GameModel.gameState = 2;
    }

    void Update()
    {
        if (GameModel.gameState == 0)
        {
            GameModel.gameState = 1;
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
    }
}
