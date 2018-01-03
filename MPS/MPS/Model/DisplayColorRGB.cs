using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace MPS.Model
{
    public class DisplayColorRgb
    {

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        private const int IndexRed = 0;
        private const int IndexGreen = 1;
        private const int IndexBlue = 2;

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

        public string GetColorCode => Red.ToString() + Green + Blue;

        public Color Color
        {
            set
            {
                //Red = ColorsDictionary[value] / 100;
                //Green = (ColorsDictionary[value] / 10) % 10;
                //Blue = ColorsDictionary[value] % 10;
                Red = ColorsDictionary[value][IndexRed] - 48;
                Green = ColorsDictionary[value][IndexGreen] - 48;
                Blue = ColorsDictionary[value][IndexBlue] - 48;
            }

        }

        //private void SetColorByCode(int red, int green, int blue)
        //{
        //    Red = red;
        //    Green = green;
        //    Blue = blue;
        //}

        //private void SetColorByCode(Color color)
        //{
        //    Red = _colorsDictionary[color] / 100;
        //    Green = (_colorsDictionary[color] / 10) % 10;
        //    Blue = _colorsDictionary[color] % 10;
        //}
        //public string ColorName
        //{
        //    set
        //    {
        //        switch (value)
        //        {
        //            case "Red":
        //                SetColorByCode(5, 0, 0);
        //                break;
        //            case "Orange":
        //                SetColorByCode(5, 1, 0);
        //                break;
        //            case "Yellow":
        //                SetColorByCode(4, 2, 0);
        //                break;
        //            case "LawnGreen":
        //                SetColorByCode(3, 5, 0);
        //                break;
        //            case "SpringGreen":
        //                SetColorByCode(0, 5, 1);
        //                break;
        //            case "Cyan":
        //                SetColorByCode(0, 5, 3);
        //                break;
        //            case "DodgerBlue":
        //                SetColorByCode(0, 3, 5);
        //                break;
        //            case "Blue":
        //                SetColorByCode(0, 0, 5);
        //                break;
        //            case "DarkViolet":
        //                SetColorByCode(3, 0, 4);
        //                break;
        //            case "Magenta":
        //                SetColorByCode(5, 0, 2);
        //                break;
        //            case "Black":
        //                SetColorByCode(0, 0, 0);
        //                break;
        //            case "White":
        //                SetColorByCode(5, 5, 5);
        //                break;
        //            default:
        //                SetColorByCode(0, 0, 0);
        //                break;
        //        }
        //    }
        //}
    }
}