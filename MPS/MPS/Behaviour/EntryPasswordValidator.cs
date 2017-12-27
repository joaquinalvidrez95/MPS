using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.Behaviour
{
    public class EntryPasswordValidator : Behavior<Entry>
    {
        public int MaxLength { get; set; }
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (IsTextValid(e.NewTextValue)) return;
            entry.TextChanged -= OnEntryTextChanged;
            entry.Text = e.OldTextValue;
            entry.TextChanged += OnEntryTextChanged;
        }

        private bool IsTextValid(string e)
        {
            return (Regex.IsMatch(e, @"^[0-9]+$") || e == "")
                && e.Length <= MaxLength;
        }
    }
}
