using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace BigfootDNN
{

    public partial class DnnAppParams
    {
        public RouteInfo Route { get; set; }

        public DnnMvcApplication App { get { return Route.App; } }

        public DnnAppParams(RouteInfo route)
        {
            Route = route;
        }

        /// <summary>
        /// Sotres the provodier configuration for the application
        /// </summary>
        public string Cache_ProviderConfiguration() { return HostKey("ProviderConfiguration"); }

        /// <summary>
        /// Stores the portal id data path. This is specific for the portalid specified
        /// </summary>
        public string Cache_DataFolder() { return PortalKey("DataFolderPath"); }

        /// <summary>
        /// The key to store module settings for a particular module instance. The module id is added to make it unique
        /// </summary>
        public string Cache_ModuleSettings()
        {
            return ModuleKey("ModuleSettings");
        }
                              
        /// <summary>
        /// Creates a unique parameter for your module that is unique for this DNN installation. Does not differentiate accross portals, or module instances.
        /// This is particularly useful for things like the database provider cache key etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <param name="moduleShortName">This is the short name of your module</param>
        /// <returns>Returns {key}_mn_{ModuleShortName}</returns>
        public string HostKey(string key)
        {
            return key + "_mn_" + App.Info.ModuleShortName;
        }
        
        /// <summary>
        /// Creates a unique parameter for your module that is unique for this specific portal. Does not differentiate accross module instances within this portal.
        /// This is particularly useful for things like portal specific data caches etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <returns>Returns {key}_mn_{moduleshortname}_pid_{portalId}</returns>
        public string PortalKey(string key)
        {
            return key + "_mn_" + App.Info.ModuleShortName + "_pid_" + Route.PortalId;
        }

        /// <summary>
        /// Creates a unique parameter for your module that is unique for this specific module instance
        /// This is particularly useful for things like module instance route etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <returns>Returns {key}_pid_{portalId}</returns>
        public string ModuleKey(string key)
        {
            return key + "_mid_" + Route.ModuleId;
        }
        
        /// <summary>
        /// QS = Well known querystring parameters used in the module
        /// </summary>
        

        /// <summary>
        /// Holds the route data for a particular module id in the page
        /// </summary>
        public string QS_Route(int moduleId) { return ModuleKey("route"); }


        /// <summary>
        /// Holds the route data for a module. Used in ajax request, where there is only one module being executed
        /// </summary>
        /// <param name="moduleShortName">This is the short name of your module</param>
        public string QS_AjaxRoute() { return HostKey("route"); }
        

    }
}

