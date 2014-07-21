using System;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;

namespace BigfootDNN
{

    /// <summary>
    /// This is the base of the RouteHandler control. It is in charged of handling all MVC AJAX requests to the module.
    /// It is identical to the RouteUserControl except that it only ouputs to the response the result of the ajax call without 
    /// any initialization code. Sice it assumes that it is an ajax request, it also asumes that the page has already been 
    /// initialized and has the correct scripts etc. So only the content of the ActionResult is returned in the response
    /// </summary>
    public abstract class BaseRouteHandler : System.Web.IHttpHandler
    {
                
        /// <summary>
        /// Called inmediately after the route has been created and before the request has been processed
        /// </summary>
        public abstract void RouteCreated(RouteInfo Route);

        /// <summary>
        /// Must be implemented in order to get the application info
        /// </summary>
        public abstract AppInfo GetAppInfo();

        public RouteInfo Route;
        public DnnMvcApplication App;
        
        public void ProcessRequest(System.Web.HttpContext context)
        {            
            try
            {
                // Create the route from the request
                Route = RouteInfo.CreateFromRequest(GetAppInfo());

                // Call the route created
                RouteCreated(Route);

                // Execute the action
                var result = Route.ExecuteRoute();

                // Process the result
                ProcessResult(result);

            }
            catch (Exception ex)
            {
                if (Route == null)
                {
                    // Log the exception in DNN
                    Exceptions.LogException(ex);
                    throw ex;
                }
                else
                {
                    Route.App.HandleException(ex);
                }
            }
        }

        private void ProcessResult(ActionResult result)
        {
            switch (result.Type)
            {
                case ActionResult.ActionTypeEnum.View:
                    result.Route.App.SetNoCache();
                    result.Route.App.RenderRazorViewToResponse(result.Route.ViewPath, result.Data);
                    break;
                case ActionResult.ActionTypeEnum.Literal:
                    result.Route.App.RenderLiteral(result.Data.ToString());
                    break;
                case ActionResult.ActionTypeEnum.Json:
                    result.Route.App.RenderJson(result.Data.ToString());
                    break;
            }
        }

        public bool IsReusable { get { return false; } }

    }

}
