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
    bool _isWriting = false;
    Coroutine _typeCoroutine;

    string _currText = "";
    Text _currTextObj;

    public bool IsWriting
    {
        get { return _isWriting; }
    }

    public void Write(string text, Text textObject)
    {
        StopWriting();

        _isWriting = true;
        _currText = text;
        _currTextObj = textObject;

        textObject.text = "";
        _typeCoroutine = StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        WaitForSeconds typeWait = new WaitForSeconds(_speed);
        foreach (char c in _currText)
        {
            _currTextObj.text += c;
            yield return typeWait;
        }

        _isWriting = false;
    }

    public void FinishWriting()
    {
        StopWriting();

        _currTextObj.text = _currText;
    }


    public void StopWriting()
    {
        _isWriting = false;
        if (_typeCoroutine != null)
        {
            StopCoroutine(_typeCoroutine);
        }
    }
}
