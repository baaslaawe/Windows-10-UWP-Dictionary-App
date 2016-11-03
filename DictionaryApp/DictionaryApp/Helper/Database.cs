using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryApp.Helper
{
    public class Database
    {
        public static List<Words> getAllWords(string tableName)
        {
            List<Words> result = new List<Words>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM" +tableName);
                    while(!(SQLiteResult.DONE == statement.Step()))
                    {
                        if(statement[0] != null)
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

        public static List<string> getGroupChar(string tableName)
        {
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
            catch
            {
                return null;
            }
        }

        public static int deleteFromTable(string TableName, Words word)
        {
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + Common.DB_NAME))
                {
                    var query = string.Format("DELETE FROM {0} WHERE ID='{1}'",TableName, word.Id);
                    var statement = connection.Prepare(query);
                    statement.Step();
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

    }
}
