using System.Collections;
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

    // Complete Area Confirmation Prompt
    [SerializeField] RectTransform _confirmPrompt;
    [SerializeField] Vector2 _confirmShowPos;
    Vector2 _confirmHidePos;

    // Continue Button
    [SerializeField] RectTransform _continueAreaBtn;

    #endregion

    #region Methods

    private void Start()
    {
        _confirmHidePos = _confirmPrompt.anchoredPosition;
        _optionsHidePos = _optionsBtns.anchoredPosition;
    }

    public void AnimateHUDIn()
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(() => AnimateConfirmPromptOut(0f));
        //seq.append(() => AnimateContinueBtnIn());
        seq.append(() => gameObject.SetActive(true));
        seq.append(() => AnimateOptionBtnsIn());
    }

    public void AnimateHUDOut()
    {
        LTSeq seq = LeanTween.sequence();

        seq.append(() => AnimateConfirmPromptOut(0f));
        seq.append(() => gameObject.SetActive(false));
    }

    public void AnimateConfirmPromptIn(float _time = 1f)
    {
        LeanTween.move(_confirmPrompt, _confirmShowPos, _animTime).setEaseOutBack();
    }

    public void AnimateConfirmPromptOut(float _time = 1f)
    {
        LeanTween.move(_confirmPrompt, _confirmHidePos, _time).setEaseInBack();
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
