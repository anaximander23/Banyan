using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Banyan.Navigation
{
    internal class NavigationService : INavigationService
    {
        public NavigationService(IPageFactory pageFactory, Func<INavigationRoot> navigationRootGetter)
        {
            _pageFactory = pageFactory;
            _navigationRootGetter = navigationRootGetter;
        }

        private readonly IPageFactory _pageFactory;
        private readonly Func<INavigationRoot> _navigationRootGetter;
        private FlyoutNavigationBehaviour _masterDetailNavigationBehaviour;

        public Page CurrentPage => GetCurrentPage();

        private INavigationRoot NavigationRoot
        {
            get => GetNavigationRoot() ?? throw new InvalidOperationException("Cannot navigate; main page has not been set.");
        }

        public async Task SetMainPage(Type pageType)
        {
            Page mainPage = await _pageFactory.CreatePage(pageType);
            SetMainPage(mainPage);
        }

        public async Task SetMainPage<T>() where T : Page
        {
            Page mainPage = await _pageFactory.CreatePage<T>();
            SetMainPage(mainPage);
        }

        public async Task SetMainPage<T>(FlyoutNavigationBehaviour navigationBehaviour) where T : FlyoutPage
        {
            Page mainPage = await _pageFactory.CreatePage<T>();
            SetMainPage(mainPage);

            _masterDetailNavigationBehaviour = navigationBehaviour;
        }

        public async Task NavigateToPage<T>() where T : Page
        {
            Page page = await _pageFactory.CreatePage<T>();
            ApplyFlyoutBehaviour();

            await NavigationRoot.Navigation.PushAsync(page, animated: true);
        }

        public async Task NavigateToPage<T, TData>(TData data) where T : Page
        {
            Page page = await _pageFactory.CreatePageWithData<T, TData>(data);

            ApplyFlyoutBehaviour();

            await NavigationRoot.Navigation.PushAsync(page, animated: true);
        }

        public Task NavigateBack()
        {
            ApplyFlyoutBehaviour();

            return NavigationRoot.Navigation.PopAsync(animated: true);
        }

        public async Task NavigateBackToPage<T>() where T : Page
        {
            bool couldNavigateBack = await TryNavigateBackToPage<T>();

            if (!couldNavigateBack)
            {
                throw new InvalidOperationException("The target page type is not in the navigation stack");
            }
        }

        public async Task<bool> TryNavigateBackToPage<T>() where T : Page
        {
            INavigation navController = NavigationRoot.Navigation;
            IReadOnlyList<Page> navStack = navController.NavigationStack;

            if (!navStack.Any(p => p is T))
            {
                return false;
            }

            if (!(CurrentPage is T))
            {
                IEnumerable<Page> pagesToPop = navStack
                    .Reverse()
                    .Skip(1)
                    .TakeWhile(p => !(p is T));

                foreach (Page page in pagesToPop)
                {
                    navController.RemovePage(page);
                }
                await navController.PopAsync(animated: true);
            }

            ApplyFlyoutBehaviour();

            return true;
        }

        public Task NavigateToMainPage()
        {
            ApplyFlyoutBehaviour();

            return NavigationRoot.Navigation.PopToRootAsync(animated: true);
        }

        public async Task ClearNavigationToPage<T>() where T : Page
        {
            Page page = await _pageFactory.CreatePage<T>();
            Page rootPage = NavigationRoot.Navigation.NavigationStack.First();

            await NavigationRoot.Navigation.PushAsync(page);
            NavigationRoot.Navigation.RemovePage(rootPage);

            ApplyFlyoutBehaviour();
        }

        public void ClearNavigationHistory()
        {
            INavigation navController = NavigationRoot.Navigation;
            IReadOnlyList<Page> navStack = navController.NavigationStack;

            IEnumerable<Page> pagesToPop = navStack
                .Reverse()
                .Skip(1);

            foreach (Page page in pagesToPop)
            {
                navController.RemovePage(page);
            }
        }

        public async Task ShowModal<T>() where T : Page
        {
            Page page = await _pageFactory.CreatePage<T>();
            await NavigationRoot.Navigation.PushModalAsync(page);
        }

        public Task RemoveModal()
        {
            return NavigationRoot.Navigation.PopModalAsync();
        }

        private INavigationRoot GetNavigationRoot()
        {
            return _navigationRootGetter?.Invoke();
        }

        private void ApplyFlyoutBehaviour()
        {
            if (_masterDetailNavigationBehaviour == FlyoutNavigationBehaviour.HideWhenNavigating
                            && NavigationRoot.MainPage is FlyoutPage flyoutPage)
            {
                flyoutPage.IsPresented = false;
            }
        }

        private Page GetCurrentPage()
        {
            return NavigationRoot.Navigation.NavigationStack.FirstOrDefault() ?? NavigationRoot.MainPage;
        }

        private void SetMainPage(Page mainPage)
        {
        }
    }
}