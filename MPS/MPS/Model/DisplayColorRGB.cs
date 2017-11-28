using Xamarin.Forms;

namespace MPS.Model
{
    public class DisplayColorRgb
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public string ColorName
        {
            set
            {
                switch (value)
                {
                    case "Red":
                        Red = 5;
                        Green = 0;
                        Blue = 0;
                        break;
                    case "Orange":
                        Red = 5;
                        Green = 1;
                        Blue = 0;
                        break;
                    case "Yellow":
                        Red = 4;
                        Green = 2;
                        Blue = 0;
                        break;
                    case "LawnGreen":
                        Red = 3;
                        Green = 5;
                        Blue = 0;
                        break;
                    case "SpringGreen":
                        Red = 0;
                        Green = 5;
                        Blue = 1;
                        break;
                    case "Cyan":
                        Red = 0;
                        Green = 5;
                        Blue = 3;
                        break;
                    case "DodgerBlue":
                        Red = 0;
                        Green = 3;
                        Blue = 5;
                        break;
                    case "Blue":
                        Red = 0;
                        Green = 0;
                        Blue = 5;
                        break;
                    case "BlueViolet":
                        Red = 2;
                        Green = 0;
                        Blue = 5;
                        break;
                    case "Magenta":
                        Red = 5;
                        Green = 0;
                        Blue = 5;
                        break;
                    case "Black":
                        Red = 0;
                        Green = 0;
                        Blue = 0;
                        break;
                    case "White":
                        Red = 5;
                        Green = 5;
                        Blue = 5;
                        break;
                    default:
                        Red = 0;
                        Green = 0;
                        Blue = 0;
                        break;
                }
            }
        }

        public string ColorCode => Red.ToString() + Green + Blue;

        public Color Color
        {
            //get;
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
                else if (value == Color.BlueViolet)
                {
                    SetColorByCode(2, 0, 5);
                }
                else if (value == Color.Magenta)
                {
                    SetColorByCode(5, 0, 5);
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