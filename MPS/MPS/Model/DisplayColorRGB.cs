using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace MPS.Model
{
    public class DisplayColorRgb
    {     

        public string ColorCode { get; set; }

        public const int IndexRed = 0;
        public const int IndexGreen = 1;
        public const int IndexBlue = 2;

        public DisplayColorRgb()
        {
            ColorCode = ColorsDictionary[Color.Black];
        }

        public static readonly Dictionary<Color, string> ColorsDictionary = new Dictionary<Color, string>()
        {
            { Color.Red, "500"},
            { Color.Orange, "510"},
            { Color.Yellow, "420"},
            { Color.LawnGreen, "350"},
            { Color.SpringGreen, "051"},
            { Color.Cyan, "053"},
            { Color.DodgerBlue, "035"},
            { Color.Blue, "005"},
            { Color.DarkViolet, "304"},
            { Color.Magenta, "502"},
            { Color.Black, "000"},
            { Color.White, "555"},
        };
      

        public Color Color
        {            
            set => ColorCode = ColorsDictionary[value];
        }

    }
}