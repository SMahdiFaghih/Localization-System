using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.ComponentModel;
using TMPro;
using RTLTMPro;

namespace Localization
{
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
}
