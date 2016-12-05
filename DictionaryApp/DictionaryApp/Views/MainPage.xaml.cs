using DictionaryApp.Helper;
using DictionaryApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DictionaryApp
{

    public sealed partial class MainPage : Page
    {
        private List<Words> source = new List<Words>();
        private long lastSearchTimeInMilis = 0;
        private void SetFullScreen()
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
        }
        private void loadDictionary()
        {
            //loads all the characters (A,B,C,...) from the database
            List<string> groupSource = Database.getGroupChar("Words");
            groupSource.Sort();
            lstGroup.ItemsSource = groupSource;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadDictionary();
            var currentView = SystemNavigationManager.GetForCurrentView();

            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();

            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            currentView.BackRequested -= backButton_Tapped;
        }
        
        public MainPage()
        {
            this.InitializeComponent();
        }


        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //handles which appbarbuttons are visible for each pivot item page
            if (MainPivot.SelectedIndex == Common.HomeIndex)
            {
                btnMulti.Visibility = Visibility.Collapsed;
                btnRemind.Visibility = Visibility.Collapsed;
            }
            else if (MainPivot.SelectedIndex == Common.SearchIndex)
            {
                btnMulti.Visibility = Visibility.Collapsed;
                btnRemind.Visibility = Visibility.Collapsed;
            }
            else if (MainPivot.SelectedIndex == Common.FavouritesIndex)
            {
                //loads all the favourited word for the Favourites page
                List<Words> lstsource = Database.getAllWords("Favourites");
                lstFavourites.ItemsSource = lstsource;
                if (lstFavourites.Items.Count > 0)
                {
                    btnMulti.Visibility = Visibility.Visible;
                }  
                btnRemind.Visibility = Visibility.Visible;

            }
            else if (MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                //loads all the recently viewed words for the Recents page
                List<Words> lstsource = Database.getAllWords("Recents");
                lstRecents.ItemsSource = lstsource;
                if (lstRecents.Items.Count > 0)
                {
                    btnMulti.Visibility = Visibility.Visible;
                }      
                btnRemind.Visibility = Visibility.Visible;
            }
            else { return; }
        }

        private void lstGroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if a character(A,B,C,...) is selected on the mainpage
            //get all the words from the database that correspond with
            //the selected character
            lstAlphabetic.Visibility = Visibility.Visible;
            lstGroup.Visibility = Visibility.Collapsed;
            string groupChar = e.ClickedItem as string;
            lstAlphabetic.ItemsSource = Database.getAllWordsByChar(groupChar);
        }

        private void lstAlphaBetic_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if you select a word in the list of words it navigates you to the 
            //ViewWord page where the word is displayed
            //The word is then added to the recents table in the database
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Database.insertIntoTable("Recents", word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void lstAlphaBetic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void btnSpeechRecognition_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Create an instance of SpeechRecognizer.
            var speechRecognizer = new SpeechRecognizer();
            // Compile the constraint.
            await speechRecognizer.CompileConstraintsAsync();
            SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();
            //display the recognition result
            txtSearch.Text = speechRecognitionResult.Text;

        }


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch != null)
            {
                if (txtSearch.Text.Length > 0)
                {
                    if (lastSearchTimeInMilis + 500 > DateTime.Now.Millisecond)
                    {
                        lastSearchTimeInMilis = DateTime.Now.Millisecond;
                        source = Database.searchWord(txtSearch.Text.Trim());
                        if (source != null)
                        {
                            if (source.Count == 0)
                            { }
                            else
                            {
                                lstSearch.Visibility = Visibility.Visible;
                                lstSearch.ItemsSource = source;
                            }
                        }
                    }
                }
                else
                {
                    lstSearch.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void lstSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if you select a word in the search list, navigate to ViewWord to display the word
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;

        }

        private void lstFavorites_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if you select a word in the favourites list, navigate to ViewWord to display the word
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;

        }

        private void lstFavorites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if the items selected is greater than 0
            if (lstFavourites.SelectedItems.Count > 0)
            {
                btnDelete.IsEnabled = true;//enable delete button
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Words word = (e.OriginalSource as FrameworkElement).DataContext as Words;
            if (word != null)
            {
                Database.deleteFromTable("Favourites", word);//deletes the selected word from favourites
                lstFavourites.ItemsSource = Database.getAllWords("Favourites");//get all the words from favourites
            }
        }

        private void lstRecents_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if you select a word in the recents list, navigate to ViewWord to display the word
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void lstRecents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if the selected items in the recents list is greater than 0
            if (lstRecents.SelectedItems.Count > 0)
            {
                btnDelete.IsEnabled = true;//enable delete button
            }
            else
            {
                btnDelete.IsEnabled = false;
            }     
        }

        private void RecentsFlyoutDelete_Click(object sender, RoutedEventArgs e)
        {
            Words word = (e.OriginalSource as FrameworkElement).DataContext as Words;
            if (word != null)
            {
                Database.deleteFromTable("Recents", word);//deleted from Recents
                lstRecents.ItemsSource = Database.getAllWords("Recents");//get all the words from recents
            }
        }

        private void btnRemind_Click(object sender, RoutedEventArgs e)
        {
            RegisterBackgroundTask(1, false); // 1 = 1 hour
        }

        private async void RegisterBackgroundTask(uint hour, bool oneShot)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            hour *= 60;
            if (result == BackgroundAccessStatus.Denied)
            {

            }

            var taskRegisted = false;
            var taskName = "ReminderWords";
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    task.Value.Unregister(true); // Unregister task
                }    
            }

            if (taskRegisted == false)
            {
                var builder = new BackgroundTaskBuilder();
                var trigger = new TimeTrigger(hour, oneShot);

                builder.Name = taskName;
                builder.TaskEntryPoint = typeof(BackgroundTask.BackgroundTask).FullName;
                builder.SetTrigger(trigger);

                var taskRegistion = builder.Register();
                taskRegistion.Completed += TaskRegistion_Completed;
            }
        }

        private void TaskRegistion_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {

        }

        private void btnMulti_Click(object sender, RoutedEventArgs e)
        {
            int index = MainPivot.SelectedIndex;
            //if you're on the Favourites pivot item page
            if (index == Common.FavouritesIndex)
            {
                if (lstFavourites.SelectionMode != ListViewSelectionMode.Multiple)
                {
                    //enables select all 
                    lstFavourites.SelectionMode = ListViewSelectionMode.Multiple;
                    lstFavourites.IsItemClickEnabled = false;
                    EnableMultiSelectMode();
                }
                else
                {
                    lstFavourites.SelectionMode = ListViewSelectionMode.Single;
                    lstFavourites.IsItemClickEnabled = true;
                    DisableMultiSelectMode();
                }
            }
            //if you're on the Recents pivot item page
            else if (index == Common.RecentsIndex)
            {
                if (lstRecents.SelectionMode != ListViewSelectionMode.Multiple)
                {
                    lstRecents.SelectionMode = ListViewSelectionMode.Multiple;
                    lstRecents.IsItemClickEnabled = false;
                    EnableMultiSelectMode();
                }
                else
                {
                    lstRecents.SelectionMode = ListViewSelectionMode.Single;
                    lstRecents.IsItemClickEnabled = true;
                    DisableMultiSelectMode();
                }
            }
        }

        private void DisableMultiSelectMode()
        {
            btnMulti.Visibility = Visibility.Visible;
            btnSelectAll.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
        }

        private void EnableMultiSelectMode()
        {
            btnMulti.Visibility = Visibility.Collapsed;
            btnSelectAll.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            btnDelete.IsEnabled = false;
            btnCancel.Visibility = Visibility.Visible;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //if you're on the Favourites pivot item page
            if (MainPivot.SelectedIndex == Common.FavouritesIndex)
            {
                //if the selected items in the list of favourites is greater than 0
                if (lstFavourites.SelectedItems.Count > 0)
                {
                    foreach (Words item in lstFavourites.SelectedItems)
                    {
                        //delete all the selected items from recents
                        Database.deleteFromTable("Favourites", item);
                    }
                    lstFavourites.ItemsSource = Database.getAllWords("Favourites");//then get all the words in favourites table
                }
                else
                {
                    Common.showMessage("Please select item to delete...");
                }
            }
            //if you're on the Recents pivot item page
            else if (MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                //if the selected items in the list of recents is greater than 0
                if (lstRecents.SelectedItems.Count > 0)
                {
                    foreach (Words item in lstRecents.SelectedItems)
                    {
                        //delete all the selected items from recents
                        Database.deleteFromTable("Recents", item);
                    }       
                    lstRecents.ItemsSource = Database.getAllWords("Recents");
                }
                else
                {
                    Common.showMessage("Please select item to delete...");
                }
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            //navigate to the settings pivot item page
            Frame.Navigate(typeof(Settings));
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DisableMultiSelectMode();
            if (MainPivot.SelectedIndex == Common.FavouritesIndex)
            {
                //changes the selection mode to single so you have to select each item individually
                lstFavourites.SelectionMode = ListViewSelectionMode.Single;
            }
            else if (MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                //changes the selection mode to single so you have to select each item individually
                lstRecents.SelectionMode = ListViewSelectionMode.Single;
            }
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            //select all if you're in Favourites 
            if (MainPivot.SelectedIndex == Common.FavouritesIndex)
            {
                lstFavourites.SelectAll();
            }
            //select all if you're in Recents  
            else if (MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                lstRecents.SelectAll();
            }
        }

        private void lstAlphabetic_ItemClick_1(object sender, ItemClickEventArgs e)
        {

        }
    }
}
