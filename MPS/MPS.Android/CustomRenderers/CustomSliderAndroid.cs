﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using MPS.Droid.CustomRenderers;
using MPS.View;

[assembly:ExportRenderer(typeof(CustomSlider), typeof(CustomSliderAndroid))]
namespace MPS.Droid.CustomRenderers
{
    public class CustomSliderAndroid : ViewRenderer<Slider, SeekBar>, SeekBar.IOnSeekBarChangeListener
    {
        double _max, _min;
        bool _progressChangedOnce;
        double Value
        {
            get { return _min + (_max - _min) * (Control.Progress / 1000.0); }
            set { Control.Progress = (int)((value - _min) / (_max - _min) * 1000.0); }
        }


        public CustomSliderAndroid(Context context) : base(context)
        {
            AutoPackage = false;
        }

        void SeekBar.IOnSeekBarChangeListener.OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (!_progressChangedOnce)
            {
                _progressChangedOnce = true;
                return;
            }

            ((IElementController)Element).SetValueFromRenderer(Slider.ValueProperty, Value);
        }

        void SeekBar.IOnSeekBarChangeListener.OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        void SeekBar.IOnSeekBarChangeListener.OnStopTrackingTouch(SeekBar seekBar)
        {
        }
        protected override SeekBar CreateNativeControl()
        {
            return new FormsSeekBar(Context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var seekBar = CreateNativeControl();
                SetNativeControl(seekBar);

                seekBar.Max = 1000;

                seekBar.SetOnSeekBarChangeListener(this);
            }

            Slider slider = e.NewElement;
            _min = slider.Minimum;
            _max = slider.Maximum;
            Value = slider.Value;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Slider view = Element;
            switch (e.PropertyName)
            {
                case "Maximum":
                    _max = view.Maximum;
                    break;
                case "Minimum":
                    _min = view.Minimum;
                    break;
                case "Value":
                    if (Value != view.Value)
                        Value = view.Value;
                    break;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            BuildVersionCodes androidVersion = Build.VERSION.SdkInt;
            if (androidVersion < BuildVersionCodes.JellyBean)
                return;

            // Thumb only supported JellyBean and higher

            if (Control == null)
                return;

            SeekBar seekbar = Control;

            Drawable thumb = seekbar.Thumb;
            int thumbTop = seekbar.Height / 2 - thumb.IntrinsicHeight / 2;

            thumb.SetBounds(thumb.Bounds.Left, thumbTop, thumb.Bounds.Left + thumb.IntrinsicWidth, thumbTop + thumb.IntrinsicHeight);
        }
    }


}
}