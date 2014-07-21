using DotNetNuke.Services.Exceptions;
using System;
using System.Web.UI;
using BigfootDNN.Helpers;

namespace BigfootDNN
{

    /// <summary>
    /// This is the base class of the RouteUserControl. In a purely MVC application it is the only registered DNN 
    /// control and handles all the MVC requests for the module. It uses the URL route parameter to identify the proper
    /// controller and action to execute
    /// </summary>
    public abstract class BaseRouteUserControl : DotNetNuke.Entities.Modules.PortalModuleBase
    {

        /// <summary>
        /// Called inmediately after the route has been created and before the request has been processed
        /// </summary>
        public abstract void RouteCreated(RouteInfo Route);

        /// <summary>
        /// Must be implemented in order to get the application info
        /// </summary>
        public abstract AppInfo GetAppInfo();

        /// <summary>
        /// Holds the current route information
        /// </summary>
        public RouteInfo Route;

        public DnnMvcApplication App { get { return Route.App; } }


        // Initialize the route control
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                // Create the route from the request
                Route = RouteInfo.CreateFromRequest(GetAppInfo(), this);
                

                // Call the route created
                RouteCreated(Route);

                // Execute the action
                var result = Route.ExecuteRoute();

                // Process the result
                ProcessResult(result);

            }
            catch (Exception ex)
            {
                // Log the exception in DNN
                Exceptions.LogException(ex);
                if (Route == null)
                {
                    throw ex;
                }
                else
                {
                    GetRouteContent().Controls.Add(new LiteralControl(Route.App.RenderErrorViewToString(ex)));
                }
            }
        }

        private Control GetRouteContent()
        {
            return FindControl("RouteContent");
        }

        private void ProcessResult(ActionResult result)
        {
            switch (result.Type)
            {
                case ActionResult.ActionTypeEnum.View:
                    GetRouteContent().Controls.Add(new LiteralControl(Route.App.RenderRazorViewToString(result.Route.ViewPath, result.Data)));
                    break;
                case ActionResult.ActionTypeEnum.Literal:
                    result.Route.App.RenderLiteral(result.Data.ToString());
                    break;

                case ActionResult.ActionTypeEnum.Json:
                    result.Route.App.RenderJson(result.Data.ToString());
                    break;
            }
        }

        private void LoadErrorView(Exception ex)
        {
            // Load the error view
            try
            {
                var returnUrl = "";
                if (Route != null) returnUrl = Route.GetMvcUrl();
                var data = ErrorViewModel.Create(ex, returnUrl);
                GetRouteContent().Controls.Add(new LiteralControl(Route.App.RenderRazorViewToString(Route.App.ErrorViewPath, data)));
            }
            catch (Exception)
            {
                Response.Write(ex.ToString());
            }
            
        }

        
        /// <summary>
        /// Gets the form name. Can be used by jquery (includes the pre #)
        /// </summary>
        public string FormName() { return Page.Form != null ? "#" + Page.Form.ClientID : "#Form"; }

    }


}
