using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLocalizationSample : MonoBehaviour
{
    public Localize SetKeyTutorial_localize;
    [Space]
    public Localize SetKeyTutorialNewLine_localize;
    [Space]
    public Localize SetKeyAndReplaceStringsTutorial_localize;
    public string[] ReplaceStrings;

    // Start is called before the first frame update
    void Start()
    {
        SetKeyTutorial_localize.SetKey("sample_set_key_tutorial");
        SetKeyTutorialNewLine_localize.SetKey("sample_set_key_new_line_tutorial");
        SetKeyAndReplaceStringsTutorial_localize.SetKey("sample_set_key_replace_strings_tutorial", ReplaceStrings[0], ReplaceStrings[1]);
    }

}