using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Localization
{
    [System.Serializable]
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance;

        public static List<string> LanguageDescriptions;
        public static bool IsInitialized;
        public static CSVLoader csvLoader;

        private static Dictionary<string, string> LocalizedFarsiTexts;
        private static Dictionary<string, string> LocalizedEnglishTexts;

        private static LocalizedLanguage CurrentLanguage;

        [Header("English")]
        [HideInInspector] public FontAssetDetails EnglishFontAssetDetails;
        [Header("Farsi")]
        [HideInInspector] public FontAssetDetails FarsiFontAssetDetails;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);

            LanguageDescriptions = new DescriptionAttributes<LocalizedLanguage>().Descriptions.ToList();

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

        public static void Initialize()
        {
            csvLoader = new CSVLoader();
            csvLoader.LoadCSV();

            UpdateDictionaries();

            IsInitialized = true;
        }

        public static Dictionary<string, string> GetDictionaryForEditor()
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            return LocalizedEnglishTexts;
        }

        public static void UpdateDictionaries()
        {
            LocalizedFarsiTexts = csvLoader.GetDictionaryValues("farsi");
            LocalizedEnglishTexts = csvLoader.GetDictionaryValues("english");
        }

        public static LocalizedLanguage GetCurrentLanguage()
        {
            return CurrentLanguage;
        }

        public static string GetLocalizedValue(string key, LocalizedLanguage language)
        {
            if (!IsInitialized)
            {
                Initialize();
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

        public static string GetLocalizedValueByCurrentLanguage(string key)
        {
            return GetLocalizedValue(key, CurrentLanguage);
        }

        public FontAssetDetails GetFontAssetDetailsByLanguage(LocalizedLanguage language)
        {
            switch (language)
            {
                case LocalizedLanguage.English:
                    return EnglishFontAssetDetails;
                case LocalizedLanguage.Farsi:
                    return FarsiFontAssetDetails;
                default:
                    return null;
            }
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

        #region Editor
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
#endif
        #endregion

        void OnValidate()
        {
            int size = System.Enum.GetNames(typeof(Outline)).Length;
            if (EnglishFontAssetDetails.MaterialPresets == null || EnglishFontAssetDetails.MaterialPresets.Length != size)
            {
                System.Array.Resize(ref EnglishFontAssetDetails.MaterialPresets, size);
            }
            if (FarsiFontAssetDetails.MaterialPresets == null || FarsiFontAssetDetails.MaterialPresets.Length != size)
            {
                System.Array.Resize(ref FarsiFontAssetDetails.MaterialPresets, size);
            }
        }
    }
}
