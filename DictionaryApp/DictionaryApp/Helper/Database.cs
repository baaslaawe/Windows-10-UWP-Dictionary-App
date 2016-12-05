using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryApp.Helper
{
    //Handles all the queries to the database
    public class Database
    {
        public static List<Words> getAllWords(string tableName)
        {
            //adds all the words in the database to the list result
            List<Words> result = new List<Words>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {                
                    var statement = connection.Prepare("SELECT * FROM " + tableName);
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {                  
                        if (statement[0] != null)
                        {
                            //create a word object, add the words values(id,word,type,description) and add it to the list
                            Words word = new Words();
                            word.Id = Convert.ToInt32(statement["Id"]);
                            word.Word = statement["Word"].ToString();
                            word.Type = statement["Type"].ToString();
                            word.Description = statement["Description"].ToString();
                            result.Add(word);
                        }
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static List<string> getGroupChar(string tableName)
        {
            //gets all the characters in the database and adds them to the list result
            //e.g: A,B,C,...
            List<string> result = new List<string>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT DISTINCT UPPER(SUBSTR(Word,1,1)) AS GroupChar FROM " + tableName);
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            result.Add(statement["GroupChar"].ToString());
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static int deleteFromTable(string TableName, Words word)
        {
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var query = string.Format("DELETE FROM {0} WHERE ID='{1}'", TableName, word.Id);
                    var statement = connection.Prepare(query);
                    statement.Step();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public static int insertIntoTable(string tableName, Words word)
        {
            //insert words into the recents and favourites table in the database
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var query = String.Format("INSERT INTO {0} VALUES({1},'{2}','{3}','{4}')", tableName, word.Id, word.Word, word.Type, word.Description);
                    var statement = connection.Prepare(query);
                    statement.Step();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
        }
        
        public static int searchFavourites(Words word)
        {
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM Favourites WHERE Id=" + word.Id);
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                            return 1; // found
                    }
                    return 0; // not found
                }
            }
            catch
            {
                return -1; // error
            }
        }

        public static List<Words> searchWord(string character)
        {
            //searches for words in the database
            //creates and adds them to the list lstWords
            List<Words> lstWords = new List<Words>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM Words WHERE Word LIKE '" + character + "%'");
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            Words word = new Words();
                            word.Id = Convert.ToInt32(statement["Id"]);
                            word.Word = statement["Word"].ToString();
                            word.Type = statement["Type"].ToString();
                            word.Description = statement["Description"].ToString();
                            lstWords.Add(word);
                        }
                    }
                    return lstWords;
                }
            }
            catch
            {
                return null;
            }
        }

        public static object getUserData(string column)
        {
            //retrieves the users favourites and recents
            object result = null;
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare(String.Format("SELECT {0} FROM UserData", column));
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            result = statement[column];
                        }

                    }
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static void setUserData(string column, object value)
        {
            //Updates users favourites and recents
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var query = String.Format("UPDATE UserData Set {0}='{1}'", column, value);
                    var statement = connection.Prepare(query);
                    statement.Step();
                }
            }
            catch
            {
                return;
            }
        }

        public static List<Words> getAllWordsByChar(string groupChar)
        {
            //gets all the words for a certain character and adds them to the list result
            List<Words> result = new List<Words>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM Words where Word Like  '" + groupChar + "%'");
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            Words word = new Words();
                            word.Id = Convert.ToInt32(statement["Id"]);
                            word.Word = statement["Word"].ToString();
                            word.Type = statement["Type"].ToString();
                            word.Description = statement["Description"].ToString();
                            result.Add(word);
                        }
                    }
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}