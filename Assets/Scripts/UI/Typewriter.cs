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
    #region Fields

    // Typewriter animation support
    float _speed = 0.1f;
    bool _isWriting = false;
    Coroutine _typeCoroutine;

    // Cache text to be written & text object
    string _currText = "";
    Text _currTextObj;

    #endregion

    #region Properties

    public bool IsWriting
    {
        get { return _isWriting; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Starts writing text to screen
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <param name="textObject">Text object in scene</param>
    public void Write(string text, Text textObject)
    {
        StopWriting();

        _isWriting = true;
        _currText = text;
        _currTextObj = textObject;

        textObject.text = "";
        _typeCoroutine = StartCoroutine(Type());
    }

    /// <summary>
    /// Modifies text object in scene over time
    /// </summary>
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

    /// <summary>
    /// Finishes typing - prints text to screen
    /// </summary>
    public void FinishWriting()
    {
        StopWriting();

        _currTextObj.text = _currText;
    }

    /// <summary>
    /// Stops typing loop
    /// </summary>
    public void StopWriting()
    {
        _isWriting = false;
        if (_typeCoroutine != null)
        {
            StopCoroutine(_typeCoroutine);
        }
    }

    #endregion
}
