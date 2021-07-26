using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component that allows typewriter style effect on text elements
/// </summary>
public class Typewriter: MonoBehaviour
{
    float _speed = 0.1f;
    Coroutine _typeCoroutine;

    public void Write(string text, Text textObject)
    {
        StopWriting();

        textObject.text = "";
        _typeCoroutine = StartCoroutine(Type(text, textObject));
    }

    IEnumerator Type(string text, Text textObject)
    {
        WaitForSeconds typeWait = new WaitForSeconds(_speed);
        foreach (char c in text)
        {
            textObject.text = textObject.text + c;
            yield return typeWait;
        }
    }

    public void StopWriting()
    {
        if (_typeCoroutine != null)
        {
            StopCoroutine(_typeCoroutine);
        }
    }
}
