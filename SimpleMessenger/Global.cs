﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMessenger
{
    public static class Global
    {
        public static string username = "";

        public struct MessageObject
        {
            public int MessageID;
            public string Message;
            public DateTime datetime;
            public string user;
        }
    }
}
