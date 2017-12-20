using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MPS.Behaviour
{
    public class ItemTappedAttached
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                propertyName: "Command",
                returnType: typeof(ICommand),
                declaringType: typeof(ListView),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                validateValue: null,
                propertyChanged: OnItemTappedChanged
                );

        private static void OnItemTappedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ListView control)
            {
                control.ItemTapped += OnItemTapped;
            }            
        }

        private static void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var control = sender as ListView;
            var command = GetItemTapped(control);
            if (command != null && command.CanExecute(e.Item))
            {
                command.Execute(e.Item);
            }
        }

        public static ICommand GetItemTapped(BindableObject bindableObject)
        {
            return (ICommand)bindableObject.GetValue(CommandProperty);
        }

        public static void SetItemTapped(BindableObject bindableObject, ICommand command)
        {
            bindableObject.SetValue(CommandProperty, command);
        }
    }
}
