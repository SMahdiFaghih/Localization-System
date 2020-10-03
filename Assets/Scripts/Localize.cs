using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using RTLTMPro;

[System.Serializable]
public struct Components
{
    public AudioClip[] AudioClips;
    public Image[] Images;
    public LocalizedString localizedString;
}

public class Localize : MonoBehaviour
{
    public TargetComponent Target;
    public Components Components;

    private RTLTextMeshPro RTLText;

    public enum TargetComponent
    {
        AudioSource,
        Image,
        Text
    }

    void Start()
    {
       
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