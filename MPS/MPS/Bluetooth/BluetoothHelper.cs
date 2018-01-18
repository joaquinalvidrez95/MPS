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
            public const string Request = "R";
            public const string OnFeedbackReceived = "S";
            public const string OnPinStatusReceived = "J";
            public const string Pin = "N";

        }
    }
}
