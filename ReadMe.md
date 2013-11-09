# WPF Localizator #
**Application for localizing WPF applications (as the name sugest).** This application finds strings in files and moves them to the resource dictionaries. Instead strings are used links to records in dictionaries. To work with these dictionaries is possible to use any procedure, I can recommend [WpfLocalizeExtension](http://wpflocalizeextension.codeplex.com/ "The Localization Extension") project, which is used in this application.
The current version can process only XAML files, but will soon be able to process C # files (code-behind, etc.) too.

WPF Localizator is intended for developers who are working on multilingual applications and do not want waste time creating records in resource dictionaries for each button, title etc. and creating links to these records.


**WPF Localizator can save you a lot of time and unnecessary work.**

![WPF Localizator - Main Window](http://s21.postimg.org/gftwaq3rr/Main_Window.png)

## Download ##
[Release](https://github.com/mveith/Veith.WPFLocalizator/releases "Release")

## Instructions ##
### A) Processing folders ###
This is convenient when a programmer doesn't want to deal localization during development. For example, we have finished the application (but only in one language) and want to convert to multi-lingual.

1. Start WPF Localizator
2. Select folder
2. Configure links and select resource dictionaries
3. Start processing
4. Localize each file - editing keys

### B) File processing in Visual Studio ###
This is convenient when a programmer wants to keep application localized at each time. So throughout the development application supports all planned languages.

1. Start WPF Localizator and configure (configure links and select resource dictionaries)
2. Add the application as External Tool in Visual Studio and set argument to item path ("$ (ItemPath)"). I also recommend to set a keyboard shortcut.
3. Open the file in Visual Studio (or select it in the Solution Explorer) and run the application as an external tool
4. Localize file - editing keys

![WPF Localizator - Keys editing](http://s14.postimg.org/w7rohm90h/Keys_Editing.png)

## Thirdparties ##
I have used these projects (and I can recommend itto everyone):

- [MahApps.Metro](http://mahapps.com/MahApps.Metro/ "MahApps.Metro Documentation") - Metro design
- [NSubstitute](https://github.com/nsubstitute/NSubstitute "NSubstitute") - Mocking framework
- [Castle Windsor](http://docs.castleproject.org/Default.aspx?Page=MainPage&NS=Windsor&AspxAutoDetectCookieSupport=1 "Castle Windsor") - Inversion of Control container
- [WpfLocalizeExtension](http://wpflocalizeextension.codeplex.com/ "WPF Localization Extension") - Localization extension for multilingual applications

## Licence ##

Published under the Creative Commons Attribution 3.0 Unported license - [http://creativecommons.org/licenses/by/3.0/deed](http://creativecommons.org/licenses/by/3.0/deed "Creative Commons Attribution 3.0 Unported license")

Copyright © Miroslav Veith 2013