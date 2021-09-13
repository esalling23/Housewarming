using System;
using System.Collections.Generic;
using UnityEngine;

public class HUDAnimator : MonoBehaviour
{
    #region Fields
    [SerializeField] float _animTime = 1f;

    // World object option buttons
    [SerializeField] RectTransform _optionsBtns;
    [SerializeField] Vector2 _optionsShowPos;
    Vector2 _optionsHidePos;

    // Complete Area Button
    [SerializeField] RectTransform _continueBtn;
    [SerializeField] Vector2 _continueBtnShowPos;
    Vector2 _continueBtnHidePos;

    // Complete Area Confirmation Prompt
    [SerializeField] RectTransform _confirmPrompt;
    [SerializeField] Vector2 _confirmShowPos;
    Vector2 _confirmHidePos;

    // Cover screen for flash
    [SerializeField] GameObject _coverScreen;
    RectTransform _coverScreenRect;

    #endregion

    #region Methods

    private void Start()
    {
        _continueBtnHidePos = _continueBtn.anchoredPosition;
        _confirmHidePos = _confirmPrompt.anchoredPosition;
        _optionsHidePos = _optionsBtns.anchoredPosition;

        _coverScreenRect = _coverScreen.GetComponent<RectTransform>();
        _coverScreen.SetActive(false);
    }

    public void AnimateHUDIn()
    {
        LTSeq seq = LeanTween.sequence();

        seq.append(() => AnimateConfirmPromptOut(0f));
        seq.append(() => AnimateContinueBtnIn());
        seq.append(() => AnimateOptionBtnsIn());
    }

    public void AnimateHUDOut()
    {
        LTSeq seq = LeanTween.sequence();

        seq.append(() => AnimateConfirmPromptOut(0f));
        seq.append(() => AnimateContinueBtnOut());
    }

    public void AnimateScreenFlash(Action action)
    {
        float fadeIn = 0.6f;
        float fadeOut = 0.5f;

        LTSeq seq = LeanTween.sequence();
        seq.append(() => _coverScreen.SetActive(true));
        seq.append(() => LeanTween.color(_coverScreenRect, Color.black, fadeIn));
        seq.append(fadeIn * 2);
        seq.append(() => action());
        seq.append(() => LeanTween.color(_coverScreenRect, new Color(0, 0, 0, 0), fadeOut));
        seq.append(fadeOut);
        seq.append(() => _coverScreen.SetActive(false));
    }

    public void AnimateConfirmPromptIn(float _time = 1f)
    {
        LeanTween.move(_confirmPrompt, _confirmShowPos, _time).setEaseOutBack();
    }

    public void AnimateConfirmPromptOut(float _time = 1f)
    {
        LeanTween.move(_confirmPrompt, _confirmHidePos, _time).setEaseInBack();
    }

    public void AnimateContinueBtnIn(float _time = 1f)
    {
        LeanTween.move(_continueBtn, _continueBtnShowPos, _time).setEaseOutBack();
    }

    public void AnimateContinueBtnOut(float _time = 1f)
    {
        LeanTween.move(_continueBtn, _continueBtnHidePos, _time).setEaseInBack();
    }

    public void AnimateOptionBtnsIn(float _time = 1f)
    {
        LeanTween.move(_optionsBtns, _optionsShowPos, _time).setEaseOutBack();
    }

    public void AnimateOptionBtnsOut(float _time = 1f)
    {
        LeanTween.move(_optionsBtns, _optionsHidePos, _time).setEaseInBack();
    }

    #endregion
}
