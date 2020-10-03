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

    private RTLTextMeshPro RTLText;

    void Start()
    {
        //AudioClips = new AudioClip[System.Enum.GetNames(typeof(LocalizationManager.LocalizedLanguage)).Length];
        //Images = new Image[System.Enum.GetNames(typeof(LocalizationManager.LocalizedLanguage)).Length];
        //RTLText = GetComponent<RTLTextMeshPro>();
        //RTLText.text = localizedString.value;
        /*else
        {
            PersianText persianText = GetComponent<PersianText>();
            persianText._rawText = LocalizationManager.GetLocalizedValue(key);
            persianText.enabled = false;
            persianText.enabled = true;
        }*/
    }
}