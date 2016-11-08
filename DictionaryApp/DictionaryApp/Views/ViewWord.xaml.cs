using DictionaryApp.Helper;
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

        private void btnSpeech_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private void btnFavourites_Click(object sender, RoutedEventArgs e)
        {
            if(isFavourite == 0)
            {
                Database.insertIntoTable("Favourites", word);
                btnFavourites.Icon = new SymbolIcon(Symbol.UnFavorite);
                btnFavourites.Label = "UnFavourites";
            }
            else
            {
                Database.deleteFromTable("Favourites", word);
                btnFavourites.Icon = new SymbolIcon(Symbol.Favorite);
                btnFavourites.Label = "Favourites";
            }
        }

        /*
        private void btnTranslate_Click(object sender, RoutedEventArgs e)
        {

        }*/

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

    }
}
