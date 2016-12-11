using UnityEngine;
using System.Collections;

public class RandomController : MonoBehaviour {

	public static void GetCurrentDate()
    {
        string answer = System.DateTime.Now.ToShortDateString();
    }
}
