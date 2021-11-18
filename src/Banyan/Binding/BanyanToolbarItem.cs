using System;
using Microsoft.Maui.Controls;

namespace Banyan.Binding
{
    public class BanyanToolbarItem : ToolbarItem
    {
        public BanyanToolbarItem()
        {
            Clicked += ExecuteClickedCommand;
        }

        public BanyanToolbarItem(string name, string icon, Action activated, ToolbarItemOrder order = ToolbarItemOrder.Default, int priority = 0)
            : base(name, icon, activated, order, priority)
        {
        }

        public static readonly BindableProperty ClickedCommandProperty = BindableProperty.Create(nameof(ClickedCommand), typeof(Command), typeof(EventToCommandBehavior));

        public Command ClickedCommand
        {
            get => (Command)GetValue(ClickedCommandProperty);
            set => SetValue(ClickedCommandProperty, value);
        }

        private void ExecuteClickedCommand(object sender, EventArgs e)
        {
            Command command = ClickedCommand;

            if (command?.CanExecute(e) == true)
            {
                command.Execute(e);
            }
            else if (command?.CanExecute(null) == true)
            {
                command.Execute(null);
            }
        }
    }
}