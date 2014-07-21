using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigfootDNN.Helpers
{
    public static class ObjectExtensions
    {

        public static int ToInt(this object obj)
        {
            var val = default(int);
            if (obj == null) return val;
            if (obj.GetType() == typeof(int)) 
                return (int)obj;
            else
                int.TryParse(obj.ToString(), out val);
            return val;
        }

        public static bool ToBool(this object obj)
        {
            bool val = false;
            if (obj != null)
            {
                if (obj.GetType() == typeof(bool))
                    return (bool)obj;
                else
                    bool.TryParse(obj.ToString(), out val);
            }
            return val;
        }

        public static HtmlString ToHtml(this object obj)
        {
            if (obj != null)
            {
                return new HtmlString(obj.ToString());
            }
            else if (obj.GetType() == typeof(HtmlString)){
                return obj as HtmlString;
            }
            else
            {
                return new HtmlString("");
            }
        }

        public static T ToEnum<T>(this object obj)
        {            
            
            if (obj == null) return default(T);
            int val;
            if (int.TryParse(obj.ToString(), out val)){
                return (T)Convert.ChangeType(val, typeof(T));
            }
            else {
                return (T)Enum.Parse(typeof(T), obj.ToString());
            }            
        }

        public static int? ToNullableInt(this object obj)
        {
            var rv = new int?();
            if (obj == null) return rv;
            if (obj.GetType() == typeof(int?))
                rv = (int?)obj;
            else
                rv = obj.ToInt();
            return rv;
        }

    }
}