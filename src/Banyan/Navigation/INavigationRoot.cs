using Microsoft.Maui.Controls;

namespace Banyan.Navigation
{
    internal interface INavigationRoot
    {
        INavigation Navigation { get; set; }
        Page MainPage { get; set; }
    }
}