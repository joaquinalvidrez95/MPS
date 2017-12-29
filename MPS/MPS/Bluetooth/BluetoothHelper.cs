using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Bluetooth
{
    public class BluetoothHelper
    {
        public class BluetoothUuid
        {
            public const string ServiceUuid = "0000FFE0-0000-1000-8000-00805F9B34FB";
            public const string CharacteristicUuid = "0000FFE1-0000-1000-8000-00805F9B34FB";
        }

        public class BluetoothContract
        {
            //public const string Speed = "V";
            //public const string DateTime = "A";
            //public const string View = "P";
            //public const string Colours = "C";
            //public const string Message = "M";
            //public const string Power = "E";
            public const string Request = "R";
            public const string Feedback = "S";
            public const string PinOk = "J";

            //public const string ViewMode = "F";
            //public const string Visibilities = "B";
        }
    }
}
