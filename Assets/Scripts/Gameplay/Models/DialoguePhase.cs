using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialoguePhase
{
    [TextArea]
    public string[] dialogues;
    public bool completed;
    public bool started;
}
