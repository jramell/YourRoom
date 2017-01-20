using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointController : MonoBehaviour {

    public AudioSource obtainPointsSFX;

    public Text pointsText;

    public GameObject pointObject;

    public Canvas UI;

    [Tooltip("Offset the points will appear in the X axis")]
    public float pointOffset = 5f;

    private Camera cam;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }



    public void AddPoints(Vector3 position, int points)
    {
        Vector3 target = new Vector3(position.x, position.y, position.z);
        //target = Input.mousePosition;
        //target = cam.ScreenToViewportPoint(target);
        GameObject instantiated = (GameObject) Instantiate(pointObject);
        instantiated.transform.SetParent(UI.transform, false);
        Debug.Log("viewport: " + target);
        target = cam.WorldToScreenPoint(target);
        //target = Vector3.zero;
        instantiated.transform.position = target;
        instantiated.GetComponent<Text>().text = points + "";
        GameModel.currentPoints += points;
        pointsText.text = "" + GameModel.currentPoints;
        obtainPointsSFX.Play();
    }
}
