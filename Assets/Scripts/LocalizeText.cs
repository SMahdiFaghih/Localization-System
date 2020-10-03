using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

[RequireComponent(typeof(RTLTextMeshPro))]
public class LocalizeText : MonoBehaviour
{
    public LocalizedString localizedString;

    private RTLTextMeshPro RTLText;

    void Start()
    {
        RTLText = GetComponent<RTLTextMeshPro>();
        RTLText.text = localizedString.value;
        /*else
        {
            PersianText persianText = GetComponent<PersianText>();
            persianText._rawText = LocalizationManager.GetLocalizedValue(key);
            persianText.enabled = false;
            persianText.enabled = true;
        }*/
    }
}
