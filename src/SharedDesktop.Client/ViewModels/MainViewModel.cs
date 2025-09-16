using PropertyChanged;
using SharedDesktop.Client.Services.RealTime;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SharedDesktop.Client.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        private readonly ISignalRService _signalRService;
        private const int MaxHistoryItems = 50; // Limit history to 50 items

        public ObservableCollection<string> History { get; set; } = new();

        public MainViewModel(ISignalRService signalRService)
        {
            _signalRService = signalRService ?? throw new ArgumentNullException(nameof(signalRService)); 
        }

        public ICommand SendClipboardCommand => new Command(async () => await SendClipboardAsync());

        private async Task SendClipboardAsync()
        {
            if (!Clipboard.HasText)
                return;

            var content = await Clipboard.GetTextAsync();
            AddToHistory(content);
            await _signalRService.SendClipboardAsync(content);
        }

        private async Task OnClipboardReceived(string content)
        {
            AddToHistory(content);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Clipboard.SetTextAsync(content);
            });
        }

        private void AddToHistory(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            // Remove existing item if present
            if (History.Contains(content))
            {
                History.Remove(content);
            }

            // Add to top of history
            History.Insert(0, content);

            // Limit history size
            while (History.Count > MaxHistoryItems)
            {
                History.RemoveAt(History.Count - 1);
            }
        }

        public async Task OnAppearing()
        {
            _signalRService.SubscribeOnClipboardReceived(nameof(MainViewModel), OnClipboardReceived);
            await _signalRService.StartAsync();
        }

        public void OnDisappearing()
            => _signalRService.UnsubscribeOnClipboardReceived(nameof(MainViewModel));
    }
}
