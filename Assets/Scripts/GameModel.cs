using UnityEngine;
using System.Collections;

public class GameModel : MonoBehaviour {

    static string currentName;

    static string birth;

    /// <summary>
    /// Where in the game is the player right now? 
    /// </summary>
    static int currentGameState;

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

    public static string birthDate
    {
        get
        {
            return birth;
        }
        set
        {
            birth = value;
        }
    }

    public static int gameState
    {
        get
        {
            return currentGameState;
        }
        set
        {
            currentGameState = value;
        }
    }
}
