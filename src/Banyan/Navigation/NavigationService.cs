using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Banyan.Navigation
{
    internal class NavigationService : INavigationService
    {
        public NavigationService(IPageFactory pageFactory, INavigationRoot navigationRoot)
        {
            _pageFactory = pageFactory;
            _navigationRoot = navigationRoot;
        }

        private readonly IPageFactory _pageFactory;
        private INavigationRoot _navigationRoot;
        private FlyoutNavigationBehaviour _masterDetailNavigationBehaviour;
        public Page CurrentPage => GetCurrentPage();

        private INavigationRoot NavigationRoot
        {
            get => _navigationRoot ?? throw new InvalidOperationException("Cannot navigate; main page has not been set.");
            set => _navigationRoot = value;
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

            if (_masterDetailNavigationBehaviour == FlyoutNavigationBehaviour.HideWhenNavigating
                && NavigationRoot.MainPage is FlyoutPage flyoutPage)
            {
                flyoutPage.IsPresented = false;
            }

            await NavigationRoot.Navigation.PushAsync(page, animated: true);
        }

        public async Task NavigateToPage<T, TData>(TData data) where T : Page
        {
            Page page = await _pageFactory.CreatePageWithData<T, TData>(data);

            if (_masterDetailNavigationBehaviour == FlyoutNavigationBehaviour.HideWhenNavigating
                && NavigationRoot.MainPage is FlyoutPage flyoutPage)
            {
                flyoutPage.IsPresented = false;
            }

            await NavigationRoot.Navigation.PushAsync(page, animated: true);
        }

        public Task NavigateBack()
        {
            if (_masterDetailNavigationBehaviour == FlyoutNavigationBehaviour.HideWhenNavigating
                && NavigationRoot.MainPage is FlyoutPage flyoutPage)
            {
                flyoutPage.IsPresented = false;
            }

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

            if (_masterDetailNavigationBehaviour == FlyoutNavigationBehaviour.HideWhenNavigating
                && NavigationRoot.MainPage is FlyoutPage flyoutPage)
            {
                flyoutPage.IsPresented = false;
            }

            return true;
        }

        public Task NavigateToMainPage()
        {
            if (_masterDetailNavigationBehaviour == FlyoutNavigationBehaviour.HideWhenNavigating
                && NavigationRoot.MainPage is FlyoutPage flyoutPage)
            {
                flyoutPage.IsPresented = false;
            }

            return NavigationRoot.Navigation.PopToRootAsync(animated: true);
        }

        public async Task ClearNavigationToPage<T>() where T : Page
        {
            Page page = await _pageFactory.CreatePage<T>();
            Page rootPage = NavigationRoot.Navigation.NavigationStack.First();

            await NavigationRoot.Navigation.PushAsync(page);
            NavigationRoot.Navigation.RemovePage(rootPage);

            if (_masterDetailNavigationBehaviour == FlyoutNavigationBehaviour.HideWhenNavigating
                && NavigationRoot.MainPage is FlyoutPage flyoutPage)
            {
                flyoutPage.IsPresented = false;
            }
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

        private Page GetCurrentPage()
        {
            return _navigationRoot.Navigation.NavigationStack.FirstOrDefault() ?? _navigationRoot.MainPage;
        }

        private void SetMainPage(Page mainPage)
        {
            switch (mainPage)
            {
                case NavigationPage navigationPage:
                    _navigationRoot.MainPage = navigationPage;
                    _navigationRoot.Navigation = navigationPage.Navigation;

                    return;

                case FlyoutPage flyoutPage:
                    if (flyoutPage.Detail is NavigationPage)
                    {
                        _navigationRoot.Navigation = flyoutPage.Detail.Navigation;
                    }
                    else
                    {
                        NavigationPage detailNavPage = flyoutPage.Detail is null ? new NavigationPage() : new NavigationPage(flyoutPage.Detail);
                        flyoutPage.Detail = detailNavPage;
                        _navigationRoot.Navigation = detailNavPage.Navigation;
                    }

                    _navigationRoot.MainPage = flyoutPage;

                    return;

                default:
                    Page rootNavPage = new NavigationPage(mainPage);

                    _navigationRoot.MainPage = mainPage;
                    _navigationRoot.Navigation = rootNavPage.Navigation;

                    return;
            }
        }
    }
}