using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.Behaviour
{
    public class EditorLengthValidatorBehavior:Behavior<Editor>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Editor bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;            
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Editor)sender;

            if (entry.Text.Length <= MaxLength) return;
            entry.TextChanged -= OnEntryTextChanged;
            entry.Text = e.OldTextValue;
            entry.TextChanged += OnEntryTextChanged;
        }
    }
}
