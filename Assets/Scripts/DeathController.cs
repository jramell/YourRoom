using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathController : MonoBehaviour
{

    public Image deathScreen;
    public Text deathText;

    [Tooltip("Death sound effect. If none is added, none wil play")]
    public AudioSource deathSoundEffect;

    [Tooltip("Contains the level's current background music player. The script uses this to stop it from playing")]
    public AudioSource backgroundMusic;

    [Tooltip("Time in seconds the death screen takes to fade in")]
    public float timeOfDeathScreenFade;

    [Tooltip("Time in seconds to wait before revealing the joke")]
    public float timeBeforeDeathJoke;

    [Tooltip("Message to be shown at death")]
    public string deathMessage;

    [Tooltip("Time to wait before the player is revived")]
    public float timeBeforeRevival;

    int originalFontSize;

    NarrationController narrationController;

    void Start()
    {
        narrationController = FindObjectOfType<NarrationController>();
        originalFontSize = deathText.fontSize;
    }
    public void Die()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        if (deathSoundEffect != null)
        {
            deathSoundEffect.Play();
        }

        Color tempColorTitle = deathScreen.color;
        Color tempColorText = deathText.color;
        float rateOfFade = 1 / timeOfDeathScreenFade * 0.01f;
        while (deathScreen.color.a < 1)
        {
            tempColorTitle.a += rateOfFade;
            deathScreen.color = tempColorTitle;
            tempColorText.a += rateOfFade;
            deathText.color = tempColorText;
            yield return new WaitForSeconds(rateOfFade);
        }
        yield return new WaitForSeconds(timeBeforeDeathJoke);
        StartCoroutine(DeathJoke());
    }

    IEnumerator DeathJoke()
    {
        deathText.text = "";
        deathText.color = Color.white;
        deathText.fontSize = 20;
        //Debug.Log("called in death contr");
        yield return StartCoroutine(narrationController.IntroduceText(deathMessage));
        yield return new WaitForSeconds(timeBeforeRevival);
        Restart();
    }

    //If it were a longer project, would have a SceneLoader that controlled that a certain variable always stored the correct
    //'current scene checkpoint' of the game in order to load the correct one here. However, since there is just one level in the game, this is faster.
    void Restart()
    {
        deathText.text = "";
        deathScreen.color = Color.red;
        deathText.fontSize = originalFontSize;
        Color tempColorTitle = deathScreen.color;
        Color tempColorText = deathText.color;
        tempColorText.a = 0;
        tempColorTitle.a = 0;
        deathText.color = tempColorText;
        deathScreen.color = tempColorTitle;
        SceneManager.LoadScene("4_FirstPath");
    }
}