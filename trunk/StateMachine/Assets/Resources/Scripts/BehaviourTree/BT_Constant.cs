using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AISandbox
{
    enum BT_Status
    {
        SUCCESS,
        FAILURE,
        RUNNING,
        ERROR
    }
    class BT_Constant
    {

        public static string CreateUUID()
        {
            char[] uuid = new char[36];
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            for (int i = 0; i < 36; i++)
            {
                uuid[i] = hexDigits[Rnd.rnd.Next(0, 0x10)];
            }

            // bits 12-15 of the time_hi_and_version field to 0010
            uuid[14] = '4';

            // bits 6-7 of the clock_seq_hi_and_reserved to 01
            uuid[19] = hexDigits[(uuid[19] & 0x3) | 0x8];

            uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
            return new string(uuid);
        }
    }
}
