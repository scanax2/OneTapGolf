using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] private UnityEvent startTurnEvent;
    [SerializeField] private UnityEvent launchBallEvent;

    private void Start()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public static GameController GetInstance()
    {
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
                Debug.Log("STARTTURN !");
                startTurnEvent.Invoke();
                SetState(StateType.PLAYING);
                break;

            case StateType.PLAYING:
                Debug.Log("PLAYING");
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Debug.Log("CLICKED !");
                    SetState(StateType.WAITING);
                    launchBallEvent.Invoke();
                }
                break;

            case StateType.BALLWAIT:
                Debug.Log("BALL WAIT !");
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
