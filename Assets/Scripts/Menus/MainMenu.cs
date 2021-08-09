using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages main menu options and animations
/// </summary>
public class MainMenu : MonoBehaviour
{
    #region Fields

    // Background image animation support
    [SerializeField] Image _backgroundImage;
    RectTransform _imageRect;
    Color _imageHiddenColor = new Color(0f, 0f, 0f, 0f);
    // Title & button animation support
    [SerializeField] RectTransform _title;
    [SerializeField] RectTransform _buttons;
    // Title & button animation positions
    [SerializeField] float _titleInPos;
    float _titleOutPos;
    [SerializeField] float _buttonsInPos;
    float _buttonsOutPos;

    // Default animation time
    [SerializeField] float _animTime;

    #endregion

    #region Methods

    private void Start()
    {
        // Gets starting positions for animate-out
        _titleOutPos = _title.anchoredPosition.y;
        _buttonsOutPos = _buttons.anchoredPosition.y;
        // Sets background image color to transparent
        _backgroundImage.color = _imageHiddenColor;
        _imageRect = _backgroundImage.GetComponent<RectTransform>();

        
        AnimateIn();
    }

    private void AnimateIn()
    {
        LTSeq seq = LeanTween.sequence();

        // initial delay
        seq.append(1f);

        // Display background image
        seq.append(LeanTween.color(_imageRect, Color.white, 1f));

        // Display Title
        seq.append(LeanTween.moveY(_title, _titleInPos, _animTime).setEaseOutBack());

        // Display buttons
        seq.append(LeanTween.moveY(_buttons, _buttonsInPos, _animTime).setEaseOutBack());
    }

    private void AnimateOut()
    {
        
        LTSeq seq = LeanTween.sequence();

        seq.append(1f);

        seq.append(LeanTween.moveY(_buttons, _buttonsOutPos, _animTime).setEaseInBack());

        seq.append(LeanTween.moveY(_title, _titleOutPos, _animTime).setEaseInBack());

        seq.append(LeanTween.color(_imageRect, _imageHiddenColor, _animTime + 1f));

        seq.append(1f);

        // Start game at end of animation sequence
        seq.append(() => PlayGame());
    }

    void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void HandlePlayButtonClick()
    {
        AnimateOut();
    }

    public void HandleQuitButtonClick()
    {
        Application.Quit();
    }

    #endregion
}
