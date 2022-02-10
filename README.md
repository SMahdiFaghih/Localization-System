
# Localization-System
Localization System for Unity can be used for both RTL and LTR languages, images, audios, fonts and etc, based on CSV file.
You can use your desired outline for the texts and also add strings to the predefined localization texts dynamically.

---
- [Installation](#installation)
- [Documentation](#documentation)
	- [How to Start](#how-to-start)
	- [Localization Targets](#localization-targets)
		- [RTLTextMeshPro](#rtltextmeshpro)
		- [AudioSource](#audiosource)
		- [Image](#image)
		- [GridLayoutGroup](#gridlayoutgroup)
		- [HorizontalOrVerticalLayoutGroup](#horizontalorverticallayoutgroup)
- [Maintainer](#Maintainer)
---

# Installation

In order to use this system inside your unity project, you can either download it from the [Releases](https://github.com/SMahdiFaghih/Localization-System/releases) page and import it from Assets > Import Package > Custom Package...
Or clone the project and copy all the assets folder content into your project.

# Documentation

## How to Start

All files of this localization system can be found in Assets > Scripts > Localziation.

Open Enums.cs script. There are two enums in it which determines the languages and the outlines. You need to add your desired language or outline into these enums in order to use them in the rest of the system.

Create an empty game object and add LocalizationManager script to it. In the inspector and for LocalizationManager component, you'll see the empty slots for the enum values that mentioned before, that you can select or drag and drop the FontAsset files and Text Materials (for outline) in there.

Add Localize script as a component to the Game object that you want to change one of its other components by localization. You can add multiple Localize components to the same GameObject for different targets. (We'll discuss about all these TARGETs later)

The word Target, means the component that you want to change its properties using Localization and for different languages. For example if you want to change the sprite of an image, your target should be Image.

From the inspector, choose the target component using the drop-down in the Localize component.
The rest of the component's view in editor will change according to that target and will allow you to set your desired settings and files for that target.
For all the targets there are buttons for every language you mentioned in LocalizedLanguage.cs Enum and by clicking on them you can see the preview of applying the localization of that target for that language in the Edit mode.

## Localization Targets

### RTLTextMeshPro

### AudioSource

if you choose AudioSource as the target, localize component will look like the image below. You can set your desired AudioClips for every language and it will be set as the audioSource's audioClip on Start.

Remember that this only works if Localize and AudioSource components are attached to the same gameObject.

![Screenshot](Images/AudioSource.png)

### Image

if you choose Image as the target, localize component will look like the image below. You can set your desired Sprites for every language and it will be set as the image's sprite on Start.

Remember that this only works if Localize and Image components are attached to the same gameObject.

![Screenshot](Images/Image.png)

### GridLayoutGroup

if you choose GridLayoutGroup as the target, localize component will look like the image below. For every language there are two drop-downs to set your desired StartCorner and ChildAlignment. These are the same enums that the original GridLayoutGroup component uses them.

Remember that this only works if Localize and GridLayoutGroup components are attached to the same gameObject.

![Screenshot](Images/GridLayoutGroup.png)

### HorizontalOrVerticalLayoutGroup

if you choose HorizontalOrVerticalLayoutGroup as the target, localize component will look like the image below. This works for both HorizontalLayoutGroup and VerticalLayoutGroup targets and for every language you have to set two properties. First is ChildAlignment and the second one is ReverseArrangement. These two are the exact values that those LayoutGroup components has them and you can use them to set the behaviour of the LayoutComponent for every language.

Remember that this only works if Localize and HorizontalLayoutGroup (or VerticalLayoutGroup) components are attached to the same gameObject.

![Screenshot](Images/HorizontalOrVerticalLayoutGroup.png)
----

# Maintainer

- [Seyyed Mahdi Faghih](https://github.com/SMahdiFaghih)
