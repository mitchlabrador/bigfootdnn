using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BigfootDNN.Helpers
{
    /// <summary>
    /// This is a global application cache that persists accross requests while the application lives. Please takenote that it is server specific
    /// </summary>
    public static class MemoryCache
    {
        static Dictionary<string, object> _cache = new Dictionary<string, object>();


        #region Create unique keys at the host / portal / module level

        /// <summary>
        /// Creates a unique parameter for your module that is unique for this DNN installation. Does not differentiate accross portals, or module instances.
        /// This is particularly useful for things like the database provider cache key etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <param name="moduleShortName">This is the short name of your module</param>
        /// <returns>Returns {key}_mn_{ModuleShortName}</returns>
        public static string HostKey(string key, string moduleShortName)
        {
            return key + "_mn_" + moduleShortName;
        }

        /// <summary>
        /// Creates a unique parameter for your module that is unique for this specific portal. Does not differentiate accross module instances within this portal.
        /// This is particularly useful for things like portal specific data caches etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <param name="portalId">The id for the portal this key refers to</param>
        /// <param name="moduleShortName">This is the short name of your module</param>
        /// <returns>Returns {key}_mn_{moduleshortname}_pid_{portalId}</returns>
        public static string PortalKey(string key, int portalId, string moduleShortName)
        {
            return key + "_mn_" + moduleShortName + "_pid_" + portalId;
        }

        /// <summary>
        /// Creates a unique parameter for your module that is unique for this specific module instance
        /// This is particularly useful for things like module instance route etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <param name="moduleId">The id for the module instance this key refers to</param>
        /// <returns>Returns {key}_pid_{portalId}</returns>
        public static string ModuleKey(string key, int moduleId)
        {
            return key + "_mid_" + moduleId;
        }

        #endregion
        

        /// <summary>
        /// Determine weather a certain key is contained in the cache
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns>True if found</returns>
        public static bool Contains(String key) { return _cache.ContainsKey(key); }

        /// <summary>
        /// Returns the value stored in the cache
        /// </summary>
        /// <param name="key">The parameter key to the value</param>
        /// <returns>The cached value</returns>
        public static object GetValue(String key)
        {
            return (_cache.ContainsKey(key)) ? _cache[key] : null;
        }

        /// <summary>
        /// Returns the value stored in the cache for a particular type
        /// </summary>
        /// <param name="key">The parameter key to the value</param>
        /// <returns>The cached value</returns>
        public static T GetValue<T>(String key)
        {
            var rv = default(T);
            if (_cache.ContainsKey(key)) 
                rv = (T)_cache[key];
            return rv;            
        }

        /// <summary>
        /// Returns the value stored in the cache as an integer
        /// </summary>
        /// <param name="key">The parameter key to the value</param>
        /// <returns>The cached value</returns>
        public static int GetInt(String key)
        {
            var val = GetValue(key);
            return val == null ? 0 : int.Parse(val.ToString());
        }
            
        /// <summary>
        /// Adds a value to the cache. Replaces any excisting value if found
        /// </summary>
        /// <param name="key">The key of the value to add</param>
        /// <param name="data">The data to add to the cache</param>
        public static void Add(String key, object data)
        {
            if (_cache.ContainsKey(key))
                _cache[key] = data;
            else
                _cache.Add(key, data);
        }

        /// <summary>
        /// Removes a value from the cache safely. It does not throw an error if the value is not found.
        /// </summary>
        /// <param name="key">The key to the cahced value</param>
        public static void Remove(string key)
        {
            if (_cache.ContainsKey(key)) _cache.Remove(key);
        }


        /// <summary>
        /// Get the fields / properties to hydrate for an object. 
        /// Caches the object map in order to maximize performance. 
        /// So reflection is used only first time on n object type
        /// </summary>
        /// <param name="obj">Object to use to hydrate</param>
        /// <returns>A list of Fields and Properties</returns>
        public static List<object> GetObjectFields(object obj)
        {
            var cacheKey = "reflectioncache_" + obj.GetType().FullName;
            List<object> fields;
            if (Contains(cacheKey))
                fields = GetValue(cacheKey) as List<object>;
            else
            {
                fields = new List<object>();
                foreach (PropertyInfo p in obj.GetType().GetProperties())
                {
                    if (p.CanWrite) fields.Add(p);
                }
                foreach (FieldInfo f in obj.GetType().GetFields())
                {
                    if (f.IsPublic && !f.IsStatic) fields.Add(f);
                }

                //fields.AddRange(obj.GetType().GetFields());

                Add(cacheKey, fields);
            }
            return fields;
        }

    }

    
        
}