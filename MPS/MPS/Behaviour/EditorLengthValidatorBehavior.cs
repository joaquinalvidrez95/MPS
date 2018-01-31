using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.Behaviour
{
    public class EditorLengthValidatorBehavior : Behavior<Editor>
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
            var editor = (Editor)sender;

            if (IsTextValid(e.NewTextValue)) return;
            editor.TextChanged -= OnEntryTextChanged;
            editor.Text = e.OldTextValue;
            editor.TextChanged += OnEntryTextChanged;
        }

        private bool IsTextValid(string e)
        {           
            return e.Length <= MaxLength 
                && !e.Contains("\n")
                && (Regex.IsMatch(e, @"^[ -Za-zñÑ]+$") || e == "");                
        }
    }
}
