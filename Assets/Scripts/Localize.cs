using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using RTLTMPro;

[System.Serializable]
public class Localize : MonoBehaviour
{
    public enum TargetComponent
    {
        AudioSource,
        Image,
        Text
    }

    public TargetComponent Target;

    public AudioClip[] AudioClips;
    public Sprite[] Sprites;
    public LocalizedString LocalizedString;

    void Start()
    {
        int currentLanguageIndex = (int) LocalizationManager.GetCurrentLanguage();
        switch (Target)
        {
            case TargetComponent.AudioSource:
                AudioSource AudioSource = GetComponent<AudioSource>();
                AudioSource.clip = AudioClips[currentLanguageIndex];
                break;
            case TargetComponent.Image:
                Image Image = GetComponent<Image>();
                Image.sprite = Sprites[currentLanguageIndex];
                break;
            case TargetComponent.Text:
                RTLTextMeshPro RTLText = GetComponent<RTLTextMeshPro>();
                if (RTLText != null)
                {
                    RTLText.text = LocalizedString.value;
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
                break;
        }
    }
}