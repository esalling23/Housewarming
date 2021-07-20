using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    GameObject _dialogueView;
    Text _dialogueText;
    DialoguePhase _currentDialoguePhase;
    int _dialogueCounter;
    Typewriter _typewriter;

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
        _typewriter = GetComponent<Typewriter>();
    }

    void Update()
    {

    }

    void StartDialogue(Dictionary<string, object> msg)
    {
        // Debug.Log("Starting dialogue");
        _dialogueCounter = 0;

        SetDialogue();
        ShowDialogueView();
    }

    void ContinueDialogue()
    {
        if (_dialogueCounter < _currentDialoguePhase.dialogues.Count() - 1)
        {
            _dialogueCounter++;
            SetDialogue();
        }
        else
        {
            _dialogueCounter = 0;
            _dialogueView.SetActive(false);
            EventManager.TriggerEvent(EventName.CompleteDialogue, null);
        }
    }

    void SetDialogue()
    {
        // Debug.Log("Setting Dialogue Phase");
        _currentDialoguePhase = GameManager.Instance.CurrentPhase.dialoguePhase;

        _typewriter.Write(_currentDialoguePhase.dialogues[_dialogueCounter], _dialogueText);
    }

    void ShowDialogueView()
    {
        // To do - animate in?
        _dialogueView.SetActive(true);
    }

    public void HandleContinueDialogueBtnClick()
    {
        ContinueDialogue();
    }
}
