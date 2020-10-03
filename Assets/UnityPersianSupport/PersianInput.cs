using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityPersianSupport;

public class PersianInput : InputField
{
    public PersianText persianText;

    protected override void Start()
    {
        base.Start();

        #if UNITY_5_2 || UNITY_5_1 || UNITY_5_0
            onValueChange.AddListener((string text) =>
            {
                persianText.SetText(text);
            });
        #else
            onValueChanged.AddListener((string text) =>
            {
                persianText.SetText(text);
            });
        #endif
    }
}
