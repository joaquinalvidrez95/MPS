using Xamarin.Forms;

namespace MPS.Trigger
{
    public class FlatButtonReleasedTrigger:TriggerAction<Button>
    {
        protected override void Invoke(Button sender)
        {
            sender.BackgroundColor = Color.Transparent;
            sender.Opacity = 1;
        }
    }
}
