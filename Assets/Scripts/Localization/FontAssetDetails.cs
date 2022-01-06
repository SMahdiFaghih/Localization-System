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
    [System.Serializable]
    public class FontAssetDetails
    {
        public TMP_FontAsset FontAsset;
        public Material[] MaterialPresets;
    }
}
