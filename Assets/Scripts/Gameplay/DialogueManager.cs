using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the dialogue view - displaying dialogue for user
/// </summary>
public class DialogueManager : MonoBehaviour
{
    #region Fields
    GameObject _dialogueView;
    Text _dialogueText;
    DialoguePhase _currentDialoguePhase;
    int _dialogueCounter;
    Typewriter _typewriter;

    #endregion

    #region Methods

    void Start()
    {
        _dialogueCounter = 0;
        _dialogueView.SetActive(false);

        // Debug.Log("Listening for start dialogue");
        EventManager.StartListening(EventName.StartDialogue, StartDialogue);
    }

    void Awake()
    {
        _dialogueView = GameObject.FindWithTag("DialogueView");
        _dialogueText = GameObject.FindWithTag("DialogueText").GetComponent<Text>();
        _typewriter = _dialogueView.GetComponent<Typewriter>();
    }

    /// <summary>
    /// Setup to start dialogue
    /// - Resets counter
    /// - Gets current dialogue phase
    /// - Shows view and writes first dialogue
    /// </summary>
    /// <param name="msg">null</param>
    void StartDialogue(Dictionary<string, object> msg)
    {
        // Debug.Log("Starting dialogue");
        _dialogueCounter = 0;
        _currentDialoguePhase = GameManager.Instance.CurrentPhase.dialoguePhase;

        // Must show dialogue view before setting dialogue
        ShowDialogueView();
        WriteDialogue();
    }

    /// <summary>
    /// Handles moving to the next piece of dialogue or triggering the end
    /// </summary>
    void ContinueDialogue()
    {
        if (_dialogueCounter < _currentDialoguePhase.dialogues.Count() - 1)
        {
            _dialogueCounter++;
            WriteDialogue();
        }
        else
        {
            _dialogueView.SetActive(false);
            EventManager.TriggerEvent(EventName.CompleteDialogue, null);
        }
    }

    /// <summary>
    /// Resets text and displays the current piece of text
    /// </summary>
    void WriteDialogue()
    {
        _typewriter.Write(_currentDialoguePhase.dialogues[_dialogueCounter], _dialogueText);
    }

    /// <summary>
    /// Displays the dialogue view in the scene
    /// </summary>
    void ShowDialogueView()
    {
        // To do - animate in?
        _dialogueView.SetActive(true);
    }

    /// <summary>
    /// Button click handler set in inspector - continues dialogue
    /// </summary>
    public void HandleContinueDialogueBtnClick()
    {
        ContinueDialogue();
    }

    #endregion
}
