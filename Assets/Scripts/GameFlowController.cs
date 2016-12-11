using UnityEngine;
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
        firstDialogSequence[0] = "Welcome to Earth, " + GameModel.playerName;
        GameModel.birthDate = System.DateTime.Now.ToShortDateString();
        firstDialogSequence[1] = "You are born in " + GameModel.birthDate;
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
        yield return new WaitForSeconds(1);
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
        }
    }
}
