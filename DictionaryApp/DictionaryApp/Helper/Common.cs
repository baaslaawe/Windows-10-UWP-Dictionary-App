using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DictionaryApp.Helper
{
    public class Common
    {
        //Db name
        public static string DB_NAME = "Database.db";
        //Column names
        public static string SpeakRateColumn = "SpeakRate";
        public static string SpeakPitchColumn = "SpeakPitch";
        public static string SpeakRangeColumn = "SpeakRange";
        public static string SpeakVolumeColumn = "SpeakVolume";
        public static string SpeakRateGender = "VoiceGender";
        public static string WordFontSize = "WordFontSize";
        public static string TypeFontSize = "TypeFontSize";
        public static string DescriptionFontSize = "DescriptionFontSize";
        public static string LanguageTo = "LanguageTo";

        //pivot index
        public static int HomeIndex = 0;
        public static int SearchIndex = 1;
        public static int FavouritesIndex = 2;
        public static int RecentsIndex = 3;
        public static int MainPivotSaveIndex = 0;

        //Function to show message dialog
        public static async void showMessage(string errorMessage)
        {
            var msg = new MessageDialog(errorMessage);
            var okBtn = new UICommand("OK");
            msg.Commands.Add(okBtn);
            try
            {
                IUICommand result = await msg.ShowAsync();
            }
            catch
            {
                return;
            }

        }

    }
}
