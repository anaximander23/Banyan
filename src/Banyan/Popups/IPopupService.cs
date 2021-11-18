using System.Threading.Tasks;

namespace Banyan.Popups
{
    public interface IPopupService
    {
        Task ShowBusyIndicator();
        Task ShowBusyIndicator(string message);
        Task DismissBusyIndicator();

        Task Show(Alert alert);

        Task<bool> Show(Dialog dialog);

        Task<string> Show(OptionsDialog dialog);
        Task<T> Show<T>(OptionsDialog<T> dialog);

        Task<string> Show(DangerousOptionsDialog dialog);
        Task<T> Show<T>(DangerousOptionsDialog<T> dialog);
    }
}