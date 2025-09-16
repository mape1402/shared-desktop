using PropertyChanged;
using SharedDesktop.Client.Services.RealTime;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedDesktop.Client.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        private readonly ISignalRService _signalRService;

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
            await _signalRService.SendClipboardAsync(content);
        }

        private async Task OnClipboardReceived(string content)
        {
            History.Insert(0, content);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Clipboard.SetTextAsync(content);
            });
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
