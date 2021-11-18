using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Banyan.Navigation
{
    public interface INavigationService
    {
        Page CurrentPage { get; }

        Task SetMainPage<T>() where T : Page;

        Task SetMainPage<T>(FlyoutNavigationBehaviour navigationBehaviour) where T : FlyoutPage;

        Task SetMainPage(Type pageType);

        Task NavigateToMainPage();

        Task NavigateToPage<T>() where T : Page;

        Task NavigateToPage<T, TData>(TData data) where T : Page;

        Task NavigateBack();

        Task NavigateBackToPage<T>() where T : Page;

        Task<bool> TryNavigateBackToPage<T>() where T : Page;

        void ClearNavigationHistory();

        Task ClearNavigationToPage<T>() where T : Page;

        Task ShowModal<T>() where T : Page;

        Task RemoveModal();
    }

    public enum FlyoutNavigationBehaviour
    {
        HideWhenNavigating,
        StayOpen
    }
}