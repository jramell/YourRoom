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

    int menuState;

    [Tooltip("Maximum length the player name can be")]
    public int maximumNameLength;

    int maximumTextLength;

    string realStoredName;

    public Button[] buttons;

    public AudioSource menuSFX;

    bool wait;

    void Awake()
    {
        maximumTextLength = maximumNameLength + 1;
        menuState = -2;
        wait = true;
    }

    void Update()
    {
        if (!startPressed)
        {
            float y = 0;
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                y = -1;
            }

            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                y = 1;
            }

            ChangeMenuState(y);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                buttons[menuState].onClick.Invoke();
            }
        }

        else
        {
            string inputThisFrame = Input.inputString;
            if (inputThisFrame != "")
            {
                if (nameText.text == "_")
                {
                    nameText.text = inputThisFrame;
                }

                if (!inputThisFrame.Contains("_") && !inputThisFrame.Contains("\n"))
                {
                    if (inputThisFrame == "\b")
                    {
                        if (nameText.text.Length > 1)
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

                if (inputThisFrame == "/n")
                {
                    StartGame();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && !wait)
            {
                StartGame();
            }
        }
    }

    void ChangeMenuState(float y)
    {
        if (y != 0)
        {
            if (menuState == -2)
            {
                menuState = 0;
                y = 0;
            }

            int buttonCount = buttons.Length;
            if (y > 0)
            {
                if (menuState == 0)
                {
                    menuState = buttonCount - 1;
                }
                else
                {
                menuState--;

                }
            }
            if (y < 0)
            {
                //if (menuState == buttonCount - 1)
                //{
                //    menuState = 0;
                //}
                //else
                //{
                menuState++;

                //}
            }

            if (menuState < 0)
            {
                menuState = buttonCount - 1;
            }

            else if (menuState > buttonCount - 1)
            {
                menuState = 0;
            }
            menuSFX.Play();
        }

        if (menuState >= 0)
        {
            //Debug.Log("menuState: " + menuState);
            buttons[menuState].Select();
        }
        
    }

    void StartGame()
    {
        GameModel.playerName = realStoredName;
        SceneManager.LoadScene("2_Start");
    }

    public void OnStartClick()
    {
        //Load starting scene
        //SceneManager.LoadScene("2_Start");
        mainTitleObject.SetActive(false);
        firstQuestionObject.SetActive(true);
        startPressed = true;
        StartCoroutine(StartWait());
    }

    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(0.1f);
        wait = false;
    }

    public void OnCreditsClick()
    {
        //Load credits
        SceneManager.LoadScene("3_Credits");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
