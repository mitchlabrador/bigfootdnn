using System;
using System.IO;
using System.Security;
using BigfootDNN.Helpers;

using DotNetNuke.Entities.Modules;


namespace BigfootDNN
{

    /// <summary>
    /// This is the base controller class from which all MVC controller must inherit
    /// </summary>
    public partial class BaseController
    {
        
        /// <summary>
        /// Holds the ModuleInfo object for the current module
        /// </summary>
        public PortalModuleBase Module { get { return Route.Module; } }

        /// <summary>
        /// The current route being executed
        /// </summary>
        public RouteInfo Route { get; set; }

        /// <summary>
        /// Gets a reference to the Route.App object
        /// </summary>
        public DnnMvcApplication App { get { return Route.App; } }
        
        /// <summary>
        /// Alias to Route.PortalId
        /// </summary>
        public int PortalId { get { return Route.PortalId; } }

        /// <summary>
        /// Alias to Route.UserId
        /// </summary>
        public int UserId { get { return Route.UserId; } }

        /// <summary>
        /// Gets direct access to the request values
        /// </summary>
        public PostHelper Values { get { return Route.App.Values; } }

        /// <summary>
        /// Creates a unique client id specific to this module instance on the page. This is useful when refering to element ids from javascript as
        /// you might have the same id if the module is added to the page several times. It simply adds an underscore plus the 
        /// module id to the name
        /// </summary>
        /// <param name="id">The id of the element you would like to make unique to the module</param>
        /// <returns>It returns {id}_{ModuleId}</returns>
        public string uid(string id) { return Route.App.uid(id); }

        /// <summary>
        /// Returns a new ActionResult for the View that matches the current action
        /// </summary>
        /// <returns>An action result object to be consumed by the route handler</returns>
        public ActionResult ViewResult()
        {
            return ViewResult(Route.Controller, Route.Action, null);
        }

        /// <summary>
        /// Returns a new ActionResult for the View that matches the current action
        /// </summary>
        /// <param name="data">Passes the data to be used as the View's model</param>
        /// <returns>An action result object to be consumed by the route handler</returns>
        public ActionResult ViewResult(object data)
        {
            return ViewResult(Route.Controller, Route.Action, data);
        }

        /// <summary>
        /// Returns a new ActionResult for the View specified but within the current controller
        /// </summary>
        /// <param name="viewName">Name of the view to return within the contorller view folder</param>
        /// <returns>An action result object to be consumed by the route handler</returns>
        public ActionResult ViewResult(string viewName)
        {
            return ViewResult(Route.Controller, viewName, null);
        }


        /// <summary>
        /// Returns a new ActionResult for the View specified but within the current controller
        /// </summary>
        /// <param name="viewName">Name of the view to return within the current contorller view folder</param>
        /// <param name="data">Passes the data to be used as the View's model</param>
        /// <returns>An action result object to be consumed by the route handler</returns>
        public ActionResult ViewResult(string viewName, object data)
        {
            return ViewResult(Route.Controller, viewName, data);
        }

        /// <summary>
        /// Returns a new ActionResult for the action and controller specified
        /// </summary>
        /// <param name="Action">Name of the action within the controller</param>
        /// <param name="Controller">Name of the controller that contains the action</param>
        /// <param name="data">Passes the data to be used as the View's model</param>
        /// <returns>An action result object to be consumed by the route handler</returns>
        public ActionResult ViewResult(string Controller, string Action, object data)
        {
            Route.Controller = Controller;
            Route.Action = Action;
            return new ActionResult(ActionResult.ActionTypeEnum.View, Route, data);
        }

        /// <summary>
        /// Returns a new ActionResult with json data to be returned to the client
        /// </summary>
        /// <param name="jsondata">JSON formatted string to return to client</param>
        /// <returns>An ActionResult object to be consumed by the route handler</returns>
        public ActionResult JsonResult(string jsondata)
        {
            return new ActionResult(ActionResult.ActionTypeEnum.Json, Route, jsondata);
        }

        
        /// <summary>
        /// Retursn a new ActionResult with a literal string to be returned to the client
        /// </summary>
        /// <param name="data">String to return to the client</param>
        /// <returns>An ActionResult object to be consumed by the route handler</returns>
        public ActionResult LiteralResult(string data)
        {
            return new ActionResult(ActionResult.ActionTypeEnum.Literal, Route, data);
        }

        /// <summary>
        /// Retursn a new ActionResult with a literal string to be returned to the client
        /// </summary>
        /// <param name="data">String to return to the client</param>
        /// <returns>An ActionResult object to be consumed by the route handler</returns>
        public ActionResult LiteralResult(int data)
        {
            return LiteralResult(data.ToString());
        }

        /// <summary>
        /// Returns a new ActionResult with a literal string to be returned to the client
        /// </summary>
        /// <param name="data">String to return to the client</param>
        /// <returns>An ActionResult object to be consumed by the route handler</returns>
        public ActionResult Content(string data)
        {
            return new ActionResult(ActionResult.ActionTypeEnum.Literal, Route, data);
        }


        /// <summary>
        /// Writes a file to the response and ends the response
        /// </summary>
        /// <param name="filePath">The physical path to the file</param>
        /// <param name="contentType">The content type of the file</param>
        /// <param name="fileName">The name to be given to the file</param>
        public void FileResult(string filePath, string contentType, string fileName)
        {
            Route.App.Response.ContentType = contentType;
            Route.App.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            Route.App.Response.WriteFile(filePath);
            Route.App.Response.Flush();
            Route.App.ResponseEndCalled = true;
            Route.App.Response.End();
        }

        /// <summary>
        /// Redirects the user to another action within this controller
        /// </summary>
        /// <param name="actionName">The name of the action to redirect to</param>
        /// <returns>And action result to be consumed by the route handler</returns>
        public ActionResult RedirectToAction(string actionName)
        {
            //return RouteHelper.ExecuteAction(DnnApp.CurrentRoute.Controller, actionName);
            Route.Action = actionName;
            return Route.ExecuteRoute();
        }

        /// <summary>
        /// Redirects the user
        /// </summary>
        /// <param name="url">The url to redirect to</param>
        public void Redirect(string url)
        {
            Route.App.ResponseEndCalled = true;
            Route.App.Response.Redirect(url);
        }

        /// <summary>
        /// Redirects the user
        /// </summary>
        /// <param name="url">The url to redirect to</param>
        /// <param name="endResponse">Determine whether to end the execution of the current page</param>
        public void Redirect(string url, bool endResponse)
        {
            Route.App.ResponseEndCalled = endResponse;
            Route.App.Response.Redirect(url, endResponse);
        }

        /// <summary>
        /// Redirects the user to another action within a different controller
        /// </summary>
        /// <param name="actionName">The name of the action to redirect to</param>
        /// <param name="controllerName">The name of the controller that contains the action</param>
        /// <returns>And action result to be consumed by the route handler</returns>
        public ActionResult RedirectToAction(string controllerName, string actionName)
        {
            Route.Controller = controllerName;
            Route.Action = actionName;
            return Route.ExecuteRoute();
        }

        /// <summary>
        /// Determines weather the current request is a POST
        /// </summary>
        public bool IsPOST { get { return Route.App.Request.RequestType == "POST"; } }

        /// <summary>
        /// Determines weather the current request is a GET
        /// </summary>
        public bool IsGET { get { return Route.App.Request.RequestType == "GET"; } }

        /// <summary>
        /// Determines weather the request is authenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get { return Route.App.Request.IsAuthenticated; }
        }

        /// <summary>
        /// Checkes weather the current request is authenticated and throws an exception if it is not
        /// </summary>
        public void AuthenticatedOnly()
        {
            AuthenticatedOnly(Route.App.MVCStrings.AuthenticatedOnly);
        }

        /// <summary>
        /// Checkes weather the current request is authenticated and throws an exception if it is not.  
        /// </summary>
        /// <param name="errorMessage">The error message to diplay otherwise</param>
        public void AuthenticatedOnly(string errorMessage)
        {
            if (IsAuthenticated == false)
            {
                throw new SecurityException(errorMessage);
            }
        }



        /// <summary>
        /// Makes sure the request is a post. Throws an exception if it is not
        /// </summary>
        public void POSTRequestOnly()
        {
            POSTRequestOnly(Route.App.MVCStrings.PostRequestOnly);
        }

        /// <summary>
        /// Makes sure the request is a post. Throws an exception if it is not. Can also checks to make sure that the user is authenticated
        /// </summary>
        public void POSTRequestOnly(bool requireAuthentication)
        {
            if (requireAuthentication) AuthenticatedOnly();
            POSTRequestOnly(Route.App.MVCStrings.PostRequestOnly);
        }

        /// <summary>
        /// Makes sure that only editors can perform the action
        /// </summary>
        public void EditorsOnly()
        {
            AuthenticatedOnly();
            if (!Route.CanEdit) throw new ApplicationException("Insufficient rights!");
        }

        /// <summary>
        /// Makes sure the request is a post. Throws an exception if it is not
        /// </summary>
        /// <param name="errorMessage">The error message to diplay otherwise</param>
        public void POSTRequestOnly(string errorMessage)
        {
            if (IsPOST == false)
            {
                throw new ApplicationException(errorMessage);
            }
        }

        /// <summary>
        /// Makes sure the request is a get. Throws an exception if it is not
        /// </summary>
        public void GETRequestOnly()
        {
            GETRequestOnly(Route.App.MVCStrings.GetRequestOnly);
        }

        /// <summary>
        /// Makes sure the request is a GET request. Throws an exception if it is not. Can also checks to make sure that the user is authenticated
        /// </summary>
        public void GETRequestOnly(bool requireAuthentication)
        {
            if (requireAuthentication) AuthenticatedOnly();
            GETRequestOnly(Route.App.MVCStrings.PostRequestOnly);
        }

        /// <summary>
        /// Makes sure the request is a get. Throws an exception if it is not
        /// </summary>
        /// <param name="errorMessage">The error message to diplay otherwise</param>
        public void GETRequestOnly(string errorMessage)
        {
            if (IsGET == false)
            {
                throw new ApplicationException(errorMessage);
            }
        }

        /// <summary>
        /// Makes sure only an admin can execute this request
        /// </summary>
        public void AdminOnly()
        {
            AdminOnly(Route.App.MVCStrings.AdminOnly);
        }

        /// <summary>
        /// Makes sure only an admin can execute this request
        /// </summary>
        /// <param name="errorMessage">The error message to show to the user</param>
        public void AdminOnly(string errorMessage)
        {
            AuthenticatedOnly(errorMessage);
            if (!Route.UserIsAdmin)
                throw new SecurityException(errorMessage);
        }

        /// <summary>
        /// Makes sure only an host users can execute this request
        /// </summary>
        public void HostOnly()
        {
            HostOnly(Route.App.MVCStrings.HostOnly);
        }

        /// <summary>
        /// Makes sure only an host users can execute this request
        /// </summary>
        /// <param name="errorMessage">The error message to show to the user</param>
        public void HostOnly(string errorMessage)
        {
            AuthenticatedOnly(errorMessage);
            if (!Route.UserIsAdmin)
                throw new SecurityException(errorMessage);
        }
        
        /// <summary>
        /// Convert an object into a JSON string
        /// </summary>
        public string ToJson(object data)
        {
            return Serializer.ToJson(data);
        }

        /// <summary>
        /// Converts a JSON string into an object
        /// </summary>
        /// <typeparam name="T">The type to desirialize the string into</typeparam>
        /// <param name="jsonString">The JSON string to deserialize</param>
        public T FromJson<T>(string jsonString)
        {
            return Serializer.FromJson<T>(jsonString);
        }

    }

}
