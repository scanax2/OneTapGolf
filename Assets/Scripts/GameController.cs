using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventWithString : UnityEvent<string> { }

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public enum StateType
    {
        DEFAULT,      // Fall-back state, should never happen
        STARTTURN,    // On start of turn (random place flag)
        PLAYING,      // While a left click is pressed and held
        GAMEOVER,     // Player is lost
        ENDTURN,      // On end of turn
        WAITING       // Waiting for the user action (e.g. click button)
    }
    private StateType state = StateType.STARTTURN;

    [SerializeField] private float startNewRoundDelay;
    [SerializeField] private float parabolaSpeedIncrease;
    [SerializeField] private UnityEvent startTurnEvent;
    [SerializeField] private UnityEvent startPlayingEvent;
    [SerializeField] private UnityEventWithString endTurnEvent;
    [SerializeField] private UnityEventWithString gameOverEvent;

    private int currentScore = 0;
    private int bestScore;

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
    public StateType GetState()
    {
        return state;
    }
    public void ClearScore()
    {
        currentScore = 0;
    }

    public IEnumerator StartNewRound(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetState(StateType.STARTTURN);
    }
    public void StartNewRound()
    {
        SetState(StateType.STARTTURN);
    }

    private void Update()
    {
        switch (state)
        {
            case StateType.STARTTURN:
                startTurnEvent.Invoke();
                SetState(StateType.PLAYING);
                break;

            case StateType.PLAYING:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    startPlayingEvent.Invoke();
                    SetState(StateType.WAITING);
                }
                break;

            case StateType.GAMEOVER:
                SetState(StateType.WAITING);
                if (bestScore < currentScore)
                {
                    bestScore = currentScore;
                }
                string message = ($"SCORE: {currentScore} BEST: {bestScore}");
                gameOverEvent.Invoke(message);
                break;

            case StateType.ENDTURN:
                currentScore += 1;
                endTurnEvent.Invoke(currentScore.ToString());
                BallController.GetInstance().IncreaseParabolaSpeed(parabolaSpeedIncrease);

                SetState(StateType.WAITING);
                StartCoroutine(StartNewRound(startNewRoundDelay));
                break;

            case StateType.WAITING:
                break;

            default:
                Debug.Log("Unknown game state: " + state);
                break;

        }
        Debug.Log(state);
    }
}
