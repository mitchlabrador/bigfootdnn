using System;
using System.Linq;
using System.Web;

namespace BigfootDNN.Helpers
{
    public class ContextHelper
    {

        /// <summary>
        /// This is a quick reference to the current HttpContext.Curren
        /// </summary>
        public static HttpContext Context { get { return HttpContext.Current; } }

        /// <summary>
        /// Determines whether the key is in the cache
        /// </summary>
        public static bool HasData(string key)
        {
            return Context.Items.Contains(key);
        }
        
        /// <summary>
        /// Gets a value from the context... returns null if empty
        /// </summary>
        /// <returns>Object in the cache or null if nothing found</returns>
        public static object GetData(string key)
        {
            return Context.Items.Contains(key) ? Context.Items[key] : null;
        }

        /// <summary>
        /// Sets a value. If found it overrides the current value, otherwise it adds it
        /// </summary>
        public static void SetData(string key, object value)
        {
            if (Context.Items.Contains(key))
                Context.Items[key] = value;
            else
                Context.Items.Add(key, value);
        }

        /// <summary>
        /// Removes a value from the cache safely. It does not throw an error if the value is not found.
        /// </summary>
        /// <param name="key">The key to the cahced value</param>
        public static void RemoveData(string key)
        {
            if (HasData(key)) HasData(key);
        }
                

        /// <summary>
        /// Returns the value stored in the cache as an integer
        /// </summary>
        /// <param name="key">The parameter key to the value</param>
        /// <returns>The cached value</returns>
        public static int GetInt(String key)
        {
            var val = GetData(key);
            return val == null ? 0 : int.Parse(val.ToString());
        }

        /// <summary>
        /// Returns the value stored in the cache as an integer
        /// </summary>
        /// <param name="key">The parameter key to the value</param>
        /// <returns>The cached value</returns>
        public static bool GetBool(String key)
        {
            var val = GetData(key);
            return val == null ? false : bool.Parse(val.ToString());
        }

        internal static T GetData<T>(string key)
        {
            var data = GetData(key);
            if (data == null)
            {
                return (T)data;
            }
            else
            {
                return default(T);
            }
        }

        
    }
}
