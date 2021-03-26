using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public enum StateType
    {
        DEFAULT,      // Fall-back state, should never happen
        STARTTURN,    // On start of turn (random place flag)
        PLAYING,      // While a button is pressed and held
        BALLWAIT,     // Waiting for ball to stop after button released
        ENDTURN,      // On end of turn
        WAITING       // Waiting for the user action (e.g. click button)
    }
    private StateType state = StateType.STARTTURN;

    public static GameController GetInstance()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = new GameController();
        }
        return instance;
    } 
    public void SetState(StateType newState)
    {
        state = newState;
    }

    private void Update()
    {
        switch (state)
        {
            case StateType.STARTTURN:
                //...
                SetState(StateType.PLAYING);
                break;
            case StateType.PLAYING:
                //...
                break;
            case StateType.BALLWAIT:
                //...
                break;
            case StateType.ENDTURN:
                //...
                SetState(StateType.WAITING);
                break;
            case StateType.WAITING:
                break;
            default:
                Debug.Log("Unknown game state: " + state);
                break;
        }
    }
}
