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

        //public Color ColorLowerLine { get; set; }        

        //public Color ColorUpperLine { get; set; }

        //public Color ColorBackground { get; set; }

        private readonly IList<DisplayColorRgb> colors = new List<DisplayColorRgb>();

        public DisplayColorRgb ColorBackgroundRgb { get; set; }

        public DisplayColorRgb ColorUpperLineRgb { get; set; }

        public DisplayColorRgb ColorLowerLineRgb { get; set; }

        public string ColorCode()
        {
            return ColorUpperLineRgb.ColorCode
                   + ColorLowerLineRgb.ColorCode
                   + ColorBackgroundRgb.ColorCode;
        }
        public void SetColorByIndex(int index, Color color)
        {
            colors[index].Color = color;
        }
        //public void SetColorNameByIndex(int index, string colorName)
        //{
        //    colors[index].ColorName = colorName;
        //}

        public void SetRedByIndex(int index, int value)
        {
            colors[index].Red = value;
        }

        public int GetRedByIndex(int index)
        {
            return colors[index].Red;
        }

        public void SetGreenByIndex(int index, int value)
        {
            colors[index].Green = value;
        }

        public int GetGreenByIndex(int index)
        {
            return colors[index].Green;
        }

        public void SetBlueByIndex(int index, int value)
        {
            colors[index].Blue = value;
        }

        public int GetBlueByIndex(int index)
        {
            return colors[index].Blue;
        }

        public DisplayColors()
        {
            ColorLowerLineRgb = new DisplayColorRgb();
            ColorUpperLineRgb = new DisplayColorRgb();
            ColorBackgroundRgb = new DisplayColorRgb();
            colors.Add(ColorUpperLineRgb);
            colors.Add(ColorLowerLineRgb);
            colors.Add(ColorBackgroundRgb);
        }

        //public string ColorBackgroundName
        //{
        //    set => ColorBackgroundRgb.ColorName = value;
        //}

        //public string ColorLowerLineName
        //{
        //    set => ColorLowerLineRgb.ColorName = value;
        //}

        //public string ColorUpperLineName
        //{
        //    set => ColorUpperLineRgb.ColorName = value;
        //}

    }
}
