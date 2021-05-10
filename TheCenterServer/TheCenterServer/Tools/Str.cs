using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCenterServer
{
    public static class Str
    {
        public static string UTF8(this string utf16)
        {
            return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(utf16));
        }
    }
}
