using SharedDesktop.Client.ViewModels;

namespace SharedDesktop.Client
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel ViewModel => BindingContext as MainViewModel;

        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel?.OnDisappearing();
        }

        private async void OnHistoryItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (sender is CollectionView collectionView && e.CurrentSelection.FirstOrDefault() is string selectedText)
            {
                // Copy selected text to clipboard
                await Clipboard.SetTextAsync(selectedText);
                
                // Show confirmation
                //await DisplayAlert("Copied", "Text has been copied to clipboard!", "OK");
                
                // Clear selection to allow reselecting the same item
                collectionView.SelectedItem = null;
            }
        }

        private async void OnCopyToClipboard(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string textToCopy)
            {
                await Clipboard.SetTextAsync(textToCopy);
                
                // Show brief toast-like message
                //await DisplayAlert("Copied", "Text copied to clipboard!", "OK");
            }
        }
    }
}
