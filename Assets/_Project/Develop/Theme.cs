using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Theme : MonoBehaviour
{
    private int _theme { get => PlayerPrefs.GetInt(nameof(_theme), 0); set { PlayerPrefs.SetInt(nameof(_theme), value); SetTheme(value);  } }

    [Header("Images")]
    [SerializeField] private Image[] _coloringImages;
    [SerializeField] private Image[] _spritingImages;

    [Header("TMP_Texts")]
    [SerializeField] private TMP_Text[] _textes;

    [Header("Theme Sprite resources")]
    [SerializeField] private Sprite[] _whiteThemeSprites;
    [SerializeField] private Sprite[] _blackThemeSprites;

    [Header("Buttons")]
    [SerializeField] private Button _setThemeButton;

    private void Awake()
    {
        _theme = _theme;
        _setThemeButton.onClick.AddListener(ThemeButton);
    }

    private void ThemeButton()
    {
        if(_theme == 0) _theme = 1;
        else _theme = 0;
    }

    private void SetTheme(int theme)
    {
        if(theme == 0)
        {
            foreach (var image in _coloringImages) image.color = Color.black;

            foreach (var text in _textes) text.color = Color.black;

            for (int i = 0; i < _spritingImages.Length; i++)
            {
                _spritingImages[i].sprite = _whiteThemeSprites[i];
            }
        }
        else
        {
            foreach (var image in _coloringImages) image.color = Color.white;

            foreach (var text in _textes) text.color = Color.white;

            for (int i = 0; i < _spritingImages.Length; i++)
            {
                _spritingImages[i].sprite = _blackThemeSprites[i];
            }
        }
    }
}
