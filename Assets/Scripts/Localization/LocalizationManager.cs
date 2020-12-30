using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.ComponentModel;
using TMPro;
using RTLTMPro;

[System.Serializable]
public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private static Dictionary<string, string> LocalizedFarsiTexts;
    private static Dictionary<string, string> LocalizedEnglishTexts;

    private static LocalizedLanguage CurrentLanguage;

    public static List<string> LanguageDescriptions;

    public static bool IsInit;

    public static CSVLoader csvLoader;

    [Header("English")]
    public TMP_FontAsset EnglishFontAsset;
    [HideInInspector]
    public Material[] EnglishMaterialPresets;
    [Header("Farsi")]
    public TMP_FontAsset FarsiFontAsset;
    [HideInInspector]
    public Material[] FarsiMaterialPresets;

    public enum LocalizedLanguage
    {
        [Description("English")]
        English,
        [Description("فارسی")]
        Farsi
    }

    public enum Outline
    {
        NoOutline,
        Black
    }

    public class DescriptionAttributes<T>
    {
        protected List<DescriptionAttribute> Attributes = new List<DescriptionAttribute>();
        public List<string> Descriptions { get; set; }

        public DescriptionAttributes()
        {
            RetrieveAttributes();
            Descriptions = Attributes.Select(x => x.Description).ToList();
        }

        private void RetrieveAttributes()
        {
            foreach (var attribute in typeof(T).GetMembers().SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>()))
            {
                Attributes.Add(attribute);
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        LanguageDescriptions = new DescriptionAttributes<LocalizedLanguage>().Descriptions.ToList();
    }

    private void Start()
    {
        string language = PlayerPrefs.GetString("Language");
        switch (language)
        {
            case "English":
                CurrentLanguage = LocalizedLanguage.English;
                break;
            case "Farsi":
            default:
                CurrentLanguage = LocalizedLanguage.Farsi;
                break;
        }

    }

    public static void Init()
    {
        csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        UpdateDictionaries();

        IsInit = true;
    }

    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!IsInit)
        {
            Init();
        }

        return LocalizedEnglishTexts;
    }

    public static void UpdateDictionaries()
    {
        LocalizedFarsiTexts = csvLoader.GetDictionaryValues("farsi");
        LocalizedEnglishTexts = csvLoader.GetDictionaryValues("english");
    }

    public static string GetLocalizedValue(string key, LocalizedLanguage language)
    {
        if (!IsInit)
        {
            Init();
        }

        string value = "";
        switch (language)
        {
            case LocalizedLanguage.Farsi:
                LocalizedFarsiTexts.TryGetValue(key, out value);
                break;
            case LocalizedLanguage.English:
                LocalizedEnglishTexts.TryGetValue(key, out value);
                break;
        }
        return value;
    }

#if UNITY_EDITOR
    public static void Add(string key, string[] values)
    {
        CheckValues(ref values);

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Add(key, values);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Replace(string key, string[] values)
    {
        CheckValues(ref values);

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Edit(key, values);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Remove(string key)
    {
        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Remove(key);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }
#endif

    private static void CheckValues(ref string[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] == null)
            {
                continue;
            }
            if (values[i].Contains("\""))
            {
                values[i].Replace('"', '\"');
            }
            values[i] = values[i].TrimStart();
            values[i] = values[i].TrimEnd();
        }
    }

    public void ChangeLanguage(string language)
    {
        if (language == CurrentLanguage.ToString())
        {
            return;
        }
        PlayerPrefs.SetString("Language", language);
        switch (language)
        {
            case "Farsi":
                CurrentLanguage = LocalizedLanguage.Farsi;
                break;
            case "English":
                CurrentLanguage = LocalizedLanguage.English;
                break;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeLanguage()
    {
        CurrentLanguage = (CurrentLanguage == LocalizedLanguage.Farsi) ? LocalizedLanguage.English : LocalizedLanguage.Farsi;
        PlayerPrefs.SetString("Language", CurrentLanguage.ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetFontAndMaterial(LocalizedLanguage language, int textTypeIndex, ref RTLTextMeshPro RTLTextMeshPro)
    {
        switch (language)
        {
            case LocalizedLanguage.English:
                RTLTextMeshPro.font = EnglishFontAsset;
                RTLTextMeshPro.fontSharedMaterial = EnglishMaterialPresets[textTypeIndex];
                break;
            case LocalizedLanguage.Farsi:
            default:
                RTLTextMeshPro.font = FarsiFontAsset;
                RTLTextMeshPro.fontSharedMaterial = FarsiMaterialPresets[textTypeIndex];
                break;
        }
    }

    public static LocalizedLanguage GetCurrentLanguage()
    {
        return CurrentLanguage;
    }

    public static void SetInstance()
    {
        /*if (Instance == null)
        {
            Instance = GameObject.FindObjectOfType<LocalizationManager>();
        }

        RTLTextMeshPro[] rtl = Resources.FindObjectsOfTypeAll<RTLTextMeshPro>();
        Color32 color = new Color32(251, 246, 221, 255);
        foreach (RTLTextMeshPro rTLTextMeshPro in rtl)
        {
            if (rTLTextMeshPro.color.r == 1 && rTLTextMeshPro.color.g == 1 && rTLTextMeshPro.color.b == 1)
            {
                rTLTextMeshPro.color = color;
            }
            //rTLTextMeshPro.alignment = TextAlignmentOptions.Midline;
            //rTLTextMeshPro.font = Instance.FarsiFontAsset;
            rTLTextMeshPro.lineSpacing = 30;
            rTLTextMeshPro.raycastTarget = false;
            /* Localize localize = rTLTextMeshPro.gameObject.GetComponent<Localize>();
            if (localize != null)
            {
                localize.ApplyLocalization(1, true);
            }
            else
            {
                rTLTextMeshPro.fontSharedMaterial = Instance.FarsiMaterialPresets[4];
            }
        }*/
    }

    void OnValidate()
    {
        int size = System.Enum.GetNames(typeof(Outline)).Length;
        if (EnglishMaterialPresets == null || EnglishMaterialPresets.Length != size)
        {
            System.Array.Resize(ref EnglishMaterialPresets, size);
        }
        if (FarsiMaterialPresets == null || FarsiMaterialPresets.Length != size)
        {
            System.Array.Resize(ref FarsiMaterialPresets, size);
        }
    }
}
