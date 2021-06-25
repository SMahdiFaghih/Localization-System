using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RTLTMPro;

[System.Serializable]
public class Localize : MonoBehaviour
{
    public enum GridLayoutStartCorner
    {
        LeftRight,
        UpLow
    }

    public enum TargetComponent
    {
        RTLText,
        AudioSource,
        Image,
        Font,
        FontAsset,
        GridLayoutGroup,
        Position2D
    }

    public TargetComponent Target;

    [HideInInspector] public AudioClip[] AudioClips;
    [HideInInspector] public Sprite[] Sprites;
    [HideInInspector] public Font[] Fonts;
    [HideInInspector] public TMP_FontAsset[] FontAssets;
    [HideInInspector] public GridLayoutStartCorner StartCorner;
    [HideInInspector] public Vector2[] Positions;
    [HideInInspector] public LocalizationManager.Outline Outline;
    [HideInInspector] public bool FixedFontAsset = false;
    [HideInInspector] public bool IsContainsAtSign = false;

    [SerializeField] private LocalizedString LocalizedString;

    public bool ValueSetBefore = false;

    void Start()
    {
        int currentLanguageIndex = (int)LocalizationManager.GetCurrentLanguage();
        ApplyLocalization(currentLanguageIndex, false);
    }

    public void ApplyLocalization(int currentLanguageIndex, bool editMode)
    {
        if (ValueSetBefore)
        {
            return;
        }

        switch (Target)
        {
            case TargetComponent.RTLText:
                SetTextValue(currentLanguageIndex, editMode);
                break;
            case TargetComponent.Image:
                Image Image = GetComponent<Image>();
                Image.sprite = Sprites[currentLanguageIndex];
                break;
            case TargetComponent.AudioSource:
                AudioSource AudioSource = GetComponent<AudioSource>();
                AudioSource.clip = AudioClips[currentLanguageIndex];
                AudioSource.Play();
                break;
            case TargetComponent.Font:
                Text text = GetComponent<Text>();
                text.font = Fonts[currentLanguageIndex];
                break;
            case TargetComponent.FontAsset:
                RTLTextMeshPro RTLText = GetComponent<RTLTextMeshPro>();
                RTLText.font = FontAssets[currentLanguageIndex];
                break;
            case TargetComponent.GridLayoutGroup:
                SetGridLayoutStartCorner(currentLanguageIndex);
                break;
            case TargetComponent.Position2D:
                gameObject.transform.localPosition = Positions[currentLanguageIndex];
                break;
        }

        enabled = false;
        enabled = true;
    }

    private void SetTextValue(int currentLanguageIndex, bool editMode = false)
    {
        RTLTextMeshPro RTLTextMeshPro = GetComponent<RTLTextMeshPro>();
        if (RTLTextMeshPro == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(LocalizedString.key))
        {
            string value;
            if (editMode)
            {
                value = LocalizationManager.GetLocalizedValue(LocalizedString.key, (LocalizationManager.LocalizedLanguage)currentLanguageIndex);
            }
            else
            {
                value = LocalizedString.value;
            }

            if (!string.IsNullOrEmpty(value))
            {
                if (!IsContainsAtSign)
                {
                    value = value.Replace("@", System.Environment.NewLine);
                }
                RTLTextMeshPro.text = value;
            }
        }

        LocalizationManager.LocalizedLanguage currenctLanguage = (LocalizationManager.LocalizedLanguage)currentLanguageIndex;
        SetOtherSettings(RTLTextMeshPro, currenctLanguage);

        //Re-Fix
        RTLTextMeshPro.UpdateText();
    }

    private void SetOtherSettings(RTLTextMeshPro RTLTextMeshPro, LocalizationManager.LocalizedLanguage currenctLanguage)
    {
        //Set ForceFix to show number-contained texts properly
        if (currenctLanguage == LocalizationManager.LocalizedLanguage.Farsi)
        {
            RTLTextMeshPro.forceFix = true;
        }
        else
        {
            RTLTextMeshPro.forceFix = false;
        }

        //Set FontAsset and MaterialPreset to show the proper Outline
        if (!FixedFontAsset)
        {
            LocalizationManager.Instance.SetFontAndMaterial(currenctLanguage, (int)Outline, ref RTLTextMeshPro);
        }

        //Set Alignment
        if (currenctLanguage == LocalizationManager.LocalizedLanguage.Farsi && RTLTextMeshPro.alignment.ToString().Contains("Left"))
        {
            int alignmentNumber = (int)RTLTextMeshPro.alignment + 3;
            RTLTextMeshPro.alignment = (TextAlignmentOptions)alignmentNumber;
        }
        else if (currenctLanguage != LocalizationManager.LocalizedLanguage.Farsi && RTLTextMeshPro.alignment.ToString().Contains("Right"))
        {
            int alignmentNumber = (int)RTLTextMeshPro.alignment - 3;
            RTLTextMeshPro.alignment = (TextAlignmentOptions)alignmentNumber;
        }
    }
    private void SetGridLayoutStartCorner(int currentLanguageIndex)
    {
        LocalizationManager.LocalizedLanguage currenctLanguage = (LocalizationManager.LocalizedLanguage)currentLanguageIndex;
        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
        if (StartCorner == GridLayoutStartCorner.LeftRight)
        {
            if (currenctLanguage == LocalizationManager.LocalizedLanguage.Farsi && gridLayoutGroup.startCorner.ToString().Contains("Left"))
            {
                int alignmentNumber = (int)gridLayoutGroup.startCorner + 1;
                gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)alignmentNumber;
            }
            else if (currenctLanguage != LocalizationManager.LocalizedLanguage.Farsi && gridLayoutGroup.startCorner.ToString().Contains("Right"))
            {
                int alignmentNumber = (int)gridLayoutGroup.startCorner - 1;
                gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)alignmentNumber;
            }
        }
        else if (StartCorner == GridLayoutStartCorner.UpLow)
        {
            if (currenctLanguage == LocalizationManager.LocalizedLanguage.Farsi && gridLayoutGroup.startCorner.ToString().Contains("Low"))
            {
                int alignmentNumber = (int)gridLayoutGroup.startCorner - 2;
                gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)alignmentNumber;
            }
            else if (currenctLanguage != LocalizationManager.LocalizedLanguage.Farsi && gridLayoutGroup.startCorner.ToString().Contains("Up"))
            {
                int alignmentNumber = (int)gridLayoutGroup.startCorner + 2;
                gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)alignmentNumber;
            }
        }
        gridLayoutGroup.enabled = false;
        gridLayoutGroup.enabled = true;
    }

    public void SetKey(string key, params string[] replaceStrings)
    {
        LocalizedString.key = key.Trim();
        SetTextValue((int)LocalizationManager.GetCurrentLanguage());

        string value = LocalizedString.value;

        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (replaceStrings.Length != 0)
        {
            List<int> hashIndexes = GetAllCharacterIndexes('#');
            int replacedStringLen = 0;
            for (int i = 0; i < hashIndexes.Count; i++)
            {
                value = value.Remove(hashIndexes[i] + replacedStringLen, 1);
                value = value.Insert(hashIndexes[i] + replacedStringLen, replaceStrings[i]);
                replacedStringLen += replaceStrings[i].Length - 1;
            }
        }

        if (!IsContainsAtSign)
        {
            value = value.Replace("@", System.Environment.NewLine);
        }

        RTLTextMeshPro RTLTextMeshPro = GetComponent<RTLTextMeshPro>();
        RTLTextMeshPro.text = value;

        ValueSetBefore = true;
    }

    private List<int> GetAllCharacterIndexes(char character)
    {
        string value = LocalizedString.value;
        List<int> allIndexes = new List<int>();
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == character)
            {
                allIndexes.Add(i);
            }
        }
        return allIndexes;
    }

    public LocalizedString GetLocalizedString()
    {
        return LocalizedString;
    }

    void OnValidate()
    {
        int size = System.Enum.GetNames(typeof(LocalizationManager.LocalizedLanguage)).Length;
        if (AudioClips == null || AudioClips.Length != size)
        {
            System.Array.Resize(ref AudioClips, size);
        }
        if (Sprites == null || Sprites.Length != size)
        {
            System.Array.Resize(ref Sprites, size);
        }
        if (Fonts == null || Fonts.Length != size)
        {
            System.Array.Resize(ref Fonts, size);
        }
        if (FontAssets == null || FontAssets.Length != size)
        {
            System.Array.Resize(ref FontAssets, size);
        }
        if (Positions == null || Positions.Length != size)
        {
            System.Array.Resize(ref Positions, size);
        }
    }
}