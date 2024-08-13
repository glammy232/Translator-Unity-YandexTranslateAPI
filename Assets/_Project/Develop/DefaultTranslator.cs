using System;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using TMPro;


public class DefaultTranslator : MonoBehaviour
{ 
    [SerializeField]private Data _data;

    public LanguageNames From;
    public LanguageNames To;

    //private string _apiKey = "AQVN1pjmUMgni90CmiGDLUyHPBo5aULBir4l6Viy";
    private string _url = "https://translate.api.cloud.yandex.net/translate/v2/translate";

    [Space(10)]
    [Header("Settings")]
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_InputField _outputField;
    [SerializeField] private TMP_Dropdown _dropdownFrom;
    [SerializeField] private TMP_Dropdown _dropdownTo;

    public enum Language
    {
        SQ,
        EN,
        AZ,
        AF,
        EU,
        BE,
        BG,
        BS,
        CY,
        VI,
        HU,
        HT,
        GL,
        NL,
        EL,
        DA,
        HE,
        ID,
        GA,
        IT,
        IS,
        ES,
        KK,
        CA,
        KY,
        LA,
        LV,
        LT,
        MG,
        MS,
        MT,
        MK,
        MN,
        DE,
        NO,
        PL,
        PT,
        RO,
        SR,
        SK,
        SL,
        SW,
        TG,
        TL, 
        TR,
        UZ,
        UK,
        FI,
        FR,
        HR,
        CS,
        SV,
        ET,
    };

    public enum LanguageNames
    {
        Albanian,
        English,
        Azerbaijanian,
        Afrikaans,
        Basque,
        Belarusian,
        Bulgarian,
        Bosnian,
        Welsh,
        Vietnamese,
        Hungarian,
        Haitian,
        Galician,
        Dutch,
        Greek,
        Danish,
        Hebrew,
        Indonesian,
        Irish,
        Italian,
        Icelandic,
        Spanish,
        Kazakh,
        Catalan,
        Kyrgyz,
        Latin,
        Latvian,
        Lithuanian,
        Malagasy,
        Malay,
        Maltese,
        Macedonian,
        Mongolian,
        German,
        Norwegian,
        Polish,
        Portuguese,
        Romanian,
        Serbian,
        Slovak,
        Slovenian,
        Swahili,
        Tajik,
        Tagalog,
        Turkish,
        Uzbek,
        Ukrainian,
        Finnish,
        French,
        Croatian,
        Czech,
        Swedish,
        Estonian,
    };

    private void Awake()
    {
        InitializeDropdowns();
    }

    private void InitializeDropdowns()
    {
        string[] options = Enum.GetNames(typeof(LanguageNames));
        List<string> names = new List<string>(options);

        _dropdownFrom.AddOptions(names);
        _dropdownTo.AddOptions(names);
    }

    public async void Translate()
    {
        await PostAcync(((Language)_dropdownFrom.value).ToString().ToLower(), ((Language)_dropdownTo.value).ToString().ToLower(), _inputField.text.Split(" "));
    }

    public void Swap()
    {
        int nameIdFrom = _dropdownFrom.value;
        int nameIdTo = _dropdownTo.value;

        _dropdownFrom.value = nameIdTo;
        _dropdownTo.value = nameIdFrom;
    }

    private int GetLanguageNameIndex(string name)
    {
        string[] options = Enum.GetNames(typeof(LanguageNames));

        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] == name) return i;
        }
        return -1;
    }
    private static Stream GenerateStreamFromString(string s)
    {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    private async Task PostAcync(string from, string to, string[] texts)
    {
        string json = JsonUtility.ToJson(new Content(from, to, texts));
        string myJSONRequest = json;
        HttpContent requestContent = new StreamContent(GenerateStreamFromString(myJSONRequest));

        HttpClient httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", "Api-Key AQVN1pjmUMgni90CmiGDLUyHPBo5aULBir4l6Viy");

        using HttpResponseMessage response = await httpClient.PostAsync(_url, requestContent);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            _data = (Data)JsonUtility.FromJson(jsonResponse, typeof(Data));
            _outputField.text = "";
            foreach (var str in _data.translations)
            {
                _outputField.text += $"{str.text} ";
            }
            SendNewTranslationItem();
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _inputField.text = "Not Found";
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            _inputField.text = "Frobidden";
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
        {
            _inputField.text = "Gateway Timeout";
        }
    }

    private void SendNewTranslationItem()
    {
        TranslationItemModel model = new TranslationItemModel(_dropdownFrom.captionText.text, _dropdownTo.captionText.text, _inputField.text, _outputField.text, GetLanguageNameIndex(_dropdownFrom.name), GetLanguageNameIndex(_dropdownTo.name), false, false);

        TranslationItemManager.Instance.ItemsData.TranslationItemModels.Add(model);

        TranslationItemManager.Instance.UpdateItemsView();
    }
}
public class Content
{
    public string sourceLanguageCode;
    public string targetLanguageCode;
    public string[] texts;

    public Content(string sourceLanguageCode, string targetLanguageCode, string[] texts)
    {
        this.sourceLanguageCode = sourceLanguageCode;
        this.targetLanguageCode = targetLanguageCode;
        this.texts = texts;
    }
}
[Serializable]
public class Data
{
    public List<Text> translations;
}
[Serializable]
public class Text
{
    public string text;
}