using UnityEngine;
using System.Collections;

/// <summary>
/// Script added to the main camera in this implementation
/// </summary>
public class ParallaxController : MonoBehaviour
{

    public GameObject[] backgrounds;
    public float smoothing = 1f;
    public bool parallaxVertically = true;

    private Vector3 previousCameraPosition;
    //For performance reasons
    private float[] parallaxScales;
    private int backgroundCount;

    /// <summary>
    /// Stored like these for performance. An alternative would be just storing the backgrounds as SpriteRenderer[] instead of GameObject[], but that would make it
    /// necessary to use SpriteRenderer.gameObject each time we want to change its position, which is (probably) less performant. Another alternative would be to just
    /// use GetComponent<SpriteRenderer>().bounds.size.x each frame, but that is WAY less performant.
    /// </summary>
    private float[] backgroundSizes;

    /// <summary>
    /// Matrix where each column j contains two instantiations of the background stored in backgrounds[j] whose positions are changed at runtime
    /// to achieve infinite scrolling effect with each background. In the last position (2), there's always the most right background, while in the
    /// first position there always is the most left one. instantiatedBackgrounds[i, 1] will be the middle one
    /// </summary>
    private GameObject[,] instantiatedBackgrounds;

    int leftIndex = 0;
    int rightIndex = 2;

    void Start()
    {
        backgroundCount = backgrounds.Length;
        parallaxScales = new float[backgroundCount];
        backgroundSizes = new float[backgroundCount];
        //As many coulmns as backgrounds there are, only 2 instiantiations per background
        instantiatedBackgrounds = new GameObject[backgroundCount, 3];
        previousCameraPosition = transform.position;
        leftIndex = 0;
        rightIndex = 2;
        for (int i = 0; i < backgroundCount; i++)
        {
            parallaxScales[i] = backgrounds[i].transform.position.z;
            //Only works for one background (will work on that)
            backgroundSizes[i] = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x;
        }

        InstantiateScrollBackgrounds();
    }

    void LateUpdate()
    {
        Parallax();
    }

    void Parallax()
    {
        float deltaX = 0f;
        float deltaY = 0f;
        for (int i = 0; i < backgroundCount; i++)
        {
            deltaX = (transform.position.x - previousCameraPosition.x) * parallaxScales[i];

            if (parallaxVertically)
            {
                deltaY = (transform.position.y - previousCameraPosition.y) * parallaxScales[i];
            }

            // Vector3 target = new Vector3(backgrounds[i].transform.position.x + deltaX, backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);
            Vector3 target = Vector3.zero;
           // backgrounds[i].transform.position = Vector3.Lerp(backgrounds[i].transform.position, target, smoothing * Time.deltaTime);

            for (int j = 0; j < 3; j++)
            {
                target = new Vector3(instantiatedBackgrounds[i,j].transform.position.x + deltaX, instantiatedBackgrounds[i, j].transform.position.y, instantiatedBackgrounds[i, j].transform.position.z);
                instantiatedBackgrounds[i,j].transform.position = Vector3.Lerp(instantiatedBackgrounds[i,j].transform.position, target, smoothing * Time.deltaTime);
            }

            previousCameraPosition = transform.position;
        }
        ScrollBackground(deltaX, deltaY);
    }

    void ScrollBackground(float deltaX, float deltaY)
    {
        if (deltaX > 0)
        {
           ScrollRight();
        }

        else if (deltaX < 0)
        {
           ScrollLeft();
        }
    }


    void ScrollRight()
    {
        for (int i = 0; i < backgroundCount; i++)
        {
         //   Debug.Log(transform.position.x - (instantiatedBackgrounds[i, 1].transform.position.x));
            if (transform.position.x > instantiatedBackgrounds[i, rightIndex].transform.position.x - backgroundSizes[i]/2)
            {
                instantiatedBackgrounds[i, 0].transform.position = new Vector3(instantiatedBackgrounds[i, 2].transform.position.x + backgroundSizes[i], instantiatedBackgrounds[i, 2].transform.position.y, instantiatedBackgrounds[i, 2].transform.position.z);
                GameObject temp = instantiatedBackgrounds[i, 0];
                instantiatedBackgrounds[i, 0] = instantiatedBackgrounds[i, 1];
                instantiatedBackgrounds[i, 1] = instantiatedBackgrounds[i, 2];
                instantiatedBackgrounds[i, 2] = temp;
                //Debug.Log(instantiatedBackgrounds[i, 2]);
                //Debug.Log(instantiatedBackgrounds[i, 1]);
                //Debug.Log(instantiatedBackgrounds[i, 0]);
                //instantiatedBackgrounds[i, leftIndex].transform.position = new Vector3(instantiatedBackgrounds[i, rightIndex].transform.position.x + backgroundSizes[i], instantiatedBackgrounds[i, rightIndex].transform.position.y, instantiatedBackgrounds[i, rightIndex].transform.position.z);
                //rightIndex = leftIndex;
                //leftIndex++;
                //if(leftIndex == backgroundCount)
                //{
                //    leftIndex = 0;
                //}
            }
        }
    }

    void ScrollLeft()
    {
        for (int i = 0; i < backgroundCount; i++)
        {
            if (transform.position.x < instantiatedBackgrounds[i, 1].transform.position.x - backgroundSizes[i])
            {
                GameObject temp = instantiatedBackgrounds[i, 2];
                temp.transform.position = new Vector3(instantiatedBackgrounds[i, 0].transform.position.x - backgroundSizes[i], instantiatedBackgrounds[i, 0].transform.position.y, instantiatedBackgrounds[i, 0].transform.position.z);
                instantiatedBackgrounds[i, 2] = instantiatedBackgrounds[i, 1];
                instantiatedBackgrounds[i, 1] = instantiatedBackgrounds[i, 0];
                instantiatedBackgrounds[i, 0] = temp;
                Debug.Log(instantiatedBackgrounds[i, 2]);
                Debug.Log(instantiatedBackgrounds[i, 1]);
                Debug.Log(instantiatedBackgrounds[i, 3]);
            }
        }
    }

    /// <summary>
    /// Instantiates two copies of each background adjacent to the original so they are reordered at runtime 
    /// </summary>
    void InstantiateScrollBackgrounds()
    {
        Vector3 targetPosition = Vector3.zero;
        for (int i = 0; i < backgroundCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (j == 0)
                {
                    targetPosition = new Vector3(backgrounds[i].transform.position.x + backgroundSizes[i], backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);
                    instantiatedBackgrounds[i, j] = (GameObject)Instantiate(backgrounds[i], targetPosition, Quaternion.identity);
                }

                else if (j == 1)
                {
                    instantiatedBackgrounds[i, j] = backgrounds[i];
                }

                else if (j == 2)
                {
                    targetPosition = new Vector3(backgrounds[i].transform.position.x - backgroundSizes[i], backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);
                    instantiatedBackgrounds[i, j] = (GameObject)Instantiate(backgrounds[i], targetPosition, Quaternion.identity);
                }
            }
        }
    }
}
