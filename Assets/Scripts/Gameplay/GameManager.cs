using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Where game phase data is created in the inspector
    [SerializeField] GameData _gameData;
    bool _gameOver;

    GamePhase _currentPhase;

    // Singleton instance
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                GameObject roomManager = GameObject.Find("GameManager");

                if (!roomManager)
                {
                    Debug.LogError("Must have one enabled GameManager object in the scene");
                }

                _instance = roomManager.GetComponent<GameManager>();
            }

            return _instance;
        }
    }
    public GamePhase CurrentPhase
    {
        get { return _currentPhase; }
    }

    public bool GameOver
    {
        get { return _gameOver; }
    }

    public GameData GameData
    {
        get { return _gameData; }
    }

    public void StartGame()
    {
        Debug.Log("Time to start game");
        EventManager.TriggerEvent(EventName.StartDialogue, null);
    }

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentPhase = _gameData.gamePhases[0];
        _gameOver = false;

        // EventManager.StartListening(EventName.CompletePhase, HandleNextPhase);
        EventManager.StartListening(EventName.CompleteDialogue, HandleDialogueComplete);
        EventManager.StartListening(EventName.CompleteArea, HandleAreaComplete);

        // Temporary until game menus are built
        StartCoroutine(TempGameStart());
    }

    IEnumerator TempGameStart()
    {
        yield return new WaitForSeconds(2f);

        StartGame();
    }

    /// <summary>
    /// Handles the end of the first half of the phase
    /// </summary>
    /// <param name="msg">null</param>
    void HandleDialogueComplete(Dictionary<string, object> msg)
    {
        _currentPhase.dialoguePhase.completed = true;
        _currentPhase.areaPhase.started = true;

        EventManager.TriggerEvent(EventName.StartArea, null);
    }

    /// <summary>
    /// Handles end of 2nd half of the phase
    /// </summary>
    /// <param name="msg">null</param>
    void HandleAreaComplete(Dictionary<string, object> msg)
    {
        _currentPhase.areaPhase.completed = true;

        NextPhase();
    }

    /// <summary>
    /// Updates _currentPhase
    /// </summary>
    void NextPhase()
    {
        GamePhase[] phases = _gameData.gamePhases;
        int phaseIndex = Array.IndexOf(phases, _currentPhase);
        if (phaseIndex == phases.Length)
        {
            _gameOver = true;

            Debug.Log("Game Over");
            EventManager.TriggerEvent(EventName.GameOver, null);

            return;
        }

        int nextPhase = phaseIndex + 1;
        _currentPhase = phases[nextPhase];

        EventManager.TriggerEvent(EventName.StartDialogue, null);
    }
}
