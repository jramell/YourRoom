using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingDialogController : MonoBehaviour {

    NarrationController narrationController;

    //[Tooltip("Default time to wait between dialog chunks. May be different in specific cases for narration purposes")]
    //public float timeToWaitBetweenDialog;

    bool started;
     string[] second;

    void Start()
    {
        narrationController = FindObjectOfType<NarrationController>();
        second = new string[2];
        second[0] = "\"" + GameModel.playerName + "?\"|1|";
        second[1] = "\"Where have you been?\"|1|";
        
        // GameModel.gameState = 2;
    }

    void Update()
    {
        if (!started)
        {
            StartCoroutine(ManageEvents());
        }
    }

    IEnumerator ManageEvents()
    {
        started = true;

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
