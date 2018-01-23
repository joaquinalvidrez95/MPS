using Xamarin.Forms;

namespace MPS.Trigger
{
    public class FlatButtonPressedTrigger:TriggerAction<Button>
    {
        protected override void Invoke(Button sender)
        {
            sender.BackgroundColor = (Color) Application.Current.Resources["ColorFlatButtonPressed"];
            sender.Opacity = (double) Application.Current.Resources["OpacityFlatButtonPressed"];
        }
    }
}
