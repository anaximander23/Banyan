using System.Linq;
using System.Threading.Tasks;
using Banyan.Navigation;
using Banyan.UIElements;
using Microsoft.Maui.Controls;

namespace Banyan.Popups
{
    internal class PopupService : IPopupService
    {
        public PopupService(INavigationRoot navigationRoot, IBusyIndicator busyIndicator)
        {
            _navigationRoot = navigationRoot;
            _busyIndicator = busyIndicator;
        }

        private readonly INavigationRoot _navigationRoot;
        private readonly IBusyIndicator _busyIndicator;

        public async Task ShowBusyIndicator()
        {
            await ShowBusyIndicator(null);
        }

        public async Task ShowBusyIndicator(string message)
        {
            _busyIndicator.Show(message);
            await Task.FromResult<object>(null);
        }

        public async Task DismissBusyIndicator()
        {
            _busyIndicator.Dismiss();
            await Task.FromResult<object>(null);
        }

        public async Task Show(Alert alert)
        {
            await GetCurrentPage().DisplayAlert(alert.Title, alert.Message, alert.Cancel);
        }

        public async Task<bool> Show(Dialog dialog)
        {
            return await GetCurrentPage().DisplayAlert(dialog.Title, dialog.Message, dialog.Confirm, dialog.Cancel);
        }

        public async Task<string> Show(OptionsDialog dialog)
        {
            return await GetCurrentPage().DisplayActionSheet(dialog.Title, dialog.Cancel, null, dialog.Options.ToArray());
        }

        public async Task<T> Show<T>(OptionsDialog<T> dialog)
        {
            string selectedLabel = await GetCurrentPage().DisplayActionSheet(dialog.Title, dialog.Cancel.Key, null, dialog.Options.Select(o => o.Key).ToArray());
            return dialog.GetSelectedOption(selectedLabel);
        }

        public async Task<string> Show(DangerousOptionsDialog dialog)
        {
            return await GetCurrentPage().DisplayActionSheet(dialog.Title, dialog.Cancel, dialog.DangerousOption, dialog.Options.ToArray());
        }

        public async Task<T> Show<T>(DangerousOptionsDialog<T> dialog)
        {
            string selectedLabel = await GetCurrentPage().DisplayActionSheet(dialog.Title, dialog.Cancel.Key, dialog.DangerousOption.Key, dialog.Options.Select(o => o.Key).ToArray());
            return dialog.GetSelectedOption(selectedLabel);
        }

        private Page GetCurrentPage()
        {
            return _navigationRoot.Navigation.NavigationStack.Last();
        }
    }
}