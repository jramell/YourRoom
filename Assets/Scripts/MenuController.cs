using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject mainTitleObject;

    public GameObject firstQuestionObject;

    public Text nameText; 

    bool startPressed;

    [Tooltip("Maximum length the player name can be")]
    public int maximumNameLength;

    int maximumTextLength;

    string realStoredName;

    void Awake()
    {
        maximumTextLength = maximumNameLength + 1;
    }

    void Update()
    {
        if (startPressed)
        {
            string inputThisFrame = Input.inputString;
            if (inputThisFrame != "")
            {
                if(nameText.text == "_")
                {
                    nameText.text = inputThisFrame;
                }

                if(!inputThisFrame.Contains("_") && !inputThisFrame.Contains("\n"))
                {
                    if(inputThisFrame == "\b")
                    {
                        if(nameText.text.Length > 1)
                        {
                            realStoredName = realStoredName.Substring(0, realStoredName.Length - 1);
                        }
                    }

                    else if (nameText.text.Length < maximumTextLength)
                    {
                        realStoredName += inputThisFrame;
                    }
                    nameText.text = realStoredName + "_";
                }

                if(inputThisFrame == "/n")
                {
                    StartGame();
                }
            }

            if(Input.GetKeyDown(KeyCode.Return))
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        GameModel.playerName = realStoredName;
        SceneManager.LoadScene("2_Start");
    }

    //IEnumerator effectInName()
    //{
    //    if (nameText.text[nameText.text.Length - 1] == '_')
    //    {
    //        nameText.text = nameText.text.Substring(0, nameText.text.Length - 1);
    //    }
    //    yield return new WaitForSeconds(1);
    //}

    public void OnStartClick()
    {
        //Load starting scene
        //SceneManager.LoadScene("2_Start");
        mainTitleObject.SetActive(false);
        firstQuestionObject.SetActive(true);
        startPressed = true;
    }

    public void OnCreditsClick()
    {
        //Load credits
        //SceneManager.LoadScene("3_Credits");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
