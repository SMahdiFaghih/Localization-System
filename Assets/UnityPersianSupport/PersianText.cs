using UnityEngine.UI;
using System.Collections;
using UnityPersianSupport;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/PersianText", 11)]
    [ExecuteInEditMode]
    public class PersianText : Text
    {
        public static bool      _enablePersianFix = true;

        [SerializeField]
        [TextArea(3, 10)]
        public string           _rawText;
        [SerializeField]
        private bool            _usernameFiled = true;

        private RectTransform   _rectTransform;

        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        // - MonoBehaviour Events ( Sorted by call hierarchy )

        protected override void OnEnable()
        {
            SetText(text);
            base.OnEnable();
        } 

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetText(text);
            base.OnValidate();
        }
 
        protected override void OnRectTransformDimensionsChange()
        {
            SetText(text);
            base.OnRectTransformDimensionsChange();
        }
#endif

        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        // - Custom Methods

        private int count;

        public void Clear()
        {
            _rawText     = "";
            this.text   = "";
        }

        public void SetText(string text = "")
        {
            if (!_enablePersianFix && !_usernameFiled)
            {
                this.text = text;
                return;
            }

            count = 1000;
            string output = "";

            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            if (string.IsNullOrEmpty(_rawText))
            {
                this.text = "";
                return;
            }

            float width = _rectTransform.rect.width;

            if (width <= 10) return;

            output = PersianFixer.FixText(_rawText);
            TextGenerationSettings setting = GetGenerationSettings(new Vector2(width, _rectTransform.rect.height));
            cachedTextGeneratorForLayout.Populate(output, setting);

            UICharInfo[] info = cachedTextGeneratorForLayout.GetCharactersArray();

            for (int i = 0; i < info.Length; i++)
            {
                if (float.IsNaN(info[i].charWidth) || info[i].charWidth < 0 || info[i].charWidth > Screen.width)
                {
                    this.text = "Woopsi ... :D";
                    return;
                }
            }

            List<string> strings = new List<string>();
            float tempLength = 0;  

            for (int charCounter = info.Length - 1; charCounter >= 0; charCounter--)
            {
                count--; 
                if (count <= 5)
                {
                    if (count <= 0)
                    {
                        this.text = "Woopsi ... :D";
                        break;
                    }
                }

                if (tempLength < width)
                {
                    tempLength += info[charCounter].charWidth / pixelsPerUnit;

                    if (charCounter == 0 && tempLength >= width)
                        charCounter = -2;
                    else
                        continue;
                }

                tempLength = 0;
                charCounter += 2;

                int spaceIndex = charCounter;
                for (; charCounter < info.Length; charCounter++)
                {
                    if (charCounter < output.Length)
                    {
                        if (output[charCounter] == ' ')
                        {
                            spaceIndex = charCounter;
                            break;
                        }
                    } 
                }

                charCounter = spaceIndex;
                int endIndex = output.Length - spaceIndex;
                strings.Add(output.Substring(spaceIndex, endIndex));
                output = output.Remove(spaceIndex, endIndex);
            }

            strings.Add(output);
            string result = "";

            for (int i = 0; i < strings.Count;)
            {
                result += strings[i++];
                if (i < strings.Count)
                    result += '\u000A';
            }

            this.text = result;
        }

        //-------------------------------------------------------------------
    }
}