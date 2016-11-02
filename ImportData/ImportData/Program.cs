using ImportData.Class;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportData
{
    class Program
    {
        private static SQLiteConnection connection;
        /// <sumary>
        /// Function createDataBase
        /// </sumary>
        /// <param name="dbName">Name of DB</param>

        private static void CreateDataBase(string dbName)
        {
            SQLiteConnection.CreateFile(dbName);
            connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbName));
            connection.Open();
        }

        /// <sumary>
        /// Function create Table
        /// </sumary>
        /// <param name="dbName">Name of DB</param>
        private static void createTable(string TableName)
        {
            string sqlCMD = string.Format("CREATE TABLE IF NOT EXISTS \"{0}\" ('Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,'Word' TEXT,'Type' TEXT,'Description' BLOB)", TableName);
            SQLiteCommand cmd = new SQLiteCommand(sqlCMD, connection);
            cmd.ExecuteNonQuery();
        }

        /// <sumary>
        /// Insert data for table
        /// </sumary>
        /// <param name="TableName">name of table</param>
        private static void insertTable(string TableName)
        {
            List<Words> dictionarySource = getDictionaryList(@"C:\Users\lynsk\Documents\Visual Studio 2015\Projects\ImportData\ImportData\StaticData\WordsSource.txt");
            if(dictionarySource.Count > 0)
            {
                foreach (var item in dictionarySource)
                {
                    string sql = string.Format("Insert into Words (Id,Word,Type,Description) values ({0},'{1}','{2}','{3}')", item.id, item.Word, item.Type, item.Description);
                    SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static List<Words> getDictionaryList(string filePath)
        {
            //C:\Users\lynsk\Documents\Visual Studio 2015\Projects\ImportData\ImportData\StaticData\WordsSource.txt"
            List<Words> dictionarySource = new List<Words>();
            var content = File.ReadAllLines(filePath);
   
            content = content.Where(x=>!String.IsNullOrEmpty(x)).ToArray();//removes all the empty rows in WordsSource.txt
            int id = 1;//sets the first id
            string word, type, description;
            foreach(var value in content)
            {
                //Create a function to save an array of the index of characters
                List<int> index = getAllIndexOf(' ',value);
                int indexOfLeftParenthesis = value.IndexOf('(');//Gets the index of the first '(' character
                if(index.Count > 1)
                {
                    //We get the word before or inside ()
                    //Get all the string after () and replace with empty
                    word = value.Replace(value.Substring(index[0],value.Length-index[0]), string.Empty);
                    //Removes the word so we only have the type and description
                    type = value.Remove(0, indexOfLeftParenthesis - 1);
                    int indexOfRightParenthesis = type.IndexOf(')');//get the index of ')'
                    description = type.Remove(0, indexOfRightParenthesis + 1);//removes the type () so we only have the description

                    type = type.Replace(description, string.Empty);//removes the description

                    Words wordValue = new Words();
                    wordValue.id = id;
                    wordValue.Word = word.Replace("'","''");//SQLite query makes an error with the character ' so we have to change it to ''
                    wordValue.Type = type.Replace("'", "''");
                    wordValue.Description = description.Replace("'", "''");
                    dictionarySource.Add(wordValue);
                    id++;
                }
            }
            return dictionarySource;
        }

        private static List<int> getAllIndexOf(char character, string value)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < value.Length; i++)
            {
                if(value[i] == character)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Auto generate SQLite databases v1.01");

            CreateDataBase("Database.db");
            createTable("Words");
            Console.WriteLine("Created DB and table successfully");
            insertTable("Words");
            Console.WriteLine("Inserted data into table successfully");

            Console.ReadLine();
        }
    }
}
