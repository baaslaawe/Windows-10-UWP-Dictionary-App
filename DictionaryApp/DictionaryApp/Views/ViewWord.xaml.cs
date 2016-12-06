using DictionaryApp.Helper;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DictionaryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewWord : Page
    {
        private Words word;
        private int isFavourite;

        private void loadWordsDetails(Words word)
        {
            //load all the values for the word selected
            txtWord.Text = word.Word;
            txtType.Text = word.Type;
            txtDescription.Text = word.Description;
        }

        private string convertIntToValue(string type, int value)
        {
            string result = "";
            if (type.Equals("pitch"))
            {
                switch (value)
                {
                    case 1:
                        result = "x-low";
                        break;
                    case 2:
                        result = "low";
                        break;
                    case 3:
                        result = "medium";
                        break;
                    case 4:
                        result = "high";
                        break;
                    case 5:
                        result = "x-high";
                        break;
                    default:
                        result = "default";
                        break;
                }
                
                        
            }
            else if (type.Equals("range"))
            {
                switch (value)
                {
                    case 1:
                        result = "x-low";
                        break;
                    case 2:
                        result = "low";
                        break;
                    case 3:
                        result = "medium";
                        break;
                    case 4:
                        result = "high";
                        break;
                    case 5:
                        result = "x-high";
                        break;
                    default:
                        result = "default";
                        break;
                }
            }
            else if (type.Equals("rate"))
            {
                switch (value)
                {
                    case 1:
                        result = "x-slow";
                        break;
                    case 2:
                        result = "slow";
                        break;
                    case 3:
                        result = "medium";
                        break;
                    case 4:
                        result = "fast";
                        break;
                    case 5:
                        result = "x-fast";
                        break;
                    default:
                        result = "default";
                        break;
                }
            }
            else if (type.Equals("volume"))
            {
                switch (value)
                {
                    case 0:
                        result = "silent";
                        break;
                    case 1:
                        result = "x-soft";
                        break;
                    case 2:
                        result = "soft";
                        break;
                    case 3:
                        result = "medium";
                        break;
                    case 4:
                        result = "loud";
                        break;
                    case 5:
                        result = "x-loud";
                        break;
                    default:
                        result = "default";
                        break;
                }
            }
            else
            {
                result = "default";
            }
            return result;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //handle back button
            SystemNavigationManager.GetForCurrentView().BackRequested += ViewWord_BackRequested;
            word = e.Parameter as Words;
            loadWordsDetails(word);
            isFavourite = Database.searchFavourites(word);

            //check favourites
            if(isFavourite == 1)
            {
                btnFavourites.Icon = new SymbolIcon(Symbol.UnFavorite);
                btnFavourites.Label = "UnFavourite";
            }
            else
            {
                btnFavourites.Icon = new SymbolIcon(Symbol.Favorite);
                btnFavourites.Label = "Favourite";
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= ViewWord_BackRequested;
        }

        private void ViewWord_BackRequested(object sender, BackRequestedEventArgs e)
        {

            e.Handled = true;
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        public ViewWord()
        {
            this.InitializeComponent();
        }

        private async void btnSpeech_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int rate = Convert.ToInt32(Database.getUserData(Common.SpeakRateColumn));
            int range = Convert.ToInt32(Database.getUserData(Common.SpeakRangeColumn));
            int pitch = Convert.ToInt32(Database.getUserData(Common.SpeakPitchColumn));
            int volume = Convert.ToInt32(Database.getUserData(Common.SpeakVolumeColumn));
            int gender = Convert.ToInt32(Database.getUserData(Common.SpeakVoiceGender));

            // The media object for controlling and playing audio.
            MediaElement mediaPlayer = new MediaElement();
            using (var speech = new SpeechSynthesizer())
            {
                //We will use Ssml for the voice format
                //You can google Ssml Document from Microsoft
                var Ssml = String.Format("<speak version='1.0' " +
                    "xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" +
                    "<prosody rate='{0}' volume='{1}' pitch='{2}' range='{3}'>'{4}'</prosody>"
                    +
                    "</speak>", convertIntToValue("rate", rate), convertIntToValue("volume", volume), convertIntToValue("pitch", pitch), convertIntToValue("range", range), txtWord.Text);
                Debug.WriteLine(Ssml);
                if (gender == 1)
                    speech.Voice = SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Male);
                else
                    speech.Voice = SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Female);

                // Generate the audio stream from plain text.
                SpeechSynthesisStream stream = await speech.SynthesizeSsmlToStreamAsync(Ssml);
                // Send the stream to the media object.
                mediaPlayer.SetSource(stream, stream.ContentType);
                mediaPlayer.Play();


            }
        }

        private void btnFavourites_Click(object sender, RoutedEventArgs e)
        {
            //if favourites button is clicked for a word, change the button icon to unfavourites button. 
            if(isFavourite == 0)
            {
                Database.insertIntoTable("Favourites", word);
                btnFavourites.Icon = new SymbolIcon(Symbol.UnFavorite);
                btnFavourites.Label = "UnFavourites";
            }
            //if the favourited word is deleted from the database, change the button icon to favourites
            else
            {

                Database.deleteFromTable("Favourites", word);
                btnFavourites.Icon = new SymbolIcon(Symbol.Favorite);
                btnFavourites.Label = "Favourites";
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //if the settings button is clicked, navigate to the settings page
            Frame.Navigate(typeof(Settings));
        }

    }
}
