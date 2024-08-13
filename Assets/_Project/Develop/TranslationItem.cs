using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TranslationItem : MonoBehaviour
{
    private TranslationItemModel _translationItemModel;
    public TranslationItemModel TranslationItemModel
    {
        get => _translationItemModel;
        set
        {
            _translationItemModel = value;
            InitializeTranslationItem(value);
        }
    }
        
    private void InitializeTranslationItem(TranslationItemModel translationItemModel)
    {
        _langFrom.text = translationItemModel.LangFrom;
        _langTo.text = translationItemModel.LangTo;

        _translationFrom.text = translationItemModel.TranslationFrom;
        _translationTo.text = translationItemModel.TranslationTo;

        if (translationItemModel.HasLanguageSpriteFrom)
        {
            _imageLangFrom.sprite = TranslationItemManager.Instance.GetLanguageSprite(translationItemModel.LanguageFromSpriteID);
        }
        if (translationItemModel.HasLanguageSpriteTo)
        {
            _imageLangTo.sprite = TranslationItemManager.Instance.GetLanguageSprite(translationItemModel.LanguageToSpriteID);
        }
    }
    [SerializeField] private TMP_Text _langFrom;
    [SerializeField] private TMP_Text _langTo;

    [SerializeField] private TMP_Text _translationFrom;
    [SerializeField] private TMP_Text _translationTo;

    [SerializeField] private Image _imageLangFrom;
    [SerializeField] private Image _imageLangTo;

    public void SetFavourite()
    {
        if (TranslationItemModel.IsFavourite)
        {
            TranslationItemModel.IsFavourite = false;
            TranslationItemManager.Instance.ItemsData.FavouriteItems.Remove(TranslationItemModel);
            TranslationItemManager.Instance.UpdateItemsView();
        }
        else
        {
            TranslationItemModel.IsFavourite = true;
            TranslationItemManager.Instance.ItemsData.FavouriteItems.Add(TranslationItemModel);
            TranslationItemManager.Instance.UpdateItemsView();
        }
    }
}

[Serializable]
public class TranslationItemModel
{
    public string LangFrom;
    public string LangTo;

    public string TranslationFrom;
    public string TranslationTo;

    public int LanguageFromSpriteID;
    public int LanguageToSpriteID;

    public bool HasLanguageSpriteFrom;
    public bool HasLanguageSpriteTo;

    public bool IsFavourite;

    public TranslationItemModel(string langFrom, string langTo, string translationFrom, string translationTo, int languageFromSpriteID, int languageToSpriteID, bool hasLanguageSpriteFrom, bool hasLanguageSpriteTo)
    {
        LangFrom = langFrom;
        LangTo = langTo;

        TranslationFrom = translationFrom;
        TranslationTo = translationTo;

        LanguageFromSpriteID = languageFromSpriteID;
        LanguageToSpriteID = languageToSpriteID;

        HasLanguageSpriteFrom = hasLanguageSpriteFrom;
        HasLanguageSpriteTo = hasLanguageSpriteTo;
    }
}
