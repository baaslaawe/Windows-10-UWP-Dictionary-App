using DictionaryApp.Helper;
using System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DictionaryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += Settings_BackRequested;
            loadSettingsData();//loads the settings from the database
        }

        private void loadSettingsData()
        {
            //loads the last saved state of the settings from UserData
            int pitch = Convert.ToInt32(Database.getUserData(Common.SpeakPitchColumn));
            int range = Convert.ToInt32(Database.getUserData(Common.SpeakRangeColumn));
            int rate = Convert.ToInt32(Database.getUserData(Common.SpeakRateColumn)); 
            int volume = Convert.ToInt32(Database.getUserData(Common.SpeakVolumeColumn));
            int male = Convert.ToInt32(Database.getUserData(Common.SpeakVoiceGender));
            int wordFontSize = Convert.ToInt32(Database.getUserData(Common.WordFontSize));
            int typeFontSize = Convert.ToInt32(Database.getUserData(Common.TypeFontSize));
            int desFontSize = Convert.ToInt32(Database.getUserData(Common.DescriptionFontSize));

            pitchSlider.Value = pitch;
            rangeSlider.Value = range;
            rateSlider.Value = rate;
            volumeSlider.Value = volume;
            wordSlider.Value = wordFontSize;
            typeSlider.Value = typeFontSize;
            descriptionSlider.Value = desFontSize;

            if(male == 1)
            {
                Male.IsChecked = true;
            }
            else
            {
                Female.IsChecked = true;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= Settings_BackRequested;
        }

        private async void Settings_BackRequested(object sender, BackRequestedEventArgs e)
        {
            //when back button is clicked a message dialog pops up
            e.Handled = true;
            var msg = new MessageDialog("Do you want to save these changes?");
            var okBtn = new UICommand("Accept");
            var cancelBtn = new UICommand("Decline");
            msg.Commands.Add(okBtn);
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();
            if (result != null && result.Label == "Accept")
            {
                //Saves the changed settings to UserData
                Database.setUserData(Common.SpeakPitchColumn,Convert.ToInt32(pitchSlider.Value));
                Database.setUserData(Common.SpeakRangeColumn, Convert.ToInt32(rangeSlider.Value));
                Database.setUserData(Common.SpeakRateColumn, Convert.ToInt32(rateSlider.Value));
                Database.setUserData(Common.SpeakVolumeColumn, Convert.ToInt32(volumeSlider.Value));
                Database.setUserData(Common.WordFontSize, Convert.ToInt32(wordSlider.Value));
                Database.setUserData(Common.TypeFontSize, Convert.ToInt32(typeSlider.Value));
                Database.setUserData(Common.DescriptionFontSize, Convert.ToInt32(descriptionSlider.Value));
            }
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        public Settings()
        {
            this.InitializeComponent();
        }

        private void wordSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //sets the size of the Words text to the value of the corresponding slider
            txtWord.FontSize = (double)(wordSlider.Value);
        }

        private void typeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //sets the size of the Type text to the value of the corresponding slider
            txtType.FontSize = (double)(typeSlider.Value);
        }

        private void descriptionSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //sets the size of the Description text to the value of the corresponding slider
            txtDescription.FontSize = (double)(descriptionSlider.Value);
        }

        private void Male_Checked(object sender, RoutedEventArgs e)
        {
            //Updates UserDate if the male voice is selected
            Database.setUserData(Common.SpeakVoiceGender, 1);
        }

        private void Female_Checked(object sender, RoutedEventArgs e)
        {
            //Updates UserDate if the female voice is selected
            Database.setUserData(Common.SpeakVoiceGender, 0);
        }
    }
}
