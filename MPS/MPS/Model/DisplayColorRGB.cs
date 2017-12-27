using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MPS.Model
{
    public class DisplayColorRgb
    {      

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

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

        public string ColorCode => Red.ToString() + Green + Blue;

        public Color Color
        {
            set
            {
                if (value == Color.Red)
                {
                    SetColorByCode(5, 0, 0);
                }
                else if (value == Color.Orange)
                {
                    SetColorByCode(5, 1, 0);
                }
                else if (value == Color.Yellow)
                {
                    SetColorByCode(4, 2, 0);
                }
                else if (value == Color.LawnGreen)
                {
                    SetColorByCode(3, 5, 0);
                }
                else if (value == Color.SpringGreen)
                {
                    SetColorByCode(0, 5, 1);
                }
                else if (value == Color.Cyan)
                {
                    SetColorByCode(0, 5, 3);
                }
                else if (value == Color.DodgerBlue)
                {
                    SetColorByCode(0, 3, 5);
                }
                else if (value == Color.Blue)
                {
                    SetColorByCode(0, 0, 5);
                }
                else if (value == Color.DarkViolet)
                {
                    SetColorByCode(3, 0, 4);
                }
                else if (value == Color.Magenta)
                {
                    SetColorByCode(5, 0, 2);
                }
                else if (value == Color.Black)
                {
                    SetColorByCode(0, 0, 0);
                }
                else if (value == Color.White)
                {
                    SetColorByCode(5, 5, 5);
                }
            }
        }

        private void SetColorByCode(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}