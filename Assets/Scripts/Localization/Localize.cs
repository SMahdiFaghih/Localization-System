using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using RTLTMPro;

[System.Serializable]
public class Localize : MonoBehaviour
{
    public enum TargetComponent
    {
        RTLText,
        AudioSource,
        Image,
        Font,
        FontAsset
    }

    public TargetComponent Target;

    [HideInInspector]
    public AudioClip[] AudioClips;
    [HideInInspector]
    public Sprite[] Sprites;
    [HideInInspector]
    public Font[] Fonts;
    [HideInInspector]
    public TMP_FontAsset[] FontAssets;
    public LocalizedString LocalizedString;

    public bool ValueSetBefore = false;

    void Start()
    {
        if (!ValueSetBefore)
        {
            int currentLanguageIndex = (int)LocalizationManager.GetCurrentLanguage();
            switch (Target)
            {
                case TargetComponent.AudioSource:
                    AudioSource AudioSource = GetComponent<AudioSource>();
                    AudioSource.clip = AudioClips[currentLanguageIndex];
                    AudioSource.Play();
                    break;
                case TargetComponent.Image:
                    Image Image = GetComponent<Image>();
                    Image.sprite = Sprites[currentLanguageIndex];
                    break;
                case TargetComponent.RTLText:
                    if (LocalizedString.key != string.Empty)
                    {
                        RTLTextMeshPro RTLTextMeshPro = GetComponent<RTLTextMeshPro>();
                        if (RTLTextMeshPro != null)
                        {
                            RTLTextMeshPro.text = LocalizedString.value;
                        }
                        else
                        {
                            PersianText persianText = GetComponent<PersianText>();
                            if (persianText != null)
                            {
                                persianText._rawText = LocalizedString.value;
                                persianText.enabled = false;
                                persianText.enabled = true;
                            }
                        }
                    }
                    break;
                case TargetComponent.Font:
                    Text text = GetComponent<Text>();
                    text.font = Fonts[currentLanguageIndex];
                    break;
                case TargetComponent.FontAsset:
                    RTLTextMeshPro RTLText = GetComponent<RTLTextMeshPro>();
                    RTLText.font = FontAssets[currentLanguageIndex];
                    break;
            }
        }
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
    }
}