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
The pages XAML in this project are as follows:

XAML Page | Description
------------ | -------------
MainPage.xaml | This is the home page of the application. The MainPage contains four pivot items, Alphabet, Search, Favourites, and Recents which are sliding pages the user can use to navigate through the entire application.
Settings.xaml | The Settings page contains the settings for the application which the user can use to alter the functionality of the application. The Settings consists of two pivot items, General and Speech. The General pivot item page is where the user can change the font size of the three columns(Word, Type, Description) of text being displayed on the application from the database. The Speech pivot item page contains options for changing the speech output.
ViewWord.xaml | The ViewWord pages is where the definitions of the words are shown. This page will display the Word, Type and Description
of a selected word from the database.



