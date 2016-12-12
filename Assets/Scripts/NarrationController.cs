using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NarrationController : MonoBehaviour
{

    [Tooltip("The text object that will contain the narration")]
    public Text narrationContainer;

    [Tooltip("The audio source containing the talk sound effect. If none is attached, no sound effect will play")]
    public AudioSource talkSoundEffect;

    [Tooltip("The amount of time to wait before writing the next character")]
    public float writeDelay;

    bool shouldSkipText;

    bool introducingText;

    void Update()
    {
        if (introducingText)
        {
            shouldSkipText = Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0);
        }
    }

    public bool isIntroducingText
    {
        get
        {
            return introducingText;
        }
    }

    public void ShowText(string textToIntroduce)
    {
        StartCoroutine(IntroduceText(textToIntroduce));
    }

    public IEnumerator IntroduceText(string textToIntroduce)
    {
        //Resets state so it doesn't skip by accident
        shouldSkipText = false;
        //Resets narration text 
        narrationContainer.text = "";
        introducingText = true;

        if (talkSoundEffect != null)
        {
            talkSoundEffect.Play();
        }

        //Separated by time
        string[] textGroup = textToIntroduce.Split('|');
        char[] textInChar = null;
        string finalText = "";
        for (int j = 0; j < textGroup.Length; j++)
        {
            textInChar = textGroup[j].ToCharArray();
            finalText += textGroup[j];
            for (int i = 0; i < textInChar.Length; i++)
            {
                if (shouldSkipText)
                {
                    narrationContainer.text = finalText;
                    shouldSkipText = false;
                    break;
                }
                narrationContainer.text += textInChar[i];
                yield return new WaitForSeconds(writeDelay);
            }

            //Assumes that if there's a |, it closes and what's between it is a floating number which meaning is to
            //wait for that many seconds before continuing the dialog.
            if (j + 1 < textGroup.Length)
            {
                //canSkipText = false;
                yield return new WaitForSeconds(float.Parse(textGroup[j + 1]));
                shouldSkipText = false;
                //Because the for loop does the other one
                j = j + 1;
            }
        }
        introducingText = false;
    }
}
