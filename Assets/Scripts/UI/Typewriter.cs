using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typewriter: MonoBehaviour
{
    float _speed = 0.1f;

    public void Write(string text, Text textObject)
    {
        Debug.Log("Writing");
        textObject.text = "";
        StartCoroutine(Type(text, textObject));
    }

    IEnumerator Type(string text, Text textObject)
    {
        WaitForSeconds typeWait = new WaitForSeconds(_speed);
        foreach (char c in text)
        {
            Debug.Log("Adding a letter");
            textObject.text = textObject.text + c;
            yield return typeWait;
        }
    }
}
