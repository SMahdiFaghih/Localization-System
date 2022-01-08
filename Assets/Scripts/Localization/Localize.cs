﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RTLTMPro;

namespace Localization
{
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
            RTLTextMeshPro,
            AudioSource,
            Image,
            GridLayoutGroup,
            HorizontalOrVerticalLayoutGroup,
            Position2D
        }

        public TargetComponent Target;

        [HideInInspector] public LocalizedString LocalizedString;
        [HideInInspector] public AudioClip[] AudioClips;
        [HideInInspector] public Sprite[] Sprites;
        [HideInInspector] public GridLayoutStartCorner StartCorner;
        [HideInInspector] public bool ReverseArrangement = true;
        [HideInInspector] public bool ChangeChildAlignment = false;
        [HideInInspector] public Vector2[] Positions;
        [HideInInspector] public Outline Outline;
        [HideInInspector] public bool FixedFontAsset = false;
        [HideInInspector] public FontAssetDetails FixedFontAssetDetails;
        [HideInInspector] public bool IsContainsAtSign = false;

        private bool _valueSetBefore = false;

        void Start()
        {
            int currentLanguageIndex = (int)LocalizationManager.GetCurrentLanguage();
            ApplyLocalization(currentLanguageIndex, false);
        }

        public void ApplyLocalization(int languageIndex, bool editMode)
        {
            if (_valueSetBefore && !editMode)
            {
                return;
            }

            switch (Target)
            {
                case TargetComponent.RTLTextMeshPro:
                    SetTextValue(languageIndex, editMode);
                    break;
                case TargetComponent.Image:
                    Image Image = GetComponent<Image>();
                    Image.sprite = Sprites[languageIndex];
                    break;
                case TargetComponent.AudioSource:
                    AudioSource AudioSource = GetComponent<AudioSource>();
                    AudioSource.clip = AudioClips[languageIndex];
                    AudioSource.Play();
                    break;
                case TargetComponent.GridLayoutGroup:
                    SetGridLayoutStartCorner(languageIndex);
                    break;
                case TargetComponent.HorizontalOrVerticalLayoutGroup:
                    SetLayoutGroupChildAlignment(languageIndex);
                    break;
                case TargetComponent.Position2D:
                    gameObject.transform.localPosition = Positions[languageIndex];
                    break;
            }

            enabled = false;
            enabled = true;
        }

        #region RTLTextMeshPro
        private void SetTextValue(int languageIndex, bool editMode = false)
        {
            RTLTextMeshPro RTLTextMeshPro = GetComponent<RTLTextMeshPro>();
            if (RTLTextMeshPro == null)
            {
                return;
            }

            LocalizedLanguage currenctLanguage = (LocalizedLanguage)languageIndex;

            if (!string.IsNullOrEmpty(LocalizedString.key))
            {
                string value;
                if (editMode)
                {
                    value = LocalizationManager.GetLocalizedValue(LocalizedString.key, currenctLanguage);
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

            SetOtherSettings(RTLTextMeshPro, currenctLanguage);

            //Re-Fix
            RTLTextMeshPro.UpdateText();
        }

        private void SetOtherSettings(RTLTextMeshPro RTLTextMeshPro, LocalizedLanguage currenctLanguage)
        {
            //Set ForceFix to show number-contained texts properly
            if (currenctLanguage == LocalizedLanguage.Farsi)
            {
                RTLTextMeshPro.forceFix = true;
            }
            else
            {
                RTLTextMeshPro.forceFix = false;
            }

            //Set FontAsset and MaterialPreset to show the proper Outline
            if (FixedFontAsset)
            {
                SetFontAndMaterial((int)Outline, ref RTLTextMeshPro);
            }
            else
            {
                SetFontAndMaterial(currenctLanguage, (int)Outline, ref RTLTextMeshPro);
            }

            //Set Alignment
            if (currenctLanguage == LocalizedLanguage.Farsi && RTLTextMeshPro.alignment.ToString().Contains("Left"))
            {
                int alignmentNumber = (int)RTLTextMeshPro.alignment + 3;
                RTLTextMeshPro.alignment = (TextAlignmentOptions)alignmentNumber;
            }
            else if (currenctLanguage != LocalizedLanguage.Farsi && RTLTextMeshPro.alignment.ToString().Contains("Right"))
            {
                int alignmentNumber = (int)RTLTextMeshPro.alignment - 3;
                RTLTextMeshPro.alignment = (TextAlignmentOptions)alignmentNumber;
            }
        }

        public void SetFontAndMaterial(LocalizedLanguage language, int outlineIndex, ref RTLTextMeshPro RTLTextMeshPro)
        {
            FontAssetDetails fontAssetDetails = LocalizationManager.Instance.GetFontAssetDetailsByLanguage(language);
            RTLTextMeshPro.font = fontAssetDetails.FontAsset;
            RTLTextMeshPro.fontSharedMaterial = fontAssetDetails.MaterialPresets[outlineIndex];
        }

        public void SetFontAndMaterial(int outlineIndex, ref RTLTextMeshPro RTLTextMeshPro)
        {
            RTLTextMeshPro.font = FixedFontAssetDetails.FontAsset;
            RTLTextMeshPro.fontSharedMaterial = FixedFontAssetDetails.MaterialPresets[outlineIndex];
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

            _valueSetBefore = true;
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

        #endregion

        #region GridLayoutGroup
        private void SetGridLayoutStartCorner(int currentLanguageIndex)
        {
            LocalizedLanguage currenctLanguage = (LocalizedLanguage)currentLanguageIndex;
            GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
            if (StartCorner == GridLayoutStartCorner.LeftRight)
            {
                if (currenctLanguage == LocalizedLanguage.Farsi && gridLayoutGroup.startCorner.ToString().Contains("Left"))
                {
                    int alignmentNumber = (int)gridLayoutGroup.startCorner + 1;
                    gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)alignmentNumber;
                }
                else if (currenctLanguage != LocalizedLanguage.Farsi && gridLayoutGroup.startCorner.ToString().Contains("Right"))
                {
                    int alignmentNumber = (int)gridLayoutGroup.startCorner - 1;
                    gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)alignmentNumber;
                }
            }
            else if (StartCorner == GridLayoutStartCorner.UpLow)
            {
                if (currenctLanguage == LocalizedLanguage.Farsi && gridLayoutGroup.startCorner.ToString().Contains("Low"))
                {
                    int alignmentNumber = (int)gridLayoutGroup.startCorner - 2;
                    gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)alignmentNumber;
                }
                else if (currenctLanguage != LocalizedLanguage.Farsi && gridLayoutGroup.startCorner.ToString().Contains("Up"))
                {
                    int alignmentNumber = (int)gridLayoutGroup.startCorner + 2;
                    gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)alignmentNumber;
                }
            }
            gridLayoutGroup.enabled = false;
            gridLayoutGroup.enabled = true;
        }
        #endregion

        #region HorizontalOrVerticalLayoutGroup
        private void SetLayoutGroupChildAlignment(int currentLanguageIndex)
        {
            LocalizedLanguage currenctLanguage = (LocalizedLanguage)currentLanguageIndex;

            HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
            if (currenctLanguage == LocalizedLanguage.Farsi && ReverseArrangement)
            {
                horizontalOrVerticalLayoutGroup.reverseArrangement = true;
            }
            else
            {
                horizontalOrVerticalLayoutGroup.reverseArrangement = false;
            }

            if (ChangeChildAlignment == true)
            {
                if (currenctLanguage == LocalizedLanguage.Farsi && horizontalOrVerticalLayoutGroup.childAlignment.ToString().Contains("Left"))
                {
                    int alignmentNumber = (int)horizontalOrVerticalLayoutGroup.childAlignment + 2;
                    horizontalOrVerticalLayoutGroup.childAlignment = (TextAnchor)alignmentNumber;
                }
                else if (currenctLanguage != LocalizedLanguage.Farsi && horizontalOrVerticalLayoutGroup.childAlignment.ToString().Contains("Right"))
                {
                    int alignmentNumber = (int)horizontalOrVerticalLayoutGroup.childAlignment - 2;
                    horizontalOrVerticalLayoutGroup.childAlignment = (TextAnchor)alignmentNumber;
                }
            }
        }
        #endregion

        void OnValidate()
        {
            int languagesCount = System.Enum.GetNames(typeof(LocalizedLanguage)).Length;
            if (AudioClips == null || AudioClips.Length != languagesCount)
            {
                System.Array.Resize(ref AudioClips, languagesCount);
            }
            if (Sprites == null || Sprites.Length != languagesCount)
            {
                System.Array.Resize(ref Sprites, languagesCount);
            }
            if (Positions == null || Positions.Length != languagesCount)
            {
                System.Array.Resize(ref Positions, languagesCount);
            }

            int outilnesCount = System.Enum.GetNames(typeof(Outline)).Length;
            if (FixedFontAsset && FixedFontAssetDetails != null && (FixedFontAssetDetails.MaterialPresets == null || FixedFontAssetDetails.MaterialPresets.Length != languagesCount))
            {
                System.Array.Resize(ref FixedFontAssetDetails.MaterialPresets, outilnesCount);
            }
        }
    }
}