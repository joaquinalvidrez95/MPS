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
    }
}