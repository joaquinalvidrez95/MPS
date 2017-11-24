using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Bluetooth
{
    public class BluetoothHelper
    {
        public class BluetoothUUID
        {
            public const string SERVICE_UUID = "0000FFE0-0000-1000-8000-00805F9B34FB";
            public const string CHARACTERISTIC_UUID = "0000FFE1-0000-1000-8000-00805F9B34FB";
        }

        public class BluetoothContract
        {
            public const string SPEED = "V";
            public const string DATE_TIME = "A";
            public const string VIEW = "P";
            public const string COLOURS = "C";
            public const string MESSAGE = "M";           
        }
    }
}
