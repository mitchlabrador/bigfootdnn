using System;
using System.Collections;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Security.Permissions;
using BigfootDNN.Helpers;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Reflection;

namespace BigfootDNN
{

    /// <summary>
    /// This class represents the route information. It includes controller, action, portalid, tabid, and moduleid
    /// This info is by default created from the url in the route parameter.
    /// </summary>
    public partial class RouteInfo
    {

        private static string QSRouteParam(int moduleId)
        {
            return "route_mid_" + moduleId;
        }

        private static string QSAjaxRouteParam(string moduleShortName)
        {
            return "route_mn_" + moduleShortName;
        }

        /// <summary>
        /// Receives the route data from the url and converts it into a RouteInfo object
        /// </summary>
        /// <param name="appInfo">The information about the app currently executing</param>
        /// <param name="mdoule">If loading from an existing PortalModuleBase use it, otherwise create a new one (new ones are always required for ajax requests)</param>
        public static RouteInfo CreateFromRequest(AppInfo appInfo, PortalModuleBase module = null)
        {
            // Determine whether this is an Ajax request
            var isajaxrequest = module == null;

            // Get the current route request
            var paramName = !isajaxrequest ? QSRouteParam(module.ModuleId) 
                                           : QSAjaxRouteParam(appInfo.ModuleShortName);

            // Get the route data
            var values = new PostHelper();
            var routeData = values.GetValue(paramName);

            // Controller and Action must be specified in all ajax requests
            if (string.IsNullOrEmpty(routeData) && isajaxrequest)
            {
                throw new ApplicationException("The Route data for this request is missing");
            }

            // Define the route
            RouteInfo Route;

            // Default to Home and index
            if (string.IsNullOrEmpty(routeData))
            {
                var defaultController = "Home";
                var defaultAction = "index";
                var settController = module.Settings["DefaultController"];
                var settAction = module.Settings["DefaultAction"];
                if ((settController != null && !settController.ToString().IsNullOrEmpty()) &&
                    (settAction != null && !settAction.ToString().IsNullOrEmpty()))
                {
                    defaultController = settController.ToString();
                    defaultAction = settAction.ToString();
                }

                // Create the default route for this module
                Route = new RouteInfo(defaultController, defaultAction, module, appInfo);
            }
            else
            {
                var routeVars = routeData.Split('-');

                // Validate the route data parameter
                if (routeVars.Length != 5) throw new ApplicationException("Invalid route: " + routeData);

                var controller = routeVars[0];
                var action = routeVars[1];
                var tabId = int.Parse(routeVars[2]);
                var portalId = int.Parse(routeVars[3]);
                var moduleId = int.Parse(routeVars[4]);

                var modController = new ModuleController();

                // For ajax requests load the module base from the database as it is not suplied
                if (isajaxrequest)
                    module = new PortalModuleBase { ModuleConfiguration = modController.GetModule(moduleId, tabId) };

                // Create the new route
                Route = new RouteInfo(controller, action, module, appInfo);
            }


            // Load the user if needed
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Route.User = module.UserInfo;
            }

            // Return the newly created route
            return Route;
            
        }

        /// <summary>
        /// Creates the Route data to be included in the URL Querystring. It inlcudes the parameter name.
        /// </summary>
        /// <param name="paramName">The name of the parameter to use in the querystring</param>
        /// <param name="controller">The name of the controller that contains the action</param>
        /// <param name="action">The name of the action within the controller</param>
        /// <param name="moduleId">The module id to include in the url route</param>
        /// <param name="querystring">Appends this value to the querystring </param>
        /// <param name="portalId">The portal id to include in the url route</param>
        /// <param name="tabId">The tab id to include in the url route</param>
        /// <returns>A url that may be used from the browser to make an ajax request</returns>
        public static List<string> BuildUrlRoute(bool isAjaxRequest, string controller, string action, int portalId, int tabId, int moduleId, string moduleShortName, string querystring)
        {
            var paramName = isAjaxRequest ? QSAjaxRouteParam(moduleShortName)
                                          : QSRouteParam(moduleId);

            // Append any other routes currently in this page

            // Build the route data
            var routeData = string.Format("{0}-{1}-{2}-{3}-{4}",
                                           controller,
                                           action,
                                           tabId,
                                           portalId,
                                           moduleId);

            var qs = new List<string>();
            //qs.Add(routeData);
            const string paramFormat = "{0}={1}";

            // Add the route parameter
            qs.Add(string.Format(paramFormat, paramName, routeData));

            // Add the querystring parameters
            var portalIdInQueryString = false;
            if (!string.IsNullOrEmpty(querystring))
            {
                foreach (var s in querystring.Split('&'))
                {
                    var qsItem = s.Split('=');
                    if (string.IsNullOrEmpty(s)) continue;
                    qs.Add(qsItem.Length > 1 ? string.Format(paramFormat, qsItem[0], qsItem[1]) :
                                               string.Format(paramFormat, s, ""));
                    // Check whether the portalid is in the query string
                    if (qsItem[0].ToLowerInvariant() == "portalid") portalIdInQueryString = true;
                }
            }

            // Add any other route paramters currently in the request. When in AjaxRequest mode, although these are not used
            // it makes calls to the MvcUrl or AjaxUrl functions from views being executed within an Ajax Request keep their 
            // place for other modules within the page. Be it the same or another module also using BigfootDNN.BigfootMVC
            foreach (var qp in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (!qp.ToLower().StartsWith("route_") || qp.Equals(paramName)) continue;

                var value = HttpContext.Current.Request.QueryString[qp];
                qs.Add(string.Format(paramFormat, qp, value));
            }

            // Always add the portalid for ajax requests. 
            // Otherwise it logs out the user so the request won't be authenticated even though the user has logged in
            if (paramName == QSAjaxRouteParam(moduleShortName) && !portalIdInQueryString)
            {
                qs.Add(string.Format(paramFormat, "portalid", portalId));
            }

            return qs;
        }


        /// <summary>
        /// Creates a new RouteInfo object by specifying the full range of parameters
        /// </summary>
        public RouteInfo(string controller, string action, PortalModuleBase module, AppInfo appInfo)
        {
            Controller = controller;
            Action = action;
            Module = module;
            App = new DnnMvcApplication(appInfo, this);
        }


        /// <summary>
        /// This is the heart of your DNN module application. It contains your module informaiton, as well as the methods
        /// and helpers you may use throughout your application. It also is the main source for the StaticUrl MvcUrl and ControlUrl
        /// methods that the baseview classes inherit. It also has all the functions inherited from the BigfootMVC.WebApp 
        /// you may use in all ASP.NET applications.
        /// </summary>
        public DnnMvcApplication App { get; set; }

        /// <summary>
        /// Name of the controller for this route
        /// </summary>
        public string Controller;

        /// <summary>
        /// Name of the action within the controller
        /// </summary>
        public string Action;

        /// <summary>
        /// This is the module configuration for the PortalModuleBase under which context we are executing
        /// </summary>
        public PortalModuleBase Module { get; set; }


        #region GetViewPath | ExecuteAction

        /// <summary>
        /// Probes the different view paths to find the control
        /// 1. Controller and ViewName is empty then it defaults to home/index.ascx | index.csthml
        /// 2. Controller is empty then it defaults to home/
        /// 3. ViewName is empty then it defaults Index.ascx | index.cshtml
        /// 4. If ViewName is not found in the Controller path specified, then it tries to find it in shared/ path under the views directory
        /// </summary>
        /// <param name="controller">
        /// The folder name that contains the view. Can include a full relative path to the file, or a full relative path to the folder that contains this view
        /// i.e. ~/Reports/Employees/EmployeeReport.ascx --> Used literally
        ///      ~/Reports/Employees/EmployeeReport.cshtml --> Used literally
        ///      ~/Reports/Employees --> Translates to ~/Reports/Employees/[viewName]
        ///      Orders --> Transalates to ~/Views/Orders/[viewName]
        ///      Orders/Test --> Translates to ~/Views/Orders/Test/[viewName]
        /// </param>
        /// <param name="viewName">
        /// The ViewName within the section. 
        ///     i.e. AddEmployee --> AddEmployee.ascx | AddEmployee.cshtml
        ///          AddEmployee.ascx --> Used literally
        ///          AddEmployee.cshtml --> Used literally
        ///          AddEmployee.ashx --> Used literally
        /// </param>
        /// <param name="app">This is the DnnApp object created by your module and attached to the RouteInfo object. It is used in order identify the module paths</param>
        /// <returns>The application relative path to the control</returns>
        public string GetViewPath(string controller, string viewName)
        {
            // Default to Home for the controller and Index for the viewName
            if (string.IsNullOrEmpty(controller)) controller = "Home";
            if (string.IsNullOrEmpty(viewName)) viewName = "index";


            // Determine weather the user is specifying the full controller path
            var ControllerIsRelativePath = controller.StartsWith("~/");
            var ControllerIncludesViewName = controller.ToLowerInvariant().EndsWith(".cshtml");


            // Add the / slash to the end of the controller's folder name. Do not do this if the controller includes the view name
            if (!ControllerIncludesViewName && !controller.EndsWith("/")) controller += "/";
           
            // Make sure the view files have their respective extensions attached
            if (!viewName.ToLowerInvariant().EndsWith(".cshtml"))
            {
                viewName += ".cshtml";
            }

            // Do not search in the shared folder if the folderName is a full relative path
            if (ControllerIncludesViewName)
            {
                if (File.Exists(App.Server.MapPath(controller)))
                    return controller;
                else
                    throw new ApplicationException(String.Format("Unable to find control {0}", controller));
            }

            // Do not append the global Views path when the controller includes a relative path
            if (ControllerIsRelativePath)
            {
                if (File.Exists(App.Server.MapPath(controller + viewName)))
                {
                    return controller + viewName;
                }
                else if (File.Exists(App.Server.MapPath(controller + "Shared/" + viewName)))
                {
                    return controller + "Shared/" + viewName;
                }
                else
                {
                    throw new ApplicationException(String.Format(App.MVCStrings.UnableToFindControl,
                                                                 controller + viewName,
                                                                 controller + "Shared/" + viewName));
                }
            }

            // Search first the natural location. Then try the shared location
            if (File.Exists(App.Server.MapPath(App.ViewsPath + controller + viewName)))
            {
                return App.ViewsPath + controller + viewName;
            }
            else if (File.Exists(App.Server.MapPath(App.ViewsPath + "Shared/" + viewName)))
            {
                return App.ViewsPath + "Shared/" + viewName;
            }            
            else
            {
                throw new ApplicationException(String.Format(App.MVCStrings.UnableToFindControl,
                                                             App.ViewsPath + controller + viewName,
                                                             App.ViewsPath + "Shared/" + viewName));
            }
        }


        /// <summary>
        /// Executes an action and returns the action result
        /// </summary>
        /// <param name="route">The route to execute</param>
        public ActionResult ExecuteRoute()
        {
            // Normalize the namespace
            var ns = App.Info.ModuleNamespace;
            if (string.IsNullOrEmpty(App.Info.ModuleNamespace) != true) ns += ".";

            // Add the controllers namespace
            ns += "Controllers.";

            // Get the literals to use
            var typename = ns + Controller + "Controller";
            var controllerKey = typename.ToLower() + ":ControllerCache";
            var actionKey = controllerKey + Action.ToLower() + ":ActionCache";

            // CONTROLLER
            //      Get the type from the cache, if not there add it
            var objT = SimpleCache.GetValue<Type>(controllerKey);
            if (objT == null)
            {
                // Get the object type
                objT = Type.GetType(typename + ", " + App.Info.ModuleAssemblyName);

                // Try it with Uppper case if it fails
                if (objT == null && !string.IsNullOrEmpty(Controller))
                {
                    var controllerChars = Controller.ToCharArray();
                    controllerChars[0] = char.ToUpper(controllerChars[0]);
                    Controller = new string(controllerChars);

                    typename = ns + Controller + "Controller";

                    // Try to get the type again
                    objT = Type.GetType(typename + ", " + App.Info.ModuleAssemblyName);
                }

                // Add it to the cache
                if (objT != null) SimpleCache.Add(controllerKey, objT);
            }
            //      Ensure the controller was found
            if (objT == null) throw new ApplicationException(string.Format(App.MVCStrings.ControllerNotFound, typename, App.Info.ModuleAssemblyName));
            //      Construct the controller and set the route information
            var objController = (BaseController)objT.InvokeMember("ctor", BindingFlags.CreateInstance, null, null, null);
            objController.Route = this;
            // ACTION
            //      Get the method from the cache. If not there then get it and add it
            var cachedMethod = SimpleCache.GetValue<MethodInfo>(actionKey);
            if (cachedMethod == null)
            {
                foreach (var m in objT.GetMethods())
                {
                    if (m.Name.ToLower() == Action.ToLower())
                    {
                        SimpleCache.Add(actionKey, m);
                        cachedMethod = m;
                        break;
                    }
                }
            }
            //      Ensure the method exists
            if (cachedMethod == null) throw new ApplicationException(string.Format(App.MVCStrings.ActionNotFound, typename, Action));
            //      Execute the action
            return (ActionResult)cachedMethod.Invoke(objController, null);
        }

        #endregion
        

        #region "Module Settings"



        /// <summary>
        /// Retreives a module setting as an integer
        /// </summary>
        /// <param name="name">The name of the setting to retreive</param>
        public int? GetSettingAsInt(string name)
        {
            return GetSetting(name) != "" ? int.Parse(GetSetting(name))
                                          : new int?();
        }

        /// <summary>
        /// Retreives a module setting as an integer
        /// </summary>
        /// <param name="name">The name of the setting to retreive</param>
        /// <param name="defaultValue">Uses this default value if nothing is found</param>
        public int GetSettingAsInt(string name, int defaultValue)
        {
            return int.Parse(GetSetting(name, defaultValue.ToString()));
        }

        /// <summary>
        /// Retreives a module setting as a date
        /// </summary>
        /// <param name="name">The name of the setting to retreive</param>
        public DateTime? GetSettingAsDate(string name)
        {
            return GetSetting(name) != "" ? DateTime.Parse(GetSetting(name))
                                          : new DateTime();
        }

        /// <summary>
        /// Retreives a module setting as a date
        /// </summary>
        /// <param name="name">The name of the setting to retreive</param>
        /// <param name="defaultValue">Uses this as the default value if nothing is found</param>
        public DateTime GetSettingAsDate(string name, DateTime defaultValue)
        {
            return DateTime.Parse(GetSetting(name, defaultValue.ToString()));
        }

        /// <summary>
        /// Retreives a module setting as a decimal
        /// </summary>
        /// <param name="name">The name of the setting to retreive</param>
        public decimal? GetSettingAsDecimal(string name)
        {
            return GetSetting(name) != "" ? decimal.Parse(GetSetting(name))
                                          : new decimal?();
        }

        /// <summary>
        /// Retreives a module setting as a decimal
        /// </summary>
        /// <param name="name">The name of the setting to retreive</param>
        /// <param name="defaultValue">Uses this as the default value if the setting is not found</param>
        public decimal GetSettingAsDecimal(string name, decimal defaultValue)
        {
            return decimal.Parse(GetSetting(name, defaultValue.ToString()));
        }

        /// <summary>
        /// Retreives a module setting as a string
        /// </summary>
        /// <param name="name">The name of the setting to retreive</param>
        /// <returns>Returns an empty string if not found</returns>
        public string GetSetting(string name)
        {
            return GetSetting(name, "");
        }

        /// <summary>
        /// Retreives a module setting as a string
        /// </summary>
        /// <param name="name">The name of the setting to retreive</param>
        /// <param name="defaultValue">Uses this as the default value if the setting is not found</param>
        public string GetSetting(string name, string defaultValue)
        {
            return string.IsNullOrEmpty((string)Module.Settings[name]) == false ? (string)Module.Settings[name] : defaultValue;
        }

        #endregion


        /// <summary>
        /// Determines wheather the current user has edit rights to this module. It does not use the IsEditable as that would not work in ajax scenarios
        /// instead it goes directly to the tab info and identify if the tab has or module has edit rights
        /// </summary>
        public bool CanEdit
        {
            get
            {
                if (canEdit.HasValue == false)
                {
                    //var tabPermissions = (new TabController()).GetTab(TabId, PortalId, false).TabPermissions;
                    canEdit = //TabPermissionController.HasTabPermission(tabPermissions, "EDIT") || 
                              ModulePermissionController.HasModulePermission(Module.ModuleConfiguration.ModulePermissions, "EDIT");
                }
                return canEdit.Value;
            }
        }
        private bool? canEdit;

        /// <summary>
        /// Portal ID for this request
        /// </summary>
        public int PortalId { get { return Module.PortalId; } }

        /// <summary>
        /// Portal GUID for this request
        /// </summary>
        public string PortalGUID { get { return Module.PortalSettings.GUID.ToString(); } }

        /// <summary>
        /// TabId for this request
        /// </summary>
        public int TabId { get { return Module.TabId; } }

        /// <summary>
        /// ModuleId for this request
        /// </summary>
        public int ModuleId { get { return Module.ModuleId; } }

        /// <summary>
        /// Carries the user information for the current request
        /// </summary>
        public UserInfo User { get; set; }

        /// <summary>
        /// Returns the user id of the currently logged in user. Throws an error if the user is not logged in
        /// </summary>
        public int UserId
        {
            get
            {
                if (User == null)
                    throw new ApplicationException("No user has has been attached to this process");
                return User.UserID;
            }
        }

        /// <summary>
        /// Returns the user id of the currently logged in user or a default value you specify
        /// </summary>
        public int UserIdOrDefault(int defaultValue)
        {
            return (User == null) ? defaultValue : User.UserID;
        }

        /// <summary>
        /// Determines if the user is admin
        /// </summary>
        public bool UserIsAdmin
        {
            get
            {
                return (User == null ||
                         User.IsSuperUser ||
                         User.IsInRole("Administrators"));
            }
        }
        
        /// <summary>
        /// Determines if the user is a host user
        /// </summary>
        public bool UserIsHost { get { return (User == null || User.IsSuperUser); } }

        /// <summary>
        /// Creates a properly formatted url with the route parameter built from this RouteInfo object
        /// </summary>
        /// <returns>A url that can be used from the client</returns>
        public string GetMvcUrl()
        {
            // Add all query string parameters that are not routes. Other routes are automatically appended by the 
            // App.BuildUrlRoute function
            var querystring = "";
            foreach (string key in HttpContext.Current.Request.QueryString.Keys)
            {
                if (key.ToLowerInvariant().StartsWith("route_")) continue;
                querystring += "&" + key + "=" + App.Request.QueryString[key];
            }
            return App.MvcUrl(Controller, Action, querystring);
        }

        /// <summary>
        /// Creates a properly formatted url with the route parameter built from this RouteInfo object
        /// </summary>
        /// <returns>A url that can be used from the client</returns>
        public string GetMvcAjaxUrl()
        {
            var querystring = "";
            foreach (string key in HttpContext.Current.Request.QueryString.Keys)
            {
                if (key == QSAjaxRouteParam(App.Info.ModuleShortName)) continue;
                querystring += "&" + key + "=" + App.Request.QueryString[key];
            }
            return App.MvcAjaxUrl(Controller, Action, querystring);
        }

        /// <summary>
        /// Returns the path to the view based on the controller and action
        /// i.e. controller=home, action=index then the view path returned would be ~/desktopmodules/modulename/views/controller/action.ascx
        /// </summary>
        /// <returns>A relative url ~/desktopmodules/modulename/views/controller/action.ascx</returns>
        public string ViewPath
        {
            get { return GetViewPath(Controller, Action); }
        }
        
    }

}
