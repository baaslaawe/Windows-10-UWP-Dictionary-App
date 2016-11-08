using DictionaryApp.Helper;
using DictionaryApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DictionaryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Words> source = new List<Words>();

        private long lastSearchTimeInMillis = 0;

        private void SetFullScreen()
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
        }

        private void loadDictionary()
        {
            List<string> groupSource = Database.getGroupChar("Words");
            groupSource.Sort();
            lstGroup.ItemsSource = groupSource;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            base.OnNavigatedTo(e);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += backButton_Tapped;
            //SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            //SetFullScreen();
            loadDictionary();
            MainPivot.SelectedIndex = Common.MainPivotSaveIndex;
        }

        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            //Debug.Write("Back button tapped");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested -= backButton_Tapped;
        }
        /*
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
        }*/
        /*
        private async void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if (lstAlphabetic.Visibility == Visibility.Visible)
            {
                lstAlphabetic.Visibility = Visibility.Collapsed;
                lstGroup.Visibility = Visibility.Visible;
            }
            else
            {
                var msg = new MessageDialog("Are you sure you want to Exit?");
                var okBtn = new UICommand("Yes");
                var cancelBtn = new UICommand("No");
                msg.Commands.Add(okBtn);
                msg.Commands.Add(cancelBtn);
                IUICommand result = await msg.ShowAsync();
                if (result != null && result.Label == "Yes")
                {
                    Application.Current.Exit();
                }
            }
               
        }*/

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(MainPivot.SelectedIndex == Common.HomeIndex)
            {
                btnMulti.Visibility = Visibility.Collapsed;
                btnRemind.Visibility = Visibility.Collapsed;
            }
            else if(MainPivot.SelectedIndex == Common.SearchIndex)
            {
                btnMulti.Visibility = Visibility.Collapsed;
                btnRemind.Visibility = Visibility.Collapsed;
            }
            else if (MainPivot.SelectedIndex == Common.FavouritesIndex)
            {
                List<Words> lstSource = Database.getAllWords("Favourites");
                lstFavourites.ItemsSource = lstSource;
                if(lstFavourites.Items.Count > 0)
                {
                    btnMulti.Visibility = Visibility.Visible;
                }
                btnRemind.Visibility = Visibility.Visible;
            }
            else if (MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                List<Words> lstSource = Database.getAllWords("Recents");
                lstRecents.ItemsSource = lstSource;
                if (lstRecents.Items.Count > 0)
                {
                    btnMulti.Visibility = Visibility.Visible;
                }
                btnRemind.Visibility = Visibility.Visible;
            }
            else
            {
                return;
            }
        }

        private void lstGroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            lstAlphabetic.Visibility = Visibility.Visible;
            lstGroup.Visibility = Visibility.Collapsed;
            string groupChar = e.ClickedItem as string;
            lstAlphabetic.ItemsSource = Database.getAllWordsByChar(groupChar);
        }

        private void lstAlphabetic_ItemClick(object sender, ItemClickEventArgs e)
        {
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Database.insertIntoTable("Recents", word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }
        
        private void lstAlphabetic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void btnSpeechRecognition_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var speechRecogniser = new SpeechRecognizer();
            await speechRecogniser.CompileConstraintsAsync();
            SpeechRecognitionResult speechRecognitionResult = await speechRecogniser.RecognizeWithUIAsync();
            txtSearch.Text = speechRecognitionResult.Text;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtSearch != null)
            {
                if(txtSearch.Text.Length > 0)
                {
                    if(lastSearchTimeInMillis + 500 > DateTime.Now.Millisecond)
                    {
                        lastSearchTimeInMillis = DateTime.Now.Millisecond;
                        source = Database.searchWord(txtSearch.Text.Trim());
                        if(source != null)
                        {
                            if(source.Count == 0)
                            {

                            }
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
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void lstFavourites_ItemClick(object sender, ItemClickEventArgs e)
        {
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void lstFavourites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstFavourites.SelectedItems.Count > 0)
            {
                btnDelete.IsEnabled = true;
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
            if(word != null)
            {
                Database.deleteFromTable("Favourites", word);
                lstFavourites.ItemsSource = Database.getAllWords("Favourites");
            }
        }

        private void lstRecents_ItemClick(object sender, ItemClickEventArgs e)
        {
            Words word = e.ClickedItem as Words;
            Frame.Navigate(typeof(ViewWord), word);
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void lstRecents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstRecents.SelectedItems.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
        }

        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void RecentsFlyoutDelete_Click(object sender, RoutedEventArgs e)
        {
            Words word = (e.OriginalSource as FrameworkElement).DataContext as Words;
            if (word != null)
            {
                Database.deleteFromTable("Recents", word);
                lstRecents.ItemsSource = Database.getAllWords("Recents");
            }
        }

        private void btnRemind_Click(object sender, RoutedEventArgs e)
        {
            RegisterBackgroundTask(1, false);// 1 = 1hour
        }

        private async void RegisterBackgroundTask(uint hour, bool oneShot)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            hour *= 60;
            if (result == BackgroundAccessStatus.Denied)
            {

            }
            var taskRegistered = false;
            var taskName = "ReminderWords";
            foreach(var task in BackgroundTaskRegistration.AllTasks)
            {
                if(task.Value.Name == taskName)
                {
                    task.Value.Unregister(true);//Unregisters the task
                }
            }
            if(taskRegistered == false)
            {
                var builder = new BackgroundTaskBuilder();
                var trigger = new TimeTrigger( hour, oneShot);
                builder.Name = taskName;
                builder.TaskEntryPoint = typeof(BackgroundTask.BackgroundTask).FullName;
                builder.SetTrigger(trigger);
                var taskRegistration = builder.Register();
                taskRegistration.Completed += TaskRegistration_Completed;
            }
        }

        private void TaskRegistration_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {

        }

        private void btnMulti_Click(object sender, RoutedEventArgs e)
        {
            int index = MainPivot.SelectedIndex;
            if(index == Common.FavouritesIndex)
            {
                if(lstFavourites.SelectionMode != ListViewSelectionMode.Multiple)
                {
                    lstFavourites.SelectionMode = ListViewSelectionMode.Multiple;
                    lstFavourites.IsItemClickEnabled = false;
                    EnableSelectMode();
                }
                else
                {
                    lstFavourites.SelectionMode = ListViewSelectionMode.Single;
                    lstFavourites.IsItemClickEnabled = true;
                    DisableMultiSelectMode();
                }
            }
            else if (index == Common.RecentsIndex)
            {
                if(lstRecents.SelectionMode != ListViewSelectionMode.Multiple)
                {
                    lstRecents.SelectionMode = ListViewSelectionMode.Multiple;
                    lstRecents.IsItemClickEnabled = false;
                    EnableSelectMode();
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

        private void EnableSelectMode()
        {
            btnMulti.Visibility = Visibility.Collapsed;
            btnSelectAll.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            btnDelete.IsEnabled = false;
            btnCancel.Visibility = Visibility.Visible;
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if(MainPivot.SelectedIndex == Common.FavouritesIndex)
            {
                lstFavourites.SelectAll();
            }
            else if(MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                lstRecents.SelectAll();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MainPivot.SelectedIndex == Common.FavouritesIndex)
            {
                if(lstFavourites.SelectedItems.Count > 0)
                {
                    foreach(Words item in lstFavourites.SelectedItems)
                    {
                        Database.deleteFromTable("Favourites", item);
                    }
                    lstFavourites.ItemsSource = Database.getAllWords("Favourites");
                }
                else
                {
                    Common.showMessage("Please select an item to delete");
                }
            }
            else if(MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                if (lstRecents.SelectedItems.Count > 0)
                {
                    foreach(Words item in lstRecents.SelectedItems)
                    {
                        Database.deleteFromTable("Recents", item);
                    }
                    lstRecents.ItemsSource = Database.getAllWords("Recents");
                }
                else
                {
                    Common.showMessage("Please select an item to delete");
                }
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
            Common.MainPivotSaveIndex = MainPivot.SelectedIndex;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DisableMultiSelectMode();
            if(MainPivot.SelectedIndex == Common.FavouritesIndex)
            {
                lstFavourites.SelectionMode = ListViewSelectionMode.Single;
            }
            else if (MainPivot.SelectedIndex == Common.RecentsIndex)
            {
                lstRecents.SelectionMode = ListViewSelectionMode.Single;
            }
        }
    }
}
