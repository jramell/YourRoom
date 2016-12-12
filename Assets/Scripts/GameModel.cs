using UnityEngine;
using System.Collections;

public class GameModel : MonoBehaviour {

    static string currentName;

    static string birth;

    static int points;

    /// <summary>
    /// Which is the current event in the game? 
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

    public static int currentPoints
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    /// <summary>
    /// Resets the state to the starting level state. If the game had multiple levels, it would reset it to the last checkpoint saved state
    /// </summary>
    public static void ResetLevelState()
    {
        currentPoints = 0;
    }
}
