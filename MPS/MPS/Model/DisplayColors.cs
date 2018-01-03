using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.Model
{
    public class DisplayColors
    {

        private readonly IList<DisplayColorRgb> _colors = new List<DisplayColorRgb>();
        public const int IndexUpperLineColor = 0;
        public const int IndexLowerLineColor = 1;
        public const int IndexBackgroundLineColor = 2;

        public DisplayColorRgb ColorBackgroundRgb { get; set; }

        public DisplayColorRgb ColorUpperLineRgb { get; set; }

        public DisplayColorRgb ColorLowerLineRgb { get; set; }

        public string GetColorCode()
        {
            return ColorUpperLineRgb.ColorCode
                   + ColorLowerLineRgb.ColorCode
                   + ColorBackgroundRgb.ColorCode;
        }
        public bool SetColorByIndex(int index, Color color)
        {
            if (index == IndexBackgroundLineColor)
            {
                if (DisplayColorRgb.ColorsDictionary[color] == ColorLowerLineRgb.ColorCode ||
                    DisplayColorRgb.ColorsDictionary[color] == ColorUpperLineRgb.ColorCode)
                {
                    return false;
                }
            }
            else
            {
                if (DisplayColorRgb.ColorsDictionary[color] == ColorBackgroundRgb.ColorCode)
                {
                    return false;
                }
            }
            _colors[index].Color = color;
            return true;
        }

        public void SetColorCodeByIndex(int index, string colorCode)
        {
            if (index == IndexBackgroundLineColor)
            {
                if (colorCode == ColorLowerLineRgb.ColorCode ||
                    colorCode == ColorUpperLineRgb.ColorCode)
                {
                    throw new ColorException();
                }
            }
            else
            {
                if (colorCode == ColorBackgroundRgb.ColorCode)
                {
                    throw new ColorException();
                }
            }
            _colors[index].ColorCode = colorCode;         
        }

        public string GetColorCodeByIndex(int index)
        {
            return _colors[index].ColorCode;
        }

        public int GetRedByIndex(int index)
        {
            return _colors[index].ColorCode[DisplayColorRgb.IndexRed] - 48;
        }

        public int GetGreenByIndex(int index)
        {
            return _colors[index].ColorCode[DisplayColorRgb.IndexGreen] - 48;
        }

        public int GetBlueByIndex(int index)
        {
            return _colors[index].ColorCode[DisplayColorRgb.IndexBlue] - 48;
        }

        public DisplayColors()
        {
            ColorLowerLineRgb = new DisplayColorRgb { Color = Color.Blue };
            ColorUpperLineRgb = new DisplayColorRgb { Color = Color.Blue };
            ColorBackgroundRgb = new DisplayColorRgb();
            _colors.Insert(IndexUpperLineColor, ColorUpperLineRgb);
            _colors.Insert(IndexLowerLineColor, ColorLowerLineRgb);
            _colors.Insert(IndexBackgroundLineColor, ColorBackgroundRgb);
        }

    }
}
