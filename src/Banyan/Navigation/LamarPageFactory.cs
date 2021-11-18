using System;
using System.Reflection;
using System.Threading.Tasks;
using Banyan.Lifecycle;
using Lamar;
using Microsoft.Maui.Controls;

namespace Banyan.Navigation
{
    internal class LamarPageFactory : IPageFactory
    {
        public LamarPageFactory(IContainer container)
        {
            _container = container;
        }

        private readonly IContainer _container;

        public async Task<Page> CreatePage<T>() where T : Page
        {
            T page = _container.GetInstance<T>();

            await BindFlyoutPage(page);
            await BindPageModel(page);

            return page;
        }

        public async Task<Page> CreatePage(Type pageType)
        {
            if (!typeof(Page).GetTypeInfo().IsAssignableFrom(pageType.GetTypeInfo()))
            {
                throw new ArgumentException("The requested type is not a Page.");
            }

            Page page = _container.GetInstance(pageType) as Page;

            await BindFlyoutPage(page);
            await BindPageModel(page);

            return page;
        }

        public async Task<Page> CreatePageWithData<T, TData>(TData data) where T : Page
        {
            T page = _container.GetInstance<T>();

            await BindFlyoutPage(page);
            await BindPageModel(page, data);

            return page;
        }

        private async Task BindFlyoutPage<T>(T page) where T : Page
        {
            if (page is FlyoutPage flyoutPage)
            {
                Type flyoutType = flyoutPage.Flyout.GetType();
                Page replacementFlyout = await CreatePage(flyoutType);
                flyoutPage.Flyout = replacementFlyout;
            }
        }

        private async Task BindPageModel<T>(T page) where T : Page
        {
            Type modelType = typeof(PageModel<>).MakeGenericType(page.GetType());

            PageModel pageModel = _container.TryGetInstance(modelType) as PageModel;

            if (pageModel is null)
            {
                return;
            }

            if (pageModel is IInitialisable initialisablePageModel)
            {
                await initialisablePageModel.Initialise();
            }

            if (pageModel is IOnAppearing onAppearingPageModel)
            {
                page.Appearing += (obj, args) => onAppearingPageModel.OnAppearing();
            }

            page.BindingContext = pageModel;
        }

        private async Task BindPageModel<T, TData>(T page, TData data) where T : Page
        {
            Type modelType = typeof(PageModel<>).MakeGenericType(page.GetType());

            PageModel pageModel = _container.TryGetInstance(modelType) as PageModel;

            if (pageModel is null)
            {
                return;
            }

            if (pageModel is IInitialisable<TData> initialisablePageModel)
            {
                await initialisablePageModel.Initialise(data);
            }

            page.BindingContext = pageModel;
        }
    }
}