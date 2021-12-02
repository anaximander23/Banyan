using System.Threading.Tasks;
using Banyan;
using Banyan.Navigation;
using Microsoft.Maui.Controls;

namespace BanyanDemo.Pages
{
    public class MainPageModel : PageModel<MainPage>
    {
        public MainPageModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _counter = 0;
            Label = "You haven't clicked yet!";

            ButtonPressed = new Command(async () => await OnButtonPressed());
        }

        private readonly INavigationService _navigationService;
        private string _label;
        private int _counter;

        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public Command ButtonPressed { get; private set; }

        private async Task OnButtonPressed()
        {
            await _navigationService.NavigateToPage<ItemListPage>();
        }
    }
}