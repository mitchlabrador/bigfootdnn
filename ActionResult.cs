namespace BigfootDNN
{

    /// <summary>
    /// This is the class that is returned by the controller and then executed by the route handler.  
    /// </summary>
    public class ActionResult
    {
        public enum ActionTypeEnum { View = 1, Literal = 2, Json = 3 }

        /// <summary>
        /// The current route being executed
        /// </summary>
        public RouteInfo Route { get; set; }

        /// <summary>
        /// Model, JSON, or literal string from the executed action
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Determines the type of result. View, Literal, or Json
        /// </summary>
        public ActionTypeEnum Type { get; set; }


        /// <summary>
        /// Determines whether the view is a razor view
        /// </summary>
        public bool IsRazorView { get { return Route.ViewPath.ToLowerInvariant().EndsWith(".cshtml"); } }

        /// <summary>
        /// Creates a new action result of the specified type and also provides a model
        /// </summary>
        /// <param name="type">The specified result type</param>
        /// <param name="viewData">The Model object, JSON, or literal string from the executed action </param>
        /// <param name="route">The route information for the current result</param>
        public ActionResult(ActionTypeEnum type, RouteInfo route, object viewData)
        {
            Data = viewData;
            Type = type;
            Route = route;
        }
    }
}
