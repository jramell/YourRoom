using UnityEngine;

/// <summary>
/// Handles 2D parallaxing of all the backgrounds added to its backgrounds array. To use it add it to the Main Camera and indicate which backgrounds to parallax.
/// Does NOT make any assumptions on the order the backgrounds are rendered. This must be set using Unity's layering system for each background. 
/// Provided backgrounds "z" position is taken as their "depth" in the parallax calculations. Does not work correctly with negative depth values and backgrounds 
/// with zero depth will not parallax. This script was made for the Unity game engine.
/// </summary>
public class ParallaxController : MonoBehaviour
{
    //--------------------------------------------------------------------------------------------------------------------------------------------------------
    // Public variables
    //--------------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Backgrounds to be parallaxed
    /// </summary>
    [Tooltip("Backgrounds to be parallaxed")]
    public GameObject[] backgrounds;

    /// <summary>
    /// Do you want to parallax vertically your backgrounds? Unchecked by default
    /// </summary>
    [Tooltip("Should backgrounds parallax vertically?")]
    public bool parallaxVertically = false;

    /// <summary>
    /// If checked, spawns backgrounds instances automatically around the camera position. 
    /// If unchecked, spawns backgrounds instances using the position of the instance provided. Checked by default.
    /// </summary>
    [Tooltip("Should backgrounds spawn around camera or around original background in scene?")]
    public bool spawnBackgroundsAroundCamera = true;

    //--------------------------------------------------------------------------------------------------------------------------------------------------------
    // Private variables
    //--------------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// The camera position in the LateUpdate of the last frame.
    /// </summary>
    private Vector3 previousCameraPosition;

    /// <summary>
    /// Stored in an array so we don't have to call backgrounds[i].transform.position.z each time we use them (in every LateUpdate, mainly).
    /// In situations where memory is critical the script may be rewritten without it.
    /// </summary>
    private float[] parallaxScales;

    /// <summary>
    /// Stores how many backgrounds there are so it isn't necessary to ask for backgrounds.Length and make a copy of it each time we use the number
    /// </summary>
    private int backgroundCount;

    /// <summary>
    /// Background sizes stored in cache for performance. An alternative would be just storing the backgrounds as SpriteRenderer[] instead of GameObject[], but that would make it
    /// necessary to use SpriteRenderer.gameObject each time we want to change its position, which would be less performant. Another alternative would be to just
    /// use GetComponent<SpriteRenderer>().bounds.size.x each frame, but that is WAY less performant.
    /// </summary>
    private float[] backgroundSizes;

    /// <summary>
    /// Each halfBackgroundSizesHalf[i] is backgroundsSizes[i]/2. It's stored in memory for performance, so we don't have to calculate backgroundSizes[i]/2 again each time we need it.
    /// </summary>
    private float[] halfBackgroundSizes;

    /// <summary>
    /// Matrix where each column j contains two instantiations of the background stored in backgrounds[j] whose positions are changed at runtime
    /// to achieve infinite scrolling effect with each background. In the last position (2), there's always the most right background, while in the
    /// first position there always is the most left one. instantiatedBackgrounds[i, 1] will be the middle one
    /// </summary>
    private GameObject[,] instantiatedBackgrounds;

    //--------------------------------------------------------------------------------------------------------------------------------------------------------
    // Methods
    //--------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        backgroundCount = backgrounds.Length;
        parallaxScales = new float[backgroundCount];
        backgroundSizes = new float[backgroundCount];
        halfBackgroundSizes = new float[backgroundCount];

        //As many columns as backgrounds there are, but only 2 instiantiations per background.
        instantiatedBackgrounds = new GameObject[backgroundCount, 3];
        previousCameraPosition = transform.position;
        for (int i = 0; i < backgroundCount; i++)
        {
            parallaxScales[i] = backgrounds[i].transform.position.z;
            backgroundSizes[i] = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x;
            halfBackgroundSizes[i] = backgroundSizes[i] / 2;
        }

        InstantiateScrollBackgrounds();
    }

    void LateUpdate()
    {
        Parallax();
    }

    /// <summary>
    /// Handles parallaxing of all backgrounds provided to the script. Backgrounds with negative "z" (depth) won't parallax correctly. Backgrounds with z=0 won't parallax.
    /// </summary>
    void Parallax()
    {
        float deltaX = transform.position.x - previousCameraPosition.x;
        float deltaY = 0f;

        //If parallax vertically is not checked, deltaY will always be zero, so deltaY * parallaxScales[i] will too.
        if (parallaxVertically)
        {
            deltaY = transform.position.y - previousCameraPosition.y;
        }

        for (int i = 0; i < backgroundCount; i++)
        {
            Vector3 target = Vector3.zero;
            for (int j = 0; j < 3; j++)
            {
                target = new Vector3(instantiatedBackgrounds[i, j].transform.position.x + deltaX * parallaxScales[i], instantiatedBackgrounds[i, j].transform.position.y + deltaY * parallaxScales[i], instantiatedBackgrounds[i, j].transform.position.z);
                //Using Time.deltaTime as an interpolant means movement is not linear, but gets slower as distance with the target diminishes. It also means the amount of 
                //movement of the backgrounds depends on framerate, but that does not affect overall functionality as backgrounds still change positions to seem seamless.
                instantiatedBackgrounds[i, j].transform.position = Vector3.Lerp(instantiatedBackgrounds[i, j].transform.position, target, Time.deltaTime);
            }
        }

        previousCameraPosition = transform.position;

        ScrollBackground(deltaX, deltaY);
    }

    /// <summary>
    /// Repositions the instances of the backgrounds provided as the camera moves to make them seem seamless
    /// </summary>
    /// <param name="deltaX">The position in X the camera moved since LastUpdate</param>
    /// <param name="deltaY">The position in Y the camera moved since LastUpdate</param>
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
            if (transform.position.x > instantiatedBackgrounds[i, 2].transform.position.x - halfBackgroundSizes[i])
            {
                instantiatedBackgrounds[i, 0].transform.position = new Vector3(instantiatedBackgrounds[i, 2].transform.position.x + backgroundSizes[i], instantiatedBackgrounds[i, 2].transform.position.y, instantiatedBackgrounds[i, 2].transform.position.z);
                GameObject temp = instantiatedBackgrounds[i, 0];
                instantiatedBackgrounds[i, 0] = instantiatedBackgrounds[i, 1];
                instantiatedBackgrounds[i, 1] = instantiatedBackgrounds[i, 2];
                instantiatedBackgrounds[i, 2] = temp;
            }
        }
    }

    void ScrollLeft()
    {
        for (int i = 0; i < backgroundCount; i++)
        {
            if (transform.position.x < instantiatedBackgrounds[i, 1].transform.position.x - halfBackgroundSizes[i])
            {
                GameObject temp = instantiatedBackgrounds[i, 2];
                temp.transform.position = new Vector3(instantiatedBackgrounds[i, 0].transform.position.x - backgroundSizes[i], instantiatedBackgrounds[i, 0].transform.position.y, instantiatedBackgrounds[i, 0].transform.position.z);
                instantiatedBackgrounds[i, 2] = instantiatedBackgrounds[i, 1];
                instantiatedBackgrounds[i, 1] = instantiatedBackgrounds[i, 0];
                instantiatedBackgrounds[i, 0] = temp;
            }
        }
    }

    /// <summary>
    /// Instantiates two copies of each background adjacent to the original so they are reordered at runtime. If spawnBackgroundsAroundCamera is checked,
    /// copies spawn around the camera's position. If not, copies spawn around the original background's position.
    /// </summary>
    void InstantiateScrollBackgrounds()
    {
        Vector3 targetPosition = Vector3.zero;
        //Instantiates using transform.position as origin
        if (spawnBackgroundsAroundCamera)
        {
            for (int i = 0; i < backgroundCount; i++)
            {
                targetPosition = new Vector3(transform.position.x - backgroundSizes[i], transform.position.y, backgrounds[i].transform.position.z);
                instantiatedBackgrounds[i, 0] = Instantiate(backgrounds[i], targetPosition, Quaternion.identity);

                targetPosition = new Vector3(transform.position.x, transform.position.y, backgrounds[i].transform.position.z);
                instantiatedBackgrounds[i, 1] = Instantiate(backgrounds[i], targetPosition, Quaternion.identity);

                targetPosition = new Vector3(transform.position.x + backgroundSizes[i], transform.position.y, backgrounds[i].transform.position.z);
                instantiatedBackgrounds[i, 2] = Instantiate(backgrounds[i], targetPosition, Quaternion.identity);
            }
        }

        else
        {
            //Instantiates using backgrounds[i].transform.position as origin for each backgrounds[i]
            for (int i = 0; i < backgroundCount; i++)
            {
                targetPosition = new Vector3(backgrounds[i].transform.position.x - backgroundSizes[i], backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);
                instantiatedBackgrounds[i, 0] = Instantiate(backgrounds[i], targetPosition, Quaternion.identity);

                //Not necessary to instantiate the middle background because we're instantiating around it
                instantiatedBackgrounds[i, 1] = backgrounds[i];

                targetPosition = new Vector3(backgrounds[i].transform.position.x + backgroundSizes[i], backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);
                instantiatedBackgrounds[i, 2] = Instantiate(backgrounds[i], targetPosition, Quaternion.identity);
            }
        }
    }
}
