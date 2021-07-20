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

    void Start()
    {
        _dialogueCounter = 0;

        EventManager.StartListening(EventName.StartDialogue, StartDialogue);
    }

    void Awake()
    {
        _dialogueView = GameObject.FindWithTag("DialogueView");
        _dialogueText = GameObject.FindWithTag("DialogueText").GetComponent<Text>();
    }

    void Update()
    {

    }

    void StartDialogue(Dictionary<string, object> msg)
    {
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
        _currentDialoguePhase = GameManager.Instance.CurrentPhase.dialoguePhase;

        // Typewriter.Type(_currentDialoguePhase.dialogues[_dialogueCounter], _dialogueText);
        _dialogueText.text = _currentDialoguePhase.dialogues[_dialogueCounter];

    }

    void ShowDialogueView()
    {
        // To do - animate in?
        Debug.Log("Starting Dialogue");
        _dialogueView.SetActive(true);
    }

    public void HandleContinueDialogueBtnClick()
    {
        ContinueDialogue();
    }
}
