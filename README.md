
# Localization-System
Localization System for Unity can be used for both RTL and LTR languages (based on CSV file). You can use your desired outline for the texts and also add strings to the predefined localization values dynamically.

 You can also use this system to apply localization to **Images, AudioSources, LayoutGroups** and **RectTrasnforms**.

---
- [Installation](#installation)
- [Documentation](#documentation)
	- [How to Start](#how-to-start)
	- [Define a new Language](#define-a-new-language)
	- [Create a new FontAsset](#create-a-new-fontasset)
	- [Define a new Outline](#define-a-new-outline)
	- [Localization Targets](#localization-targets)
		- [RTLTextMeshPro](#rtltextmeshpro)
			- [First looks at CSV file](#first-looks-at-csv-file)
			- [Add/Edit a LocalizedText](#addedit-a-localizedtext)
			- [Search/Delete existing LocalizedTexts](#searchdelete-existing-localizedtexts)
			- [Fixed FontAsset](#fixed-fontasset)
			- [Set key and adjust values dynamically](#set-key-and-adjust-values-dynamically)
			- [Multiline texts](#multiline-texts)
			- [Keyless values](#keyless-values)
		- [AudioSource](#audiosource)
		- [Image](#image)
		- [GridLayoutGroup](#gridlayoutgroup)
		- [HorizontalOrVerticalLayoutGroup](#horizontalorverticallayoutgroup)
		- [RectTransform](#recttransform)
- [Maintainer](#Maintainer)
---

# Installation

In order to use this system inside your unity project, you can either download it from the [Releases](https://github.com/SMahdiFaghih/Localization-System/releases) page and import it from Assets > Import Package > Custom Package...
Or clone the project and copy all the assets folder content into your project.

# Documentation

## How to Start

All files of this localization system can be found in **Assets > Scripts > Localization**.

Open Enums.cs script. There are two enums in it which determines the languages and the outlines. You need to add your desired language or outline into these enums in order to use them in the rest of the system.

Create an empty game object and add **LocalizationManager** script to it. In the inspector and for LocalizationManager component, you'll see the empty slots for the enum values that mentioned before, that you can select or drag and drop the FontAsset files and Text Materials (for outline) in there.

Add **Localize** script as a component to the Game object that you want to change one of its other components by localization. You can add multiple Localize components to the same GameObject for different targets. (We'll discuss about all these **Targets** later)

The word Target, means the component that you want to change its properties using Localization and for different languages. For example if you want to change the sprite of an image, your target should be Image.

From the inspector, choose the target component using the drop-down in the Localize component.
The rest of the component's view in editor will change according to that target and will allow you to set your desired settings and files for that target.

For all the targets there are buttons for every language you mentioned in LocalizedLanguage.cs Enum and by clicking on them you can see the preview of applying the localization of that target for that language in the Edit mode.

## Define a new Language

If you want to define a new language, you just have to do the two followings:

* Add it to LocalziationLanguage enum is Enums.cs file.
* follow the Localziation.csv pattern and add it to the first row of it. (For more information read [this](#first-looks-at-csv-file) part of this document)

## Create a new FontAsset

In order to use Localziation for RTLTextMeshPro, you need to use FontAssets and not Fonts. But don't worry, you can easily create a FontAsset using your TTF file.
I used some different settings for the original .ttf file and some of them has some problems for outlines after creating a FontAsset using them. So i suggest you to use the following settings for your .ttf file using the inspector and hit apply.

![Screenshot](Images/FontSettings.png)

Then right click on the .ttf file and create a FontAsset from **Create > TextMeshPro > FontAsset**. For more information about FontAsset and its different properties please checkout [here](https://learn.unity.com/tutorial/textmesh-pro-font-asset-creation).

## Define a new Outline

If you want to define a new outline, you just have to do the two followings:

* Every FontAsset has an **Atlas Material**. copy it from the used fontAssets and set your desired **Thickness** and **Color** for the outline using the inspector. I suggest you to set its **Shader** to **TextMeshPro Fixed/Mobile/Distance Field** as its shown in the image below.
* Give it a name and add it to Outline enum is Enums.cs file. Then a new slot will be added for every language in the Localization Manager that you can select or drag and drop your previously created Material.

* ![Screenshot](Images/Outline.png)

## Localization Targets

### RTLTextMeshPro

if you choose RTLTextMeshPro as the target, localize component will look like the image below. Then you can use your predefined key so that its value for the current language will be set as your text on Start. Don't worry i will explain everything about this key-value thing :)

Remember that this only works if Localize and RTLTextMeshPro components are attached to the same gameObject.

![Screenshot](Images/RTLTextMeshPro.png)

### First looks at CSV file

In order to use a text in different languages, first you have to define it in the Localization.csv file. You can find it in **Assets > Resources > Localization**.

Every row of this file consists of some texts surrounded by double-quotes and separated by comma. The first row of has the headers of every column and the rest of it consists of a key and some texts for every language. Note that the Localziation System uses this key and the current selected language to set the text value of RTLTextMeshPro component.

### Add/Edit a LocalizedText

In order to add a new localized text, you have to first, specify a key and the value of it for every language. Then you need to do just one of the followings:

* Open the Localization.csv file as a plain text, follow its pattern and add/edit your key and values to a new row.
* Open the Localization.csv file as in LibreOffice or similar applications and add/edit your key and values to a new row. (Make sure that the saving methods of that application follows our pattern, otherwise it won't work.)
* Use the ability of this system to add/edit your key and values using the inspector. In the Localize component there is a '+' button. Click on it and you'll see a window like the image below that you can add/edit your key and values there. Note that you should first type in the key and then the values. If the key is repetitive, the value textFields will be filled with its current values. You can also use this window to edit your previously added values (but not the key).

* ![Screenshot](Images/LocalizerWindow.png)

### Search/Delete existing LocalizedTexts

There is a search icon in Localize component that by clicking on it you can search between saved data in Localization.csv and check if a particular key or localized value exists. Using it, you can also delete keys (and their values of course) from file by just clicking on 'x' button next to them.

![Screenshot](Images/Search.png)

### Fixed FontAsset

Localization system sets your selected fontAsset for every RTLTextMeshPro component. But sometimes for a certain text, you just what the text to change and you wan to use a fixed fontAsset for all the languages. In this case you must check the Fixed FontAsset checkbox and assign your desired FontAsset and its outlines according to the image below.

![Screenshot](Images/FixedFontAsset.png)

### Set key and adjust values dynamically

Sometimes thet text that must shown in a certain RTLTextMeshPro, is not fixed and you should set it from the code. For these situations you should leave the Key inputFiled empty (in Localize component) and use the code and **SetKey** function to set that key and the value will be applied to that RTLTextMeshPro immediately.

Sometimes your text has some parts that should be calculated in your code, for these situations you should write **'#'** character for that parts in values of that key in Localization.csv file. then by using the same **SetKey** function and passing string parameters, localization system will replace '#' characters with them in the same order you pass them to this function. There is a example for it in **TextLocalizationByCode** scene and **TextLocalizationSample** script.

### Multiline texts

Sometimes you have a multiline text and you want to use Localization to set it. in the Localziation.csv file all values of a certain key must be in a single line so, to do this, you can use **'@'** character in your value wherever you need the rest of it to be written in the next line. There is a example for it in **TextLocalizationByCode** scene and **TextLocalizationSample** script.

This may cause problem for texts that contain emails or any other situations that you want to use '@' character in your text. So to fix this problem, for these texts you must check the **Contains AtSign(@)** checkbox in the Localize component.

### Keyless values

Sometimes your text doesn't have any key (i.g. its just a number or a date) and you just want the proper fontAsset to be set for it. In these situations just leave the Key inputFiled empty (in Localize component) and set your text using the normal way (RTLTextMeshPro.text = 'your text'). Localize component will set fontAsset and other settings on Start even if the Key inputField is empty.

### AudioSource

if you choose AudioSource as the target, localize component will look like the image below. You can set your desired **AudioClips** for every language and it will be set as the audioSource's audioClip on Start.

Remember that this only works if Localize and AudioSource components are attached to the same gameObject.

![Screenshot](Images/AudioSource.png)

### Image

if you choose Image as the target, localize component will look like the image below. You can set your desired **Sprites** for every language and it will be set as the image's sprite on Start.

Remember that this only works if Localize and Image components are attached to the same gameObject.

![Screenshot](Images/Image.png)

### GridLayoutGroup

if you choose GridLayoutGroup as the target, localize component will look like the image below. For every language there are two drop-downs to set your desired **StartCorner** and **ChildAlignment**. These are the same enums that the original GridLayoutGroup component uses them.

Remember that this only works if Localize and GridLayoutGroup components are attached to the same gameObject.

![Screenshot](Images/GridLayoutGroup.png)

### HorizontalOrVerticalLayoutGroup

if you choose HorizontalOrVerticalLayoutGroup as the target, localize component will look like the image below. This works for both **HorizontalLayoutGroup** and **VerticalLayoutGroup** targets and for every language you have to set two properties. First is **ChildAlignment** and the second one is **ReverseArrangement**. These two are the exact values that those LayoutGroup components has them and you can use them to set the behaviour of the LayoutComponent for every language.

Remember that this only works if Localize and HorizontalLayoutGroup (or VerticalLayoutGroup) components are attached to the same gameObject.

![Screenshot](Images/HorizontalOrVerticalLayoutGroup.png)

### RectTransform

if you choose RectTransform as the target, localize component will look like the image below. For every language there is a button which by pressing on it, Localize script will store the properties (only **AnchoredPosition** and **SizeDelta**) of the RectTrasnform component.

Note that you can only use it to adjust the **Position** of your gameObject. All you need to do is to move your gameObject to the desired position and click on this button.

Remember that this only works if Localize and RectTransform components are attached to the same gameObject.

![Screenshot](Images/RectTransform.png)

----

# Maintainer

- [Seyyed Mahdi Faghih](https://github.com/SMahdiFaghih)
