using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine.Networking;

namespace AISandbox
{
    public enum FSMMsgType
    {
        GETTOKEY,
        GETTODOOR,
        GETTOTREASURE,
        OPENDOOR,
        PICKUPKEY,
        PICKUPTREASURE
    }

    public struct Telegram
    {
        public FSMMsgType  messageType;
        public object sender;
        public object content;
    }
}
