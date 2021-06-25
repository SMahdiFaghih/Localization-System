
# Localization-System
Localization System for Unity can be used for both RTL and LTR languages, images, audios, fonts and etc, based on CSV file.
You can use your desired outline for the texts and also add strings to the predefined localization texts dynamically.

---
- [Installation](#installation)
- [Documentation](#documentation)
	- [How to Start](#how-to-start)
- [Maintainer](#Maintainer)
---

# Installation

In order to use this system inside your unity project, you can either download it from the [Releases](https://github.com/SMahdiFaghih/Localization-System/releases) page and import it from Assets > Import Package > Custom Package...
Or clone the project and copy all its content into your project.

# Documentation

## How to Start

After adding all the files, create an empty game object and add LocalizationManager script to it and open this script. There are two enums in this script which determines the languages and the outlines. You need to add your desired language or outline into these enums in order to use them in the rest of the system.

In the inspector and for LocalizationManager script, you'll see the empty slots for the enum values that mentioned before, that you can select or drag and drop the FontAsset files and Text Materials (for outline) in there.

Add Localize script as a component to the Game object that you want to change one of its other components by localization. You can add multiple Localize components for different targets.

Then, from the inspector, choose the target component from the drop down menu in the Localize component.
The rest of the component view in editor will change according to the target and will allow you to set your desired settings and files for that target.

----

# Maintainer

- [Seyyed Mahdi Faghih](https://github.com/SMahdiFaghih)
