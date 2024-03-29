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
        public enum TargetComponent
        {
            RTLTextMeshPro,
            AudioSource,
            Image,
            GridLayoutGroup,
            HorizontalOrVerticalLayoutGroup,
            RectTransform
        }

        public TargetComponent Target;

        [HideInInspector] public LocalizedString LocalizedString;
        [HideInInspector] public AudioClip[] AudioClips;
        [HideInInspector] public Sprite[] Sprites;
        [HideInInspector] public GridLayoutGroupProperties[] GridLayoutGroupProperties;
        [HideInInspector] public HorizontalOrVerticalLayoutGroupProperties[] HorizontalOrVerticalLayoutGroupProperties;
        [HideInInspector] public RectTransformProperties[] RectTransformProperties;
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
                    Image image = GetComponent<Image>();
                    image.sprite = Sprites[languageIndex];
                    break;
                case TargetComponent.AudioSource:
                    AudioSource audioSource = GetComponent<AudioSource>();
                    audioSource.clip = AudioClips[languageIndex];
                    audioSource.Play();
                    break;
                case TargetComponent.GridLayoutGroup:
                    SetGridLayoutGroupProperties(languageIndex);
                    break;
                case TargetComponent.HorizontalOrVerticalLayoutGroup:
                    SetHorizontalOrVerticalLayoutGroupProperties(languageIndex);
                    break;
                case TargetComponent.RectTransform:
                    RectTransform rectTransform = GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = RectTransformProperties[languageIndex].AnchoredPosition;
                    rectTransform.sizeDelta = RectTransformProperties[languageIndex].SizeDelta;
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
        private void SetGridLayoutGroupProperties(int currentLanguageIndex)
        {
            GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
            gridLayoutGroup.startCorner = GridLayoutGroupProperties[currentLanguageIndex].StartCorner;
            gridLayoutGroup.childAlignment = GridLayoutGroupProperties[currentLanguageIndex].ChildAlignment;
        }
        #endregion

        #region HorizontalOrVerticalLayoutGroup
        private void SetHorizontalOrVerticalLayoutGroupProperties(int currentLanguageIndex)
        {
            HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
            horizontalOrVerticalLayoutGroup.childAlignment = HorizontalOrVerticalLayoutGroupProperties[currentLanguageIndex].ChildAlignment;
            horizontalOrVerticalLayoutGroup.reverseArrangement = HorizontalOrVerticalLayoutGroupProperties[currentLanguageIndex].ReverseArrangment;   
        }
        #endregion

        #region RectTransform
        public void SetRectTransformProperties(int languageIndex)
        {
            RectTransform currentRectTransform = GetComponent<RectTransform>();
            RectTransformProperties[languageIndex].AnchoredPosition = currentRectTransform.anchoredPosition;  
            RectTransformProperties[languageIndex].SizeDelta = currentRectTransform.sizeDelta;  
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
            if (RectTransformProperties == null || RectTransformProperties.Length != languagesCount)
            {
                System.Array.Resize(ref RectTransformProperties, languagesCount);
            }
            if (GridLayoutGroupProperties == null || GridLayoutGroupProperties.Length != languagesCount)
            {
                System.Array.Resize(ref GridLayoutGroupProperties, languagesCount);
            }
            if (HorizontalOrVerticalLayoutGroupProperties == null || HorizontalOrVerticalLayoutGroupProperties.Length != languagesCount)
            {
                System.Array.Resize(ref HorizontalOrVerticalLayoutGroupProperties, languagesCount);
            }

            int outilnesCount = System.Enum.GetNames(typeof(Outline)).Length;
            if (FixedFontAsset && FixedFontAssetDetails != null && (FixedFontAssetDetails.MaterialPresets == null || FixedFontAssetDetails.MaterialPresets.Length != languagesCount))
            {
                System.Array.Resize(ref FixedFontAssetDetails.MaterialPresets, outilnesCount);
            }
        }
    }
}