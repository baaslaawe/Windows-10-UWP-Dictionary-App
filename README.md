# Windows-10-Pocket-Dictionary-App

**Student Name:** Gareth Lynskey </br>
**Student ID:** G00312651 </br>
**Course:** Software Development </br>
**Current Year:** 4th Year </br>
**Module:** Mobile Application Development </br>
**Lecturer:** Martin Kenirons </br>

## **_Introduction_**
This is a dictionary application build for the Universal Windows Platform. All the information on this application is
stored on an SQLite database. The MainPage of the application "Alphabet", consists of the letters of the alphabet to which 
you can navigate into to find a list of all the words that correspond to the chosen letter. It also contains a button at the bottom
of the page to favourite words that you want to save. The Search page has a search bar you can use to search words in the dictionary 
to avoid scrolling down through the long list of words. The Favourites page contains a list of all the words you have favourited.
The Recents page contains a list of all the words you have recently viewed.

## **_Techical Summary_**

### **XAML**
The XAML pages are used to the design the view of the application. These are the pages that the user interacts with.
The XAML pages in this project are as follows:

XAML Page | Description
------------ | -------------
MainPage.xaml | This is the home page of the application. The MainPage contains four pivot items, Alphabet, Search, Favourites, and Recents which are sliding pages the user can use to navigate through the entire application.
Settings.xaml | The Settings page contains the settings for the application which the user can use to alter the functionality of the application. The Settings consists of two pivot items, General and Speech. The General pivot item page is where the user can change the font size of the three columns(Word, Type, Description) of text being displayed on the application from the database. The Speech pivot item page contains options for changing the speech output.
ViewWord.xaml | The ViewWord pages is where the definitions of the words are shown. This page will display the Word, Type and Description
of a selected word from the database.

### **C#**
The C# pages are where the application is provided functionality. The logic behind all the XAML pages is written using C#. For
every XAML page, there is a corresponding C# class behind it. The C# pages in this project are as follows:

Class | Description
------------ | -------------
App.xaml.cs | This class comes with all visual studio projects. The App.xaml.cs page contains methods which tell the application what to do during the applications life cycle, an example of this would be what the application does when its been launched, suspended or terminated. These methods give the developer flexibility to describe what they want done during critical stages of the application life cycle. This class contains methods that I have created to manage what the application does when it is launched, if the navigation fails and when the application is suspended.
MainPage.xaml.cs | This class contains the main functionality of the MainPage.xaml page. This page controls the navigation to and from pages, what functions are called when certain buttons or pages are clicked, which parts of the application are visable and when to query the database.
Settings.xaml.cs | This class controls the functionality of the Settings.xaml page. It contains all the logic behind the settings options, error handling and page navigation.
ViewWord.xaml.cs | This class controls the functionality of the ViewWord.xaml page. This page contains the logic behind the buttons clicked (example: favourites, speech, delete), loads the full details of the word and page navigation.
Words.cs | Getters and Setters for the Words table in the database.
Common.cs | This class contains variables for the database, columns and pivot item pages.
Database.cs | This class controls all the SELECT, INSERT, DELETE and UPDATE queries to the database.

### **SQLite**
The SQLite database has all the words for the dictionary stored on a local database. This database also stores the users data such as what words were recently viewed and which words were favourited.
The database called Database.db consists of four tables which are as follows:

Table Name | Description
------------ | -------------
Words | Stores all the words in the dictionary. It has four columns, Id, Word, Type and Description.
Favourites | Stores all the words that were favourited by the user.
Recents | Stores all the words recently viewed by the user. 
UserData | Stores all the settings changes made by the user

## **_Project User Guide_**
- Download the zip file provided on the github repository.
- Open the project .sln file using Visual Studio 2015.
- You may need to follow the SQLite User guide described below in order to get the project to run. I downloaded the project on other computers and had to do so.
- Run the project.

## **_SQLite User Guide_**
Installation guide on SQLite for the project on Visual Studio 2015:
- First you need to go to the SQLite download site at [https://www.sqlite.org/download.html](https://www.sqlite.org/download.html)
- Download SQLite for **Universal Windows Platform**
- Open up Visual Studio 2015
- The first thing you must do is add a reference to SQLite in your project
- Right click on the **References** tab in your project in **Solution Explorer** and then click **Add Reference**
- On the pop up window, navigate to the **Universal Windows section** on the left and click **Extensions** 
- Tick the **SQLite for Universal App platform** box, and also the **C++ Runtime 2015** then click **OK**
- In your project in solution explorer once again right click on the **References** folder and select **Manage Nuget Packages**
- Select **Browse** and then type the following into the search area: **SQLite.Net-PCL**
- You should see the package, then click **install**
- Thats it, SQLite is now installed into the project

## **_References_**
1.[https://docs.microsoft.com/en-us/ef/core/get-started/uwp/getting-started](https://docs.microsoft.com/en-us/ef/core/get-started/uwp/getting-started)
2.[http://stackoverflow.com/questions/34850610/query-multiple-tables-sqlite-windows-10-uwp](http://stackoverflow.com/questions/34850610/query-multiple-tables-sqlite-windows-10-uwp)
3.[https://code.msdn.microsoft.com/windowsapps/Implement-SQLite-Local-8b13a307](https://code.msdn.microsoft.com/windowsapps/Implement-SQLite-Local-8b13a307)
4.[https://code.msdn.microsoft.com/windowsapps/Local-Data-Base-SQLite-for-5e6146aa](https://code.msdn.microsoft.com/windowsapps/Local-Data-Base-SQLite-for-5e6146aa)
5.[http://grogansoft.com/blog/?p=1116](http://grogansoft.com/blog/?p=1116)
6.[http://stackoverflow.com/questions/15292880/create-sqlite-database-and-table](http://stackoverflow.com/questions/15292880/create-sqlite-database-and-table)
7.[http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/](http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/)
8.[http://stackoverflow.com/questions/2660723/remove-characters-after-specific-character-in-string-then-remove-substring](http://stackoverflow.com/questions/2660723/remove-characters-after-specific-character-in-string-then-remove-substring)
9.[https://msdn.microsoft.com/en-us/windows/uwp/input-and-devices/speech-recognition](https://msdn.microsoft.com/en-us/windows/uwp/input-and-devices/speech-recognition)
10.[https://msdn.microsoft.com/en-us/library/jj127898.aspx](https://msdn.microsoft.com/en-us/library/jj127898.aspx)
11.[https://blogs.windows.com/buildingapps/2016/05/23/using-speech-in-your-uwp-apps-from-talking-to-conversing/#cwlJ46sIxxmpKfxm.97](https://blogs.windows.com/buildingapps/2016/05/23/using-speech-in-your-uwp-apps-from-talking-to-conversing/#cwlJ46sIxxmpKfxm.97)
12.[https://msdn.microsoft.com/en-us/library/windows.media.speechsynthesis.speechsynthesizer.synthesizessmltostreamasync.aspx](https://msdn.microsoft.com/en-us/library/windows.media.speechsynthesis.speechsynthesizer.synthesizessmltostreamasync.aspx)
13.[https://www.w3.org/TR/speech-synthesis/](https://www.w3.org/TR/speech-synthesis/)
14.[http://www.c-sharpcorner.com/uploadfile/2b876a/local-data-base-sqlite-for-windows-10/](http://www.c-sharpcorner.com/uploadfile/2b876a/local-data-base-sqlite-for-windows-10/)
15.[http://stackoverflow.com/questions/35463566/uwp-text-to-speech-from-listbox-c](http://stackoverflow.com/questions/35463566/uwp-text-to-speech-from-listbox-c)
16.[http://devcenter.wintellect.com/jprosise/handling-the-back-button-in-windows-10-uwp-apps](http://devcenter.wintellect.com/jprosise/handling-the-back-button-in-windows-10-uwp-apps)
17.[http://www.mso.anu.edu.au/~ralph/OPTED/](http://www.mso.anu.edu.au/~ralph/OPTED/) This is where I got all the words for the dictionary. This website has changed since I first started the project and now the words are unavailable. </br>
18.[https://www.w3.org/Voice/2007/speech-synthesis11/WD-speech-synthesis11-20070611diff.html](https://www.w3.org/Voice/2007/speech-synthesis11/WD-speech-synthesis11-20070611diff.html)
19.[https://msdn.microsoft.com/en-us/library/b0zbh7b6(v=vs.110).aspx](https://msdn.microsoft.com/en-us/library/b0zbh7b6(v=vs.110).aspx)



