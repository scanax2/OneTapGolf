using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventWithString : UnityEvent<string> { }
[System.Serializable]
public class UnityEventWithBool : UnityEvent<bool> { }

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public enum StateType
    {
        DEFAULT,      // Fall-back state, should never happen
        STARTGAME,    // On start of new game
        STARTROUND,   // On start of turn (random place flag)
        PLAYING,      // While left click is pressed and held
        ENDROUND,     // On end of turn
        GAMEOVER,     // Player lost
        WAITING,      // Waiting for the user action (e.g. click button)
        WAITINGBALL   // Waiting for the ball actions
    }
    private StateType state;

    [SerializeField] private float startNewRoundDelay;
    [SerializeField] private float parabolaStartSpeed;
    [SerializeField] private float parabolaSpeedIncrease;

    [SerializeField] private UnityEvent startGameEvent;
    [SerializeField] private UnityEvent startTurnEvent;
    [SerializeField] private UnityEvent startPlayingEvent;
    [SerializeField] private UnityEventWithString endTurnEvent;
    [SerializeField] private UnityEventWithString gameOverEvent;

    private int currentScore = 0;
    private int bestScore;

    private void Start()
    {
        state = StateType.STARTGAME;

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
    public void StartNewGame()
    {
        SetState(StateType.STARTGAME);
    }
    public void ResetGame()
    {
        BallController.GetInstance().SetParabolaSpeed(parabolaStartSpeed);
        currentScore = 0;
    }

    public IEnumerator StartNewRound(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetState(StateType.STARTROUND);
    }

    private void Update()
    {
        switch (state)
        {
            case StateType.STARTGAME:
                startGameEvent.Invoke();
                ResetGame();
                SetState(StateType.STARTROUND);
                break;

            case StateType.STARTROUND:
                startTurnEvent.Invoke();
                SetState(StateType.PLAYING);
                break;

            case StateType.PLAYING:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    startPlayingEvent.Invoke();
                    SetState(StateType.WAITINGBALL);
                }
                break;

            case StateType.WAITINGBALL:
                break;

            case StateType.GAMEOVER:
                bool highscore = false;
                if (bestScore < currentScore)
                {
                    bestScore = currentScore;
                    highscore = true;
                }
                var message = ($"SCORE: {currentScore} BEST: {bestScore}");
                if (highscore)
                {
                    message += "\n New highscore !";
                }
                gameOverEvent.Invoke(message);
                SetState(StateType.WAITING);
                break;

            case StateType.ENDROUND:
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

    }
}
