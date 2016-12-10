using UnityEngine;
using System.Collections;

public class GameModel : MonoBehaviour {

    static string currentName;

    public static string playerName
    {
        get
        {
            return currentName;
        }
        set
        {
            currentName = value;
        }
    }
}
