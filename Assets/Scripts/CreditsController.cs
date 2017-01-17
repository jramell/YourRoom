using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsController : MonoBehaviour {

    /// <summary>
    /// Time in seconds the credits will wait before moving to the next screen
    /// </summary>
    const float TIME_TO_NEXT_CREDIT = 4.5f;

    public GameObject[] credits;

    public GameObject skipText;

    private int current;

    private int maxLength;

    bool skipWait;

    float sumWait;

    float waitGranularity;

    void Awake()
    {
        maxLength = credits.Length;
        waitGranularity = 0.01f;
    }

    void Start()
    {
        StartCoroutine(AutoCredits());
    }

	void Update () {
	    if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(1))
        {
            if(!skipWait)
            {
                IncreaseCurrent();
            }
        }
	}

    void IncreaseCurrent()
    {
        credits[current].SetActive(false);
        current++;
        if (current < maxLength)
        {
            credits[current].SetActive(true);
            sumWait = 0;
        }

        else
        {
            skipText.SetActive(false);
            SceneManager.LoadScene("1_Title");
        }
    }

    IEnumerator AutoCredits()
    {
        while (current < maxLength)
        {
            yield return new WaitForSeconds(waitGranularity);
            sumWait += waitGranularity;
            if (sumWait >= TIME_TO_NEXT_CREDIT)
            {
                IncreaseCurrent();
            }
        }
    }
}
