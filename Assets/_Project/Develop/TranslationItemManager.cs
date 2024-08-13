using System;
using System.Collections.Generic;
using UnityEngine;

public class TranslationItemManager : MonoBehaviour
{
    public static TranslationItemManager Instance;

    private void Awake()
    {
        Instance = this;
        if (_json == "") return;

        DeserializeItems();
        UpdateItemsView();
    }

    [Header("Data")]
    public ItemsData ItemsData;

    [Space(5)]
    [Header("Prefab")]
    [SerializeField] private GameObject _translationItemPrefab;

    [Space(5)]
    [Header("Content Parent")]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private Transform _contentParentFavourites;

    [Space(5)]
    [Header("Sprites")]
    [SerializeField] private Sprite[] _languageSprites;

    private string _json { get => PlayerPrefs.GetString(nameof(_json), ""); set => PlayerPrefs.SetString(nameof(_json), value); }

    public void UpdateItemsView()
    {
        DestroyChildren();

        foreach (var model in ItemsData.FavouriteItems)
        {
            GameObject newItem = Instantiate(_translationItemPrefab.gameObject, _translationItemPrefab.transform.position, Quaternion.identity, _contentParentFavourites);

            newItem.GetComponent<TranslationItem>().TranslationItemModel = model;

            newItem.SetActive(true);
        }

        if (ItemsData.TranslationItemModels.Count == 0) return;

        foreach (var model in ItemsData.TranslationItemModels)
        {
            GameObject newItem = Instantiate(_translationItemPrefab.gameObject, _translationItemPrefab.transform.position, Quaternion.identity, _contentParent);

            newItem.GetComponent<TranslationItem>().TranslationItemModel = model;

            newItem.SetActive(true);
        }
    }
    private void DestroyChildren()
    {
        var children = new List<GameObject>();
        foreach (Transform child in _contentParentFavourites) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        var children0 = new List<GameObject>();
        foreach (Transform child0 in _contentParent) children0.Add(child0.gameObject);
        children0.ForEach(child0 => Destroy(child0));
    }

    public Sprite GetLanguageSprite(int id) => _languageSprites[id];

    public void SerializeItems()
    {
        _json = JsonUtility.ToJson(ItemsData);
    }
    public void DeserializeItems()
    {
        ItemsData = (ItemsData)JsonUtility.FromJson(_json, typeof(ItemsData));
    }

    private void OnApplicationQuit() => SerializeItems();
}
[Serializable]
public class ItemsData
{
    public List<TranslationItemModel> TranslationItemModels;

    public List<TranslationItemModel> FavouriteItems;
}
