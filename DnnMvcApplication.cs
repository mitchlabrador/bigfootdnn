using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using BigfootDNN.Helpers;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using System.Web.UI.HtmlControls;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;
using MailPriority = DotNetNuke.Services.Mail.MailPriority;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.UI.Modules;
using DotNetNuke.Instrumentation;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Security;

namespace BigfootDNN
{
    /// <summary>
    /// This is code derived from the opensource project BigfootMVC
    /// </summary>
    public partial class DnnMvcApplication
    {

        /// <summary>
        /// The route information used in a particular execution instance of a module
        /// </summary>
        public RouteInfo Route { get; set; }

        /// <summary>
        /// Carries a reference to the application information object which contains the module specific information (folder name, etc.)
        /// </summary>
        public AppInfo Info { get; set; }

        ///// <summary>
        ///// Holds the ability to make unique keys for caching, querystring parameter names, etc.
        ///// </summary>
        //public DnnAppParams Keys { get { return _keys ?? (_keys = new DnnAppParams(Route)); } }
        //private DnnAppParams _keys;

        public DnnMvcApplication(AppInfo appInfo, RouteInfo route)
        {
            Route = route;
            Info = appInfo;
            // Load the validation strings
            SetupLocalResources();
        }

        /// <summary>
        /// Provides the application with access to the DataProvider in order to run queries etc.
        /// </summary>
        public DataProvider DBConfig
        {
            get
            {
                var key = GetCacheKeyForHost("ProviderConfiguration");
                if (!SimpleCache.Contains(key))
                {
                    SimpleCache.Add(key, new DataProvider(Info.ModuleDBObjectQualifier));
                }
                return SimpleCache.GetValue<DataProvider>(key);
            }
        }

        /// <summary>
        /// Returns a SqlHelper class already configured to execute queries against the database
        /// </summary>
        public SqlHelper SQL
        {
            get
            {
                return new SqlHelper(DBConfig.DatabaseOwner,
                                     DBConfig.ObjectQualifier,
                                     DBConfig.ModuleQualifier,
                                     DBConfig.ConnectionString);
            }
        }
        
        #region Application Paths

        /// <summary>
        /// Retreives the view that is rendered with the error information
        /// </summary>
        public string ErrorViewPath { get { return AppPath + "error.cshtml"; } }

        /// <summary>
        /// Returns the path of the current portal
        /// </summary>
        public string PortalPath { get { return PortalSett.HomeDirectory; } }

        /// <summary>
        /// Returns the url data path to the current module within the current portal i.e. /portals/0/modulename/
        /// </summary>
        private string PortalDataPath
        {
            get
            {
                var format = PortalPath.EndsWith("/") ? "{0}{1}/" : "{0}/{1}/";
                return string.Format(format, PortalPath, Info.ModuleFolderName);
            }
        }

        /// <summary>
        /// Returns the physical data path to the current module within the current portal i.e. v:\wwwroot\dotnetnuke\portals\0\modulename\
        /// </summary>
        public string GetPortalDataPath()
        {
            var path = PortalDataPath;
            if (!_portalDataPathVerified && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                _portalDataPathVerified = true;
            }
            return path;
        }
        bool _portalDataPathVerified = false;

        /// <summary>
        /// Returns the physical data path to the module directory
        /// </summary>
        /// <returns>Returns the physical data path to the module directory</returns>
        public string GetPortalDataPhysicalPath()
        {
            var dataDir = GetPortalDataPath();
            return GetPhysicalPath(dataDir);
        }

        /// <summary>
        /// This is the root virtual path of where your applications files reside. This is the point from where 
        /// all content paths start. Example if you have all the content folder in ~/app/content/ then
        /// the AppPath would be ~/app/ . Always includes the last forward slash
        /// </summary>
        public string AppPath
        {
            get { return (string.IsNullOrEmpty(_appPath)) ? "~/DesktopModules/" + Info.ModuleFolderName + "/" : _appPath; }
            set { _appPath = value; }
        }
        private string _appPath;

        /// <summary>
        /// This is the data path of your applcation. This is the Data folder inside your application root
        /// </summary>
        public string DataPath
        {
            get { return string.IsNullOrEmpty(_dataPath) ? AppPath + "Data/" : _dataPath; }
            set { _dataPath = value; }
        }
        private string _dataPath;

        /// <summary>
        /// This is the path where the views of your applcation reside. They are in the Views folder inside your application root
        /// </summary>
        public string ViewsPath
        {
            get { return string.IsNullOrEmpty(_viewsPath) ? AppPath + "Views/" : _viewsPath; }
            set { _viewsPath = value; }
        }
        private string _viewsPath;

        /// <summary>
        /// This is the path where the content of your applcation reside like images, css, javascript, flash, and all other static files. 
        /// They are in the content folder folder inside your application root
        /// </summary>
        public string ContentPath
        {
            get { return string.IsNullOrEmpty(_contentPath) ? AppPath + "content/" : _contentPath; }
            set { _contentPath = value; }
        }
        private string _contentPath;

        /// <summary>
        /// This is the path to all your javascript files. AppPath/ContentPath/js/
        /// </summary>
        public string ScriptsPath
        {
            get { return string.IsNullOrEmpty(_scriptsPath) ? AppPath + "content/js/" : _scriptsPath; }
            set { _scriptsPath = value; }
        }
        private string _scriptsPath;

        /// <summary>
        /// This is the path to all your image files. AppPath/ContentPath/images/
        /// </summary>
        public string ImagesPath
        {
            get { return string.IsNullOrEmpty(_imagesPath) ? AppPath + "content/images/" : _imagesPath; }
            set { _imagesPath = value; }
        }
        private string _imagesPath;

        /// <summary>
        /// This is the path to all your CSS files. AppPath/ContentPath/css/
        /// </summary>
        public string CssPath
        {
            get { return string.IsNullOrEmpty(_cssPath) ? AppPath + "content/css/" : _cssPath; }
            set { _cssPath = value; }
        }
        private string _cssPath;

        /// <summary>
        /// This is the path to all your flash files. AppPath/ContentPath/FlashPath/
        /// </summary>
        public string FlashPath
        {
            get { return string.IsNullOrEmpty(_flashPath) ? AppPath + "content/flash/" : _flashPath; }
            set { _flashPath = value; }
        }
        private string _flashPath;

        /// <summary>
        /// This is the path to all your audio files. AppPath/ContentPath/AudioPath/
        /// </summary>
        public string AudioPath
        {
            get { return string.IsNullOrEmpty(_audioPath) ? AppPath + "content/audio/" : _audioPath; }
            set { _audioPath = value; }
        }
        private string _audioPath;

        #endregion


        #region PortalSettings

        /// <summary>
        /// Retreives the portal settings for the DNN app
        /// </summary>
        public PortalSettings PortalSett { get { return PortalController.GetCurrentPortalSettings(); } }

        #endregion


        #region LoginUrl and HomeUrl

        /// <summary>
        /// This the login url properly configured to return to the home page
        /// </summary>
        public string LoginUrl
        {
            get
            {
                var url = FormsAuthentication.LoginUrl;
                url += url.Contains("?") ? "&" : "?";
                url += "returnurl=" + DotNetNuke.Common.Globals.NavigateURL();
                return url;
            }
        }

        /// <summary>
        /// The url for the portal's home page
        /// </summary>
        public string HomeUrl { get { return DotNetNuke.Common.Globals.NavigateURL(); } }

        #endregion


        #region Urls 


        #region Urls: Base

        /// <summary>
        /// Gets a browser usable path to a MVC controller action.
        /// </summary>
        /// <param name="controller">The name of the controller that contains the action</param>
        /// <param name="action">The name of the action within the controller</param>
        /// <param name="moduleId">The module id to include in the url route</param>
        /// <param name="querystring">Appends this value to the querystring </param>
        /// <param name="fullUrl">Determines weather to append the domain name to the url</param>
        /// <param name="portalId">The portal id to include in the url route</param>
        /// <param name="tabId">The tab id to include in the url route</param>
        /// <returns>A url that may be used from the browser</returns>
        public string MvcUrl(string controller, string action, int portalId, int tabId, int moduleId, string querystring, bool fullUrl)
        {
            var relurl = DotNetNuke.Common.Globals.NavigateURL(tabId, "", RouteInfo.BuildUrlRoute(false, controller, action, portalId, tabId, moduleId, Info.ModuleShortName, querystring).ToArray());
            if (fullUrl && relurl.StartsWith("http") == false)
            {
                relurl = GetAbsoluteUrl(relurl, true);
            }
            return relurl;
        }

        /// <summary>
        /// Gets a browser usable path to a MVC controller action for AJAX requests. It activates the ajax route handler.
        /// </summary>
        /// <param name="controller">The name of the controller that contains the action</param>
        /// <param name="action">The name of the action within the controller</param>
        /// <param name="moduleId">The module id to include in the url route</param>
        /// <param name="querystring">Appends this value to the querystring </param>
        /// <param name="portalId">The portal id to include in the url route</param>
        /// <param name="tabId">The tab id to include in the url route</param>
        /// <returns>A url that may be used from the browser to make an ajax request</returns>
        public string MvcAjaxUrl(string controller, string action, int portalId, int tabId, int moduleId, string querystring)
        {
            var qs = RouteInfo.BuildUrlRoute(true, controller, action, portalId, tabId, moduleId, Info.ModuleShortName, querystring);
            var relurl = AppPath + "route.ashx?" + String.Join("&", qs.ToArray());
            if (relurl.StartsWith("~/"))
                relurl = relurl.Substring(2);
            if (relurl.EndsWith("/"))
                relurl = relurl.Substring(0, relurl.Length - 1);
            return GetApplicationPath() + relurl;
        }
               

        /// <summary>
        /// This is a shortcut to DotNetNuke's NavigateUrl. It gets the url path to activate a certain DNN control within your module.
        /// </summary>
        /// <param name="moduleId">The ModuleID for the module containing the control</param>
        /// <param name="controlName">The control key within your module delcaration</param>
        /// <param name="queryString">Appends this value to the querystring </param>
        /// <returns>A url that may be used from the browser</returns>
        public string ControlUrl(int moduleId, string controlName, string queryString)
        {
            queryString += string.IsNullOrEmpty(queryString) ? "mid=" + moduleId : "&mid=" + moduleId;
            return DotNetNuke.Common.Globals.NavigateURL(controlName, queryString);
        }

        /// <summary>
        /// Gets a browser usable path to a content file within the system. It uses the extention of the file to determine the location of the static file
        /// within the content folder.
        /// i.e. "image.jpg" will return "/desktopmodules/modulename/content/images/image.jpg"
        /// i.e. "menu/left.jpg" will return "/desktopmodules/modulename/content/images/menu/left.jpg"
        /// i.e. "test.js" will return "/desktopmodules/modulename/content/js/test.js"
        /// </summary>
        /// <param name="fileName">The name of the file relative to the content folder i.e. left.jpg or menu/left.jpg</param>
        /// <returns>A url that may be used from the browser to access the static resource</returns>
        public string StaticUrl(string fileName)
        {
            return StaticUrl(fileName, false);
        }

        /// <summary>
        /// Gets a browser usable path to a content file within the system. It uses the extention of the file to determine the location of the static file
        /// within the content folder.
        /// i.e. "image.jpg" will return "/desktopmodules/modulename/content/images/image.jpg"
        /// i.e. "menu/left.jpg" will return "/desktopmodules/modulename/content/images/menu/left.jpg"
        /// i.e. "test.js" will return "/desktopmodules/modulename/content/js/test.js"
        /// i.e. "~/Test/test.js" will return /desktopmodules/modulename/Test/test.js"
        /// </summary>
        /// <param name="fileName">The name of the file relative to the content folder i.e. left.jpg or menu/left.jpg</param>
        /// <param name="includeDomain">Determines weather to include the domain name in the url. (i.e. http://domain.com/desktopmodules/modulename/content/image.jpg)</param>
        /// <returns>A url that may be used from the browser</returns>
        public string StaticUrl(string fileName, bool includeDomain)
        {
            var relUrl = GetRelativeUrl(fileName);

            if (fileName.StartsWith("~/"))
                relUrl = AppPath + fileName.Substring(2);
            else if (fileName.StartsWith("/~/")) // This notation is used to signify the DNN application root
                relUrl = fileName.Substring(1);

            return GetAbsoluteUrl(relUrl, includeDomain);
        }

        /// <summary>
        /// Adds the authentication token to a url. Provided that you are doing cookieless authentication
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>Returns the modified url</returns>
        public string AddAuthTokenToUrl(string url)
        {
            return Response.ApplyAppPathModifier(url);
        }

        /// <summary>
        /// Retreives the relative url of a file based on its extention
        /// </summary>
        /// <param name="filename">filename: test.jpg returns ~/content/images/test.jpg</param>
        /// <returns>The server side relative url to the file</returns>
        public string GetRelativeUrl(string filename)
        {
            // Don't process files that already include the root element "~/" or the /~/
            if (filename.StartsWith("~/") || filename.StartsWith("/~/")) return filename;

            filename = filename.ToLower();
            if (filename.EndsWith("js"))
                return ScriptsPath + filename;
            else if (filename.EndsWith("css"))
                return CssPath + filename;
            else if (filename.EndsWith("jpg") || filename.EndsWith("gif") || filename.EndsWith("png"))
                return ImagesPath + filename;
            else if (filename.EndsWith("flv") || filename.EndsWith("swf"))
                return FlashPath + filename;
            else
                return filename;
        }

        /// <summary>
        /// Returns the server root application path. /AppLocation/ or / when at the root of the domain. This is the path to the application after the domain name
        /// </summary>
        /// <returns>Returns / or /appname/ or /apps/appname/ etc. What is after the domain name</returns>
        public string GetApplicationPath() { return GetApplicationPath(false); }

        /// <summary>
        /// Root application path. /app/ or / when at the root of the domain. 
        /// </summary>
        /// <param name="includeDomain">Determines weather the domain name is included</param>
        /// <returns>Returns the full application path root including the domain name if asked.</returns>
        public string GetApplicationPath(bool includeDomain)
        {
            var apppath = Request.ApplicationPath;
            if (apppath.EndsWith("/") == false) apppath += "/";

            if (includeDomain)
            {
                var host = GetApplicationHost;
                // Remove the host last "/"
                host = host.Substring(0, host.Length - 1);
                // Combine the application path and the host
                apppath = host + apppath;
            }
            return apppath;
        }


        /// <summary>
        /// Gets the application host name of the current request. Returns something like this http://applicationhost.com:4454 if it is port 80 then
        /// the port number is not used
        /// </summary>
        public string GetApplicationHost
        {
            get
            {
                var port = Request.ServerVariables["SERVER_PORT"];
                if (String.IsNullOrEmpty(port) || port == "80" || port == "443")
                    port = "";
                else
                    port = ":" + port;

                var protocol = Request.ServerVariables["SERVER_PORT_SECURE"];
                if (string.IsNullOrEmpty(protocol) || protocol == "0")
                    protocol = "http://";
                else
                    protocol = "https://";

                var domain = Request.ServerVariables["SERVER_NAME"];

                return protocol + domain + port + "/";
            }
        }

        /// <summary>
        /// Returns the physical path to a relative url. 
        /// For example url: ~/content/media/xyz.ppt returns c:\inetpub\wwwroot\appdomain.com\internal\content\media\xyz.ppt
        /// </summary>
        /// <param name="url">The relative application path i.e. ~/content/media/xyz.ppt</param>
        /// <returns>Full file system path i.e. c:\inetpub\wwwroot\appdomain.com\internal\content\media\xyz.ppt</returns>
        public string GetPhysicalPath(string url)
        {
            return Context.Server.MapPath(url);
        }

        /// <summary>
        /// Returns the absolute path to a url in the appliation. 
        /// For example url: ~/content/media/xyz.ppt returns /applicationpath/content/media/xyz.ppt
        /// </summary>
        /// <param name="url">The relative application path i.e. ~/content/media/xyz.ppt</param>
        /// <returns>Returns abosolute path to the url without the domain name. i.e. /applicationpath/content/media/xyz.ppt</returns>
        public string GetAbsoluteUrl(string url) { return GetAbsoluteUrl(url, false, false); }

        /// <summary>
        /// Returns the absolute path to a url in the appliation. 
        /// For example url: ~/content/media/xyz.ppt returns /applicationpath/content/media/xyz.ppt or http://applicationhsot/applicationpath/content/media/xyz.ppt
        /// </summary>
        /// <param name="url">The relative application path i.e. ~/content/media/xyz.ppt</param>
        /// <param name="includeFullPath">Determines weather to include the domain name</param>
        /// <returns>Returns abosolute path to the url without the domain name. i.e. /applicationpath/content/media/xyz.ppt
        /// or http://applicationhsot/applicationpath/content/media/xyz.ppt</returns>
        public string GetAbsoluteUrl(string url, bool includeFullPath)
        {
            return GetAbsoluteUrl(url, includeFullPath, false);
        }

        /// <summary>
        /// Returns the absolute path to a url in the appliation.  Optionally adds the url authentication token to the url
        /// For example url: ~/content/media/xyz.ppt returns /applicationpath/content/media/xyz.ppt or http://applicationhsot/applicationpath/content/media/xyz.ppt
        /// </summary>
        /// <param name="url">The relative application path i.e. ~/content/media/xyz.ppt</param>
        /// <param name="includeFullPath">Determines weather to include the domain name</param>
        /// <param name="addUrlAuthToken">Determines whether to add the url authentication token to the path</param>
        /// <returns>Returns abosolute path to the url without the domain name. i.e. /applicationpath/content/media/xyz.ppt
        /// or http://applicationhsot/applicationpath/content/media/xyz.ppt</returns>
        public string GetAbsoluteUrl(string url, bool includeFullPath, bool addUrlAuthToken)
        {
            var apppath = GetApplicationPath(includeFullPath);

            if (url.StartsWith("~/")) url = url.Substring(2);
            if (url.StartsWith("/")) url = url.Substring(1);

            url = apppath + url;

            if (addUrlAuthToken) url = AddAuthTokenToUrl(url);

            return url;
        }
                
        #endregion


        
        

        #region Urls: Alias

        /// <summary>
        /// Gets a browser usable path to a MVC controller action.
        /// </summary>
        /// <param name="controller">The name of the controller that contains the action</param>
        /// <param name="action">The name of the action within the controller</param>
        /// <returns>A url that may be used from the browser to access the static resource</returns>
        public string MvcUrl(string controller, string action) { return MvcUrl(controller, action, Route.PortalId, Route.TabId, Route.ModuleId, string.Empty, false); }

        /// <summary>
        /// Gets a browser usable path to a MVC controller action.
        /// </summary>
        /// <param name="controller">The name of the controller that contains the action</param>
        /// <param name="action">The name of the action within the controller</param>
        /// <param name="querystring">Appends this value to the querystring </param>
        /// <returns>A url that may be used from the browser</returns>
        public string MvcUrl(string controller, string action, string querystring) { return MvcUrl(controller, action, Route.PortalId, Route.TabId, Route.ModuleId, querystring, false); }

        /// <summary>
        /// Gets a browser usable path to a MVC controller action.
        /// </summary>
        /// <param name="controller">The name of the controller that contains the action</param>
        /// <param name="action">The name of the action within the controller</param>
        /// <param name="querystring">Appends this value to the querystring </param>
        /// <param name="fullUrl">Determines weather to append the domain name to the url</param>
        /// <returns>A url that may be used from the browser</returns>
        public string MvcUrl(string controller, string action, string querystring, bool fullUrl) { return MvcUrl(controller, action, Route.PortalId, Route.TabId, Route.ModuleId, querystring, fullUrl); }

        /// <summary>
        /// Gets a browser usable path to a MVC controller action for AJAX requests. It activates the ajax route handler.
        /// </summary>
        /// <param name="controller">The name of the controller that contains the action</param>
        /// <param name="action">The name of the action within the controller</param>
        /// <returns>A url that may be used from the browser to make an ajax request</returns>
        public string MvcAjaxUrl(string controller, string action) { return MvcAjaxUrl(controller, action, Route.PortalId, Route.TabId, Route.ModuleId, string.Empty); }

        /// <summary>
        /// Gets a browser usable path to a MVC controller action for AJAX requests. It activates the ajax route handler.
        /// </summary>
        /// <param name="controller">The name of the controller that contains the action</param>
        /// <param name="action">The name of the action within the controller</param>
        /// <param name="querystring">Appends this value to the querystring </param>
        /// <returns>A url that may be used from the browser to make an ajax request</returns>
        public string MvcAjaxUrl(string controller, string action, string querystring) { return MvcAjaxUrl(controller, action, Route.PortalId, Route.TabId, Route.ModuleId, querystring); }


        /// <summary>
        /// This is a shortcut to DotNetNuke's NavigateUrl. It gets the url path to activate a certain DNN control within your module.
        /// </summary>
        /// <param name="controlName">The control key within your module delcaration</param>
        /// <returns>A url that may be used from the browser</returns>
        public string ControlUrl(string controlName) { return ControlUrl(Route.ModuleId, controlName, string.Empty); }

        /// <summary>
        /// This is a shortcut to DotNetNuke's NavigateUrl. It gets the url path to activate a certain DNN control within your module.
        /// </summary>
        /// <param name="controlName">The control key within your module delcaration</param>
        /// /// <param name="querystring">Appends this value to the querystring </param>
        /// <returns>A url that may be used from the browser</returns>
        public string ControlUrl(string controlName, string querystring) { return ControlUrl(Route.ModuleId, controlName, querystring); } 

        #endregion
                

        #endregion


        #region Phisical File Paths: CreateFilePath | GetFilePath | GetFileNameForDisplay
        // Creates a valid file path and name and returns it
        //   Returns: The file path and name e.g. c:\inetpub\wwwroot\portals\0\ModuleName\file1.xls
        public string CreateFilePath(string FileName)
        {
            string name = Path.GetFileNameWithoutExtension(FileName);
            string ext = Path.GetExtension(FileName);

            // Add the dot to the ext
            if (string.IsNullOrEmpty(ext) == false && ext.StartsWith(".") == false)
            {
                ext = "." + ext;
            }
            else if (string.IsNullOrEmpty(ext))
            {
                ext = "";
            }

            // Add the .config to make it impossible to download directly
            ext += ".config";

            // Clean up the file name
            foreach (var c_loopVariable in Path.GetInvalidFileNameChars())
            {
                var c = c_loopVariable;
                name = name.Replace(c, '_');
            }

            // Get a unique filename
            int nameCount = 1;
            var uname = name;
            while (File.Exists(GetPortalDataPhysicalPath() + uname + ext))
            {
                uname = name + nameCount.ToString();
                nameCount += 1;
            }

            // Return the full file name
            return GetPortalDataPhysicalPath() + uname + ext;
        }

        // Gets the path used a file name and note
        //   Returns: The file path and name e.g. c:\inetpub\wwwroot\portals\0\ModuleName\file1.xls</returns>
        public string GetFilePath(string FileName)
        {
            return GetPortalDataPhysicalPath() + FileName;
        }

        public string GetFileNameForDisplay(string filename)
        {
            // Remove the .config
            if (filename.ToLower().EndsWith(".config"))
            {
                filename = filename.Substring(0, filename.Length - 7);
            }

            // Truncate files greater than 50 chars
            if (filename.Length > 50)
            {
                filename = filename.Substring(0, 50) + "...";
            }

            return filename;
        }

        #endregion


        #region HandleException

        /// <summary>
        /// Handle an exception. It logs it to DNN and knows how to handle security exceptions on ajax requests etc.
        /// </summary>
        /// <param name="ex">The exception to handle</param>
        public void HandleException(Exception ex)
        {
            HandleException(ex, true);
        }

        /// <summary>
        /// Handle an exception. It logs it to DNN and knows how to handle security exceptions on ajax requests etc.
        /// </summary>
        /// <param name="ex">The exception to handle</param>
        /// <param name="ReThrow">Determines weather to rethrow the exception after the loging actions etc. have been executed</param>
        public void HandleException(Exception ex, bool ReThrow)
        {
            // Ignore exceptions due to response manually ended
            if (ResponseEndCalled) { return; }

            // Log the exception in DNN
            Exceptions.LogException(ex);

            // When an ajax request, signal to the client that the request failed.
            if (IsAjaxRequest)
            {
                Response.Clear();

                RenderErrorViewToResponse(ex);

                // Set the status code to 402 for security problems. 
                //  This is needed in ajax request in order to allow the javascript to respond by redirecting the user
                //  from the browser. Otherwise the asp.net authentication module will do a redirect and render back to the
                //  ajax call the login page, which would break your design since you might be injecting whatever 
                //  comes back into an element in the existing page. BigfootMVC Javascript sees the error code 402 and it
                //  knows that it needs to do a redirect client side to the login page.
                if ((ex.GetType().Name == typeof(System.Security.SecurityException).Name) ||
                    (ex.InnerException != null && object.ReferenceEquals(ex.InnerException.GetType().Name, typeof(System.Security.SecurityException).Name)))
                {
                    Response.StatusCode = 402;
                }
                else
                {
                    Response.StatusCode = 500;
                }

                Response.StatusDescription = ex.Message;
                Response.End();
            }
            else if (ReThrow)
            {
                throw ex;
            }
        }

        public string RenderErrorViewToString(Exception ex, bool isPartialRender = false)
        {
            var errorViewModel = ErrorViewModel.Create(ex, Route.App.Request.Url.ToString());
            return RenderRazorViewToString(ErrorViewPath, errorViewModel, "", isPartialRender);
        }

        public void RenderErrorViewToResponse(Exception ex, bool isPartialRender = false)
        {
            var errorViewModel = ErrorViewModel.Create(ex, Route.App.Request.Url.ToString());
            RenderRazorViewToResponse(ErrorViewPath, errorViewModel, "", isPartialRender);
        }

        #endregion


        #region RequestJQueryRegistration InjectjQueryLibrary

        /// <summary>
        /// Requests the jQuery library
        /// </summary>
        public void RequestJQueryRegistration()
        {
            DotNetNuke.Framework.jQuery.RequestUIRegistration();
        }

        /// <summary>
        /// Requests the jQuery UI library
        /// </summary>
        public void RequestJQueryUIRegistration() 
        {
            DotNetNuke.Framework.jQuery.RequestRegistration();
        }

        /// <summary>
        /// Injects the jquery and jqueryui library
        /// </summary>
        /// <param name="page">The current page</param>
        /// <param name="includejQueryUI">Wheather to include the jquery library</param>
        /// <param name="route">The current route to use to ge the url to the javascript files</param>
        /// <param name="jqueryUrl">The url to the debug version of jQuery</param>
        /// <param name="jqueryMinUrl">The url to the minified version of jQuery</param>
        /// <param name="jqueryUIUrl">The url to the debug version of jQueryUI</param>
        /// <param name="jqueryUIMinUrl">The url to the minified version of jQueryIO</param>
        public void InjectjQueryLibary(System.Web.UI.Page page, bool includejQueryUI, string jqueryUrl, string jqueryMinUrl, string jqueryUIUrl, string jqueryUIMinUrl)
        {

            var uncompressed = IsInDebugMode;
            int major, minor, build, revision;
            var injectjQueryLib = false;
            var injectjQueryUiLib = false;
            if (SafeDNNVersion(out major, out minor, out revision, out build))
            {
                switch (major)
                {
                    case 4:
                        injectjQueryLib = true;
                        injectjQueryUiLib = true;
                        break;
                    case 5:
                        injectjQueryLib = false;
                        injectjQueryUiLib = true;
                        break;
                    default://6.0 and above
                        injectjQueryLib = false;
                        injectjQueryUiLib = false;
                        break;
                }
            }
            else
                injectjQueryLib = true;

            if (injectjQueryLib)
            {
                var lib = uncompressed ? jqueryUrl : jqueryMinUrl;
                IncludeScriptFile(page, "jquery", lib);
            }
            else
            {
                //call DotNetNuke.Framework.jQuery.RequestRegistration();
                Type jQueryType = Type.GetType("DotNetNuke.Framework.jQuery, DotNetNuke");
                if (jQueryType != null)
                {
                    //run the DNN 5.0 specific jQuery registration code
                    jQueryType.InvokeMember("RequestRegistration", System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, jQueryType, null);
                }
            }

            //include the UI libraries??
            if (includejQueryUI)
            {
                if (injectjQueryUiLib)
                {
                    string lib = null;
                    lib = uncompressed ? jqueryUIUrl : jqueryUIMinUrl;
                    page.ClientScript.RegisterClientScriptInclude("jqueryUI", lib);
                }
                else
                {
                    //use late bound call to request registration of jquery
                    Type jQueryType = Type.GetType("DotNetNuke.Framework.jQuery, DotNetNuke");
                    if (jQueryType != null)
                    {
                        //dnn 6.0 and later, allow jquery ui to be loaded from the settings.
                        jQueryType.InvokeMember("RequestUIRegistration", System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, jQueryType, null);

                    }
                }
            }
        } 
        #endregion


        #region SafeDNNVersion
        public bool SafeDNNVersion(out int major, out int minor, out int revision, out int build)
        {
            var ver = System.Reflection.Assembly.GetAssembly(typeof(DotNetNuke.Common.Globals)).GetName().Version;
            if (ver != null)
            {
                major = ver.Major;
                minor = ver.Minor;
                build = ver.Build;
                revision = ver.Revision;
                return true;
            }
            else
            {
                major = 0; minor = 0; build = 0; revision = 0;
                return false;
            }
        } 
        #endregion


        #region IncludeScriptFile & IncludeCSSFile

        /// <summary>
        /// Helper function to insert a given javascript file into the page header
        /// </summary>
        /// <param name="page">Current Page object</param>
        /// <param name="id">Unique (for the page) id of the file</param>
        /// <param name="src">Qualified location of the script file</param>
        public void IncludeScriptFile(Page page, string id, string src)
        {
            var scriptInclude = (HtmlGenericControl)page.Header.FindControl(id);
            if (scriptInclude != null) return;
            scriptInclude = new HtmlGenericControl("script");
            scriptInclude.Attributes.Add("src", src);
            scriptInclude.Attributes.Add("type", "text/javascript");
            scriptInclude.ID = id;
            page.Header.Controls.Add(scriptInclude);
        }
        /// <summary>
        /// Helper function to insert a given CSS file into the page header
        /// </summary>
        /// <param name="page">Current Page Object</param>
        /// <param name="id">Unique (for the page) id of the file</param>
        /// <param name="href">Qualified location of the CSS file</param>
        public void IncludeCSSFile(Page page, string id, string href)
        {
            var cssLink = (HtmlLink)page.Header.FindControl(id);
            if (cssLink != null) return;
            cssLink = new HtmlLink();
            cssLink.Href = href;
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
            cssLink.ID = id;
            page.Header.Controls.Add(cssLink);
        } 
        #endregion

        
        #region Request | Response | Context | Server

        /// <summary>
        /// Access to the HttpRequest contained within the ContextProvider. Aliased here for convinience
        /// </summary>
        public HttpRequestBase Request { get { return Context.Request; } }

        /// <summary>
        /// Access to the HttpResponse contained within the ContextProvider. Aliased here for convinience
        /// </summary>
        public HttpResponseBase Response { get { return Context.Response; } }

        /// <summary>
        /// Access to the HttpContext contained within the ContextProvider. Aliased here for convinience
        /// </summary>
        public HttpContextBase Context { get { return new HttpContextWrapper(HttpContext.Current); } }

        /// <summary>
        /// Access to the HttpServerUtility contained within the ContextProvider. Aliased here for convinience
        /// </summary>
        public HttpServerUtilityBase Server { get { return Context.Server; } }

        /// <summary>
        /// The name of the currently logged in principle
        /// </summary>
        public string UserName { get { return (Request.IsAuthenticated) ? Context.User.Identity.Name : ""; } } 

        #endregion

        
        #region InTestingMode | IsInDebugMode | IsAjaxRequest | IsAuthenticated
        /// <summary>
        /// Flag that determines that the application is in testing mode
        /// </summary>
        public bool InTestingMode { get; set; }


        /// <summary>
        /// Determines weather we are in debug mode at compile time
        /// </summary>
        public bool IsInDebugMode
        {
            get
            {
#if (DEBUG)
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Determines weather the current request is an ajax request
        /// </summary>
        public bool IsAjaxRequest
        {
            get
            {
                if (InTestingMode) return false;
                return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
        }

        /// <summary>
        /// Determines if the current user is authenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get { return Request.IsAuthenticated; }
        }

        #endregion

        
        #region Html | JS Helpers
        /// <summary>
        /// Gets a reference to the HTML Helper
        /// </summary>
        public HtmlHelper Html
        {
            get { return _html ?? (_html = HtmlHelper.Create()); }
        }
        private HtmlHelper _html;

        /// <summary>
        /// Gets a reference to the JS Helper
        /// </summary>
        public JSHelper JS
        {
            get { return _js ?? (_js = JSHelper.Create()); }
        }
        private JSHelper _js; 
        #endregion


        #region "Encryption"

        private PortalSecurity _portalSecurity;
        public PortalSecurity GetPortalSecurity()
        {
            return _portalSecurity ?? (_portalSecurity = new PortalSecurity());
        }

        /// <summary>
        /// Encrypts a value with the specified key using DES encryption
        /// </summary>
        /// <param name="key">The key to use in the encryption</param>
        /// <param name="data">The data to encrypt</param>
        /// <returns>Base64 encrypted string</returns>
        public string Encrypt(string key, string data)
        {
            return GetPortalSecurity().Encrypt(key, data);
            //// don't do anything if the key is empty
            //if (string.IsNullOrEmpty(key)) return data;

            //// normalize the key to 16 characters
            //if (key.Length < 16)
            //    key = key + "XXXXXXXXXXXXXXXX".Substring(0, 16 - key.Length);
            //else
            //    key = key.Substring(0, 16);

            //// create encryption key
            //var bytekey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            //var bytevector = Encoding.UTF8.GetBytes(key.Substring(8));

            //// Convert the data
            //var bytedata = Encoding.UTF8.GetBytes(data);

            //// Encrypt
            //var des = new DESCryptoServiceProvider();
            //var memstream = new MemoryStream();
            //var cryptostream = new CryptoStream(memstream, des.CreateEncryptor(bytekey, bytevector),
            //                                    CryptoStreamMode.Write);
            //cryptostream.Write(bytedata, 0, bytedata.Length);
            //cryptostream.FlushFinalBlock();

            //// covert to string and base64 encode
            //return Convert.ToBase64String(memstream.ToArray());
        }

        /// <summary>
        /// Decrypts a value with the specified key using DES encryption
        /// </summary>
        /// <param name="key">The key to use in the decryption</param>
        /// <param name="data">The data to decrypt base64 encoded</param>
        /// <returns>Decripted string</returns>
        public string Decrypt(string key, string data)
        {
            return GetPortalSecurity().Decrypt(key, data);
            //// don't do anything if the key is empty
            //if (string.IsNullOrEmpty(key)) return data;

            //// normalize the key to 16 characters
            //if (key.Length < 16)
            //    key = key + "XXXXXXXXXXXXXXXX".Substring(0, 16 - key.Length);
            //else
            //    key = key.Substring(0, 16);

            //// create encryption key
            //var bytekey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            //var bytevector = Encoding.UTF8.GetBytes(key.Substring(8));

            //byte[] bytedata;
            //try
            //{
            //    bytedata = Convert.FromBase64String(data);
            //}
            //// return the data unchanged if there is some sort of error converting
            //catch (Exception)
            //{
            //    return data;
            //}

            //// Run the decryption
            //try
            //{
            //    var des = new DESCryptoServiceProvider();
            //    var ms = new MemoryStream();
            //    var cs = new CryptoStream(ms, des.CreateDecryptor(bytekey, bytevector), CryptoStreamMode.Write);
            //    cs.Write(bytedata, 0, bytedata.Length);
            //    cs.FlushFinalBlock();

            //    return Encoding.UTF8.GetString(ms.ToArray());
            //}
            //catch (Exception)
            //{
            //    return "";
            //}
        }

        #endregion


        #region "Values"

        /// <summary>
        /// Class used to aid in the collection of posted values from the request. First it tries the QueryString and second it tries the form collection
        /// </summary>
        public PostHelper Values
        {
            get
            {
                var key = Route.App.GetCacheKeyForHost("poster_values");
                if (!ContextHelper.HasData(key))
                    ContextHelper.SetData(key, new PostHelper(Context, null, InTestingMode));

                return ContextHelper.GetData<PostHelper>(key);
            }
        }

        #endregion


        #region "Email"

        /// <summary>
        /// Sends an email using the email provider
        /// </summary>
        /// <param name="from">Who is the email from</param>
        /// <param name="to">Who is it going to. Can include multiple address separated by comma</param>
        /// <param name="subject">The subject line</param>
        /// <param name="body">The body of the html</param>
        /// <param name="isHtml">True if the body is using HTML</param>
        public void SendEmail(string from, string to, string subject, string body, bool isHtml)
        {
            var msg = new MailMessage(from, to, subject, body) { IsBodyHtml = isHtml };
            SendEmail(msg);
        }

        public void SendEmail(MailMessage msg)
        {
            SendEmail(msg, "");
        }

        public void SendEmail(MailMessage msg, string attachment)
        {
            var priority = msg.Priority == System.Net.Mail.MailPriority.High
                                ? MailPriority.High
                                : (msg.Priority == System.Net.Mail.MailPriority.Low
                                        ? MailPriority.Low
                                        : MailPriority.Normal);
            var from = msg.From.Address;
            var to = msg.To.ToString();
            var cc = msg.CC.ToString();
            var bcc = msg.Bcc.ToString();
            var subject = msg.Subject;
            var bodyFormat = msg.IsBodyHtml ? MailFormat.Html : MailFormat.Text;
            var encoding = msg.BodyEncoding;
            var body = msg.Body;

            //var hostSettings = HostController.Instance.GetSettingsDictionary();

            //var SMTPServer = Convert.ToString(hostSettings["SMTPServer"]);
            //var SMTPAuthentication = Convert.ToString(hostSettings["SMTPAuthentication"]);
            //var SMTPUsername = Convert.ToString(hostSettings["SMTPUsername"]);
            //var SMTPPassword = Convert.ToString(hostSettings["SMTPPassword"]);
            //var SMTPEnableSSL = hostSettings["SMTPEnableSSL"].ToString().ToLowerInvariant() == "y";

            Mail.SendMail(from, to, cc, bcc, priority, subject, bodyFormat, encoding, body, attachment, "", "", "", "");
        }

        #endregion

        
        #region ResponseEndCalled
        /// <summary>
        /// This flags indicates that we have implicitly called response.end and it does not log the exception
        /// thrown by premature response termination
        /// </summary>
        public bool ResponseEndCalled
        {
            get
            {
                return ContextHelper.GetBool("ResponseEndCalled");
            }
            set
            {
                ContextHelper.SetData("ResponseEndCalled", value);
            }
        } 
        #endregion


        #region "Render"

        public void RenderRazorViewToResponse(string scriptPath, object data = null, string localResourceFile = "", bool isRenderPartial = false)
        {
            //var razorEngine = new RazorEngine(scriptPath, Route, localResourceFile);
            //razorEngine.Render(Response.Output, data, isRenderPartial);
            var razorEngine = new RazorEngine(scriptPath, Route, localResourceFile);
            var writer = new StringWriter();
            razorEngine.Render(writer, data, isRenderPartial);
            Response.Output.Write(HttpUtility.HtmlDecode(writer.ToString()));
        }
                        
        public string RenderRazorViewToString(string scriptPath, object data = null, string localResourceFile = "", bool isRenderPartial = false)
        {   
            var razorEngine = new RazorEngine(scriptPath, Route, localResourceFile);
            var writer = new StringWriter();
            razorEngine.Render(writer, data, isRenderPartial);           
            return HttpUtility.HtmlDecode(writer.ToString());
        }

        //public void RenderViewToResponse(string viewRelativePath, object data)
        //{
        //    RenderViewToResponse(viewRelativePath, data, null, null);
        //}
        
        //public void RenderViewToResponse(string viewRelativePath, object data, string loadField1Name, object loadField1Data)
        //{
        //    // Render the page to the output stream of the response provided
        //    Server.Execute(RenderViewToPage(viewRelativePath, data, loadField1Name, loadField1Data), Response.Output, true);
        //}

        //public string RenderViewToString(string viewRelativePath, object data)
        //{
        //    return RenderViewToString(viewRelativePath, data, null, null);
        //}

        //public string RenderViewToString(string viewRelativePath, object data, string loadField1Name, object loadField1Data)
        //{
        //    // Render the page
        //    var output = new StringWriter();

        //    // Render the page to the output stream
        //    Server.Execute(RenderViewToPage(viewRelativePath, data, loadField1Name, loadField1Data), output, true);

        //    // Return the rendered output
        //    return output.ToString();
        //}

        //private Page RenderViewToPage(string viewRelativePath, object data, string loadField1Name, object loadField1Data)
        //{
        //    // Load the control
        //    var pageHolder = new Page();
        //    var viewControl = (UserControl)pageHolder.LoadControl(viewRelativePath);

        //    // Get the type of the control
        //    var t = viewControl.GetType();

        //    // Set the model data
        //    SetField(viewControl, t, "ModelData", data);

        //    // Flag the render view as being partially rendered
        //    SetField(viewControl, t, "IsRenderPartial", true);

        //    // Add additional data if specified
        //    SetField(viewControl, t, loadField1Name, loadField1Data);

        //    // Load the control on the page
        //    pageHolder.Controls.Add(viewControl);

        //    return pageHolder;
        //}

        //private void SetField(object control, Type t, string fieldName, object data)
        //{
        //    // Add additional data if specified
        //    if (string.IsNullOrEmpty(fieldName) || data == null) return;
        //    var field = t.GetField(fieldName);
        //    if (field != null)
        //    {
        //        field.SetValue(control, data);
        //    }
        //    else // Check if it is a property
        //    {
        //        var prop = t.GetProperty(fieldName);
        //        if (prop != null)
        //            prop.SetValue(control, data, BindingFlags.SetProperty, null, null, null);
        //    }
        //}

        //public UserControl LoadView(string viewPath, object data)
        //{
        //    // Load the control
        //    var pageHolder = new Page();
        //    return LoadView(viewPath, data, pageHolder);
        //}

        //public UserControl LoadView(string viewPath, object data, Page currentPage)
        //{
        //    return LoadView(viewPath, data, currentPage, null, null);
        //}

        //public UserControl LoadView(string viewPath, object data, Page currentPage, string loadField1Name, object loadField1Data)
        //{
        //    var ctl = (UserControl)currentPage.LoadControl(viewPath);

        //    // Get the type of the control
        //    var t = ctl.GetType();

        //    // Set the model data
        //    SetField(ctl, t, "ModelData", data);

        //    // Flag the render view as being partially rendered
        //    SetField(ctl, t, "IsRenderPartial", true);

        //    // Add additional data if specified
        //    SetField(ctl, t, loadField1Name, loadField1Data);
        //    return (UserControl)ctl;
        //}

        public void RenderJson(string data)
        {
            Response.Clear();
            SetNoCache();
            Response.ContentType = "application/json";
            Response.Write(data);
            ResponseEndCalled = true;
            Response.End();
        }

        public void RenderLiteral(string data)
        {
            Response.Clear();
            SetNoCache();
            Response.Write(data);
            ResponseEndCalled = true;
            Response.End();
        }

        public void SetNoCache()
        {
            Response.Expires = 0;
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");
        }

        #endregion


        #region "REST Functions"

        public string PostValues(string url, NameValueCollection postdata)
        {
            var wc = new WebClient();
            ServicePointManager.Expect100Continue = false;
            var returnData = wc.UploadValues(url, "POST", postdata);
            return (new StreamReader(new MemoryStream(returnData))).ReadToEnd();
        }

        public XmlDocument PostValuesAndReturnXml(string url, NameValueCollection postData)
        {
            var doc = new XmlDocument();
            doc.LoadXml(PostValues(url, postData));
            return doc;
        }
        #endregion

        
        #region "GetLocalString | MVCStrings | SetupLocalResources"


        /// <summary>
        /// Gets a localized string for the current module
        /// </summary>
        /// <param name="key">The key to retrieve. DotNetNuke by default adds .Text at the end of any
        /// key that does not contain a "." dot in it. To avoid this behaviour structure your
        /// key names with a "." dot in them.
        /// </param>
        /// <param name="defaultValue">The default value to return if the requested value is not found</param>
        /// <returns>Returns null if not found otherwise the value</returns>
        public string GetLocalString(string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(key)) return defaultValue;
            var value = Localization.GetString(key, "~/DesktopModules/" + Info.ModuleFolderName + "/App_LocalResources/SharedResources.resx");
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
        /// Gets the instance of the MVCStrings being used in the MVC section of the app
        /// </summary>
        public DnnMvcStrings MVCStrings { get { return _mvcStrings ?? (_mvcStrings = new DnnMvcStrings(this)); } }
        private DnnMvcStrings _mvcStrings;

        /// <summary>
        /// This method should be called in order to configure the local resource strings
        /// </summary>
        public void SetupLocalResources()
        {
            MVCStrings.LoadValidationLanguage();
            //Values.Message_ExpectedValueNotFound = MVCStrings.PostValRequired;
        }

        #endregion


        #region Common Functions: uid | CleanInput | CleanInput_NoMarkupNoScript

        /// <summary>
        /// Creates a unique client id specific to this module instance on the page. This is useful when refering to element ids from javascript as
        /// you might have the same id if the module is added to the page several times. It simply adds an underscore plus the 
        /// module id to the name
        /// </summary>
        /// <param name="id">The id of the element you would like to make unique to the module</param>
        /// <returns>It returns {id}_{ModuleId}</returns>
        public string uid(string id)
        {
            return id + "_" + Route.ModuleId;
        }

        public string CleanInput_NoMarkupNoScript(string input)
        {
            return CleanInput(input, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
        }

        public string CleanInput(string input, PortalSecurity.FilterFlag filterOptions)
        {
            return GetPortalSecurity().InputFilter(input, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoSQL | PortalSecurity.FilterFlag.NoScripting);
        }

        #endregion


        #region Logging
        public void LogError(object message)
        {

            DnnLog.Error(message);
        }

        public void LogErrorFormat(string message, params string[] values)
        {
            DnnLog.Error(message, values);
        }

        public void LogError(string message, Exception ex)
        {
            DnnLog.Error(message, ex);
        }

        public void LogDebug(object message)
        {
            DnnLog.Debug(message);
        }

        public void LogDebug(string message, params string[] values)
        {
            DnnLog.Debug(message, values);
        }

        public void LogInfo(object message)
        {
            DnnLog.Info(message);
        }

        public void LogInfo(string message, params string[] values)
        {
            DnnLog.Info(message, values);
        }

        public void LogWarning(object message)
        {
            DnnLog.Warn(message);
        }

        public void LogWarning(string message, params string[] values)
        {
            DnnLog.Warn(message, values);
        }

        public void LogWarning(string message, Exception ex)
        {
            DnnLog.Warn(message, ex);
        }

        public void LogFatal(object message)
        {
            DnnLog.Fatal(message);
        }

        public void LogFatal(string message, params string[] values)
        {
            DnnLog.Fatal(message, values);
        }

        public void LogFatal(string message, Exception ex)
        {
            DnnLog.Fatal(message, ex);
        }
        #endregion


        #region DataCache (DNN Cache)

        /// <summary>
        /// Creates a unique parameter for your module that is unique for this DNN installation. Does not differentiate accross portals, or module instances.
        /// This is particularly useful for things like the database provider cache key etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <returns>Returns {key}_mn_{ModuleShortName}</returns>
        public string GetCacheKeyForHost(string key)
        {
            return key + "_mn_" + Info.ModuleShortName;
        }

        /// <summary>
        /// Creates a unique parameter for your module that is unique for this specific portal. Does not differentiate accross module instances within this portal.
        /// This is particularly useful for things like portal specific data caches etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <returns>Returns {key}_mn_{moduleshortname}_pid_{portalId}</returns>
        public string GetCacheKeyForPortal(string key)
        {
            return key + "_mn_" + Info.ModuleShortName + "_pid_" + Route.PortalId;
        }

        /// <summary>
        /// Creates a unique parameter for your module that is unique for this specific module instance
        /// This is particularly useful for things like module instance route etc.
        /// </summary>
        /// <param name="key">The parameter name to create</param>
        /// <returns>Returns {key}_pid_{portalId}</returns>
        public string GetCacheKeyForModule(string key)
        {
            return key + "_mid_" + Route.ModuleId;
        }

        /// <summary>
        /// Sets an item in the DNN cache. Replaces if already found
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="toSet">The object to set</param>
        /// <param name="key">The key to use</param>
        public static void SetItemInDnnCache<T>(T toSet, string key)
        {
            DataCache.SetCache(key, toSet);
        }

        /// <summary>
        /// Retreives an item from the DnnCache for the key specified
        /// </summary>
        public static T GetItemFromDnnCache<T>(string key)
        {
            return (T)DataCache.GetCache(key);
        }

        /// <summary>
        /// Determines whether a certain item exists in the DnnCache
        /// </summary>
        public static bool ItemExistsInDnnCache(string key)
        {
            return DataCache.GetCache(key) != null;
        }

        #endregion


        #region DnnFolders | DnnFiles

        public IFolderManager Folders { get { return FolderManager.Instance; } }

        public IFileManager Files { get { return FileManager.Instance; } }

        #endregion

    }

}
