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
            return ColorUpperLineRgb.GetColorCode
                   + ColorLowerLineRgb.GetColorCode
                   + ColorBackgroundRgb.GetColorCode;
        }
        public bool SetColorByIndex(int index, Color color)
        {

            if (index == IndexBackgroundLineColor)
            {
                if (DisplayColorRgb.ColorsDictionary[color] == ColorLowerLineRgb.GetColorCode ||
                    DisplayColorRgb.ColorsDictionary[color] == ColorUpperLineRgb.GetColorCode)
                {
                    return false;
                }
            }
            else
            {
                if (DisplayColorRgb.ColorsDictionary[color] == ColorBackgroundRgb.GetColorCode)
                {
                    return false;
                }
            }
            _colors[index].Color = color;
            return true;
        }

        public void SetRedByIndex(int index, int value)
        {
            _colors[index].Red = value;
        }

        public int GetRedByIndex(int index)
        {
            return _colors[index].Red;
        }

        public void SetGreenByIndex(int index, int value)
        {
            _colors[index].Green = value;
        }

        public int GetGreenByIndex(int index)
        {
            return _colors[index].Green;
        }

        public void SetBlueByIndex(int index, int value)
        {
            _colors[index].Blue = value;
        }

        public int GetBlueByIndex(int index)
        {
            return _colors[index].Blue;
        }

        public DisplayColors()
        {
            ColorLowerLineRgb = new DisplayColorRgb();
            ColorUpperLineRgb = new DisplayColorRgb();
            ColorBackgroundRgb = new DisplayColorRgb();            
            _colors.Insert(IndexUpperLineColor, ColorUpperLineRgb);
            _colors.Insert(IndexLowerLineColor, ColorLowerLineRgb);
            _colors.Insert(IndexBackgroundLineColor, ColorBackgroundRgb);
        }


    }
}
