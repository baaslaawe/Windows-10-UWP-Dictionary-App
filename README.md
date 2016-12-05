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

### **SQLite**
The SQLite database has all the words for the dictionary stored on a local database. This database also stores the users data such as what words were recently viewed and which words were favourited.
The database called Database.db consists of four tables which are as follows:

Table Name | Description
------------ | -------------
Words | Stores all the words in the dictionary. It has four columns, Id, Word, Type and Description.
Favourites | Stores all the words that were favourited by the user.
Recents | Stores all the words recently viewed by the user. 
UserData | Stores all the settings changes made by the user.




