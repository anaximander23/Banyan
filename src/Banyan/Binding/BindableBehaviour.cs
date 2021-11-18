using System;
using Microsoft.Maui.Controls;

namespace Banyan.Binding
{
    // taken from https://xamarinforms.wordpress.com/2015/07/07/adding-a-bindable-map-with-the-map-behavior/
    // via https://anthonysimmon.com/eventtocommand-in-xamarin-forms-apps/
    // 2019-06-26
    // Adapted to Maui 2021-11-18
    public class BindableBehavior<T> : Behavior<T> where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T visualElement)
        {
            base.OnAttachedTo(visualElement);

            AssociatedObject = visualElement;

            if (visualElement.BindingContext != null)
                BindingContext = visualElement.BindingContext;

            visualElement.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T view)
        {
            view.BindingContextChanged -= OnBindingContextChanged;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
    }
}