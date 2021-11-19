using Banyan;
using Microsoft.Maui.Controls;

namespace BanyanDemo.Pages
{
    public class MainPageModel : PageModel<MainPage>
    {
        public MainPageModel()
        {
            _counter = 0;
            Label = "You haven't clicked yet!";

            ButtonPressed = new Command(OnButtonPressed);
        }

        private string _label;
        private int _counter;

        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public Command ButtonPressed { get; private set; }

        private void OnButtonPressed(object obj)
        {
            _counter++;

            Label = $"Current count: {_counter}";
        }
    }
}