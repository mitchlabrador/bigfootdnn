using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigfootDNN.Helpers
{
    public static class EnumExtensions
    {

        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }
    }
}