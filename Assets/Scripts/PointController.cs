﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointController : MonoBehaviour {

    public AudioSource obtainPointsSFX;

    public Text pointsText;

    public GameObject pointObject;

    public Canvas UI;

    [Tooltip("Offset the points will appear in the X axis")]
    public float pointOffset;

    private Camera cam;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    public void AddPoints(Vector3 position, int points)
    {
        Vector3 target = new Vector3(position.x + pointOffset, position.y, position.z);
        //target = cam.WorldToScreenPoint(target);
        GameObject instantiated = (GameObject) Instantiate(pointObject,target , Quaternion.identity);
        instantiated.transform.SetParent(UI.transform, false);
        instantiated.GetComponent<Text>().text = points + "";
        GameModel.currentPoints += points;
        pointsText.text = "" + GameModel.currentPoints;
        obtainPointsSFX.Play();
    }
}
