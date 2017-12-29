using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.Behaviour
{
    public class DiscreteSliderBehaviour : Behavior<Slider>
    {
        protected override void OnAttachedTo(Slider bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ValueChanged += OnValueChanged;
        }



        protected override void OnDetachingFrom(Slider bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var slider = (Slider)sender;

            slider.ValueChanged -= OnValueChanged;
            slider.Value = Math.Round(e.NewValue);
            slider.ValueChanged += OnValueChanged;

        }
    }
}
