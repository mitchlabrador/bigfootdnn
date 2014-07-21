using DotNetNuke.Entities.Users;
using System;
using System.Web;
using System.Web.WebPages;
using BigfootDNN.Helpers;

namespace BigfootDNN
{
    public abstract class RazorWebPage : WebPageBase
    {        
        protected override void ConfigurePage(WebPageBase parentPage)
        {
            base.ConfigurePage(parentPage);

            //Child pages need to get their context from the Parent
            Context = parentPage.Context;
        }
        
        /// <summary>
        /// This is the route information for the current route being executed
        /// </summary>
        public RouteInfo Route;

        /// <summary>
        /// Alias to Route.PortalId
        /// </summary>
        public int PortalId { get { return Route.PortalId; } }

        /// <summary>
        /// Alias to Route.UserId
        /// </summary>
        public int UserId { get { return Route.UserId; } }
        
        /// <summary>
        /// The current user
        /// </summary>
        public UserInfo User { get { return Route.User; } }
        
        /// <summary>
        /// Creates a reference to the current application
        /// </summary>
        public DnnMvcApplication App { get { return Route.App; } }

        /// <summary>
        /// Determines weather this view is being rendered as a partial view
        /// </summary>
        public bool IsRenderPartial { get; set; }
                
        /// <summary>
        /// Render a partial view onto the page
        /// </summary>
        /// <param name="ViewName">The name of the view within the current controller to render</param>
        /// <returns>The rendered html output of the view</returns>
        public string RenderPartial(string ViewName)
        {
            return RenderPartial(Route.Controller, ViewName, null);
        }

        /// <summary>
        /// Render a partial view onto the page
        /// </summary>
        /// <param name="ViewName">The name of the view within the current controller to render</param>
        /// <param name="data">The data to be used as the views model</param>
        /// <returns>The rendered html output of the view</returns>
        public string RenderPartial(string ViewName, object data)
        {
            return RenderPartial(Route.Controller, ViewName, data);
        }

        /// <summary>
        /// Render a partial view onto the page only if the specified condition is met
        /// </summary>
        /// <param name="Condition">The condition that must be met in order for the view to be rendered</param>
        /// <param name="ViewName">The name of the view within the current controller to render</param>
        /// <param name="data">The data to be used as the views model</param>
        /// <returns>The rendered html output of the view</returns>
        public string RenderPartialIf(bool Condition, string ViewName, object data)
        {
            return Condition ? RenderPartial(ViewName, data) : "";
        }

        /// <summary>
        /// Render a partial view onto the page only if the specified condition is met
        /// </summary>
        /// <param name="Condition">The condition that must be met in order for the view to be rendered</param>
        /// <param name="ControllerName">The name of the controller folder where the view exists</param>
        /// <param name="ViewName">The name of the view within the controller folder specified</param>
        /// <param name="data">The data to be used as the views model</param>
        /// <returns>The rendered html output of the view</returns>
        public string RenderPartialIf(bool Condition, string ControllerName, string ViewName, object data)
        {
            return Condition ? RenderPartial(ControllerName, ViewName, data) : "";
        }

        /// <summary>
        /// Render a partial view onto the page.
        /// </summary>
        /// <param name="ControllerName">The name of the controller folder where the view exists</param>
        /// <param name="ViewName">The name of the view within the controller folder</param>
        /// <param name="data">The data to be used as the view model</param>
        /// <returns>The rendered html output of the view</returns>
        public string RenderPartial(string ControllerName, string ViewName, object data)
        {
            try
            {
                var viewPath = Route.GetViewPath(ControllerName, ViewName);
                return Route.App.RenderRazorViewToString(viewPath, data, "", true);
            }
            catch (Exception ex)
            {
                Route.App.HandleException(ex, false);
                var errorViewModel = ErrorViewModel.Create(ex, Route.App.Request.Url.ToString());
                return Route.App.RenderRazorViewToString(Route.App.ErrorViewPath, errorViewModel);
            }
        }

        /// <summary>
        /// This functions is called from the markup to handle any view error. It has the logic to know weather to output the error to the screen
        /// or in the case of being partially rendered to raise the error to the RenderView function
        /// </summary>
        /// <param name="ex">The exception to be handled</param>
        public string HandleViewError(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException("ex");
            // If it is rendering in partial mode, then throw the error because the host control will properly catch it
            if (IsRenderPartial) throw ex;

            Route.App.HandleException(ex, false);
            return Route.App.RenderErrorViewToString(ex);
        }
        
        /// <summary>
        /// Creates a unique client id specific to this module instance on the page. This is useful when refering to element ids from javascript as
        /// you might have the same id if the module is added to the page several times. It simply adds an underscore plus the 
        /// module id to the name
        /// </summary>
        /// <param name="id">The id of the element you would like to make unique to the module</param>
        /// <returns>It returns {id}_{ModuleId}</returns>
        public string uid(string id)
        {
            return App.uid(id);
        }


        /// <summary>
        /// Contains the HTML helpers you can use in the views. It is an alias class that provides easy access to the HTMLHelper class.
        /// </summary>
        public HtmlHelper Html { get { return Route.App.Html; } }

        /// <summary>
        /// Contains the Javascript helpers you can use in the views. It is an alias class that provides easy access to the JSHelper class.
        /// </summary>
        public JSHelper JS { get { return Route.App.JS; } }

        /// <summary>
        /// This is a general purpose html tag builder class. It is an alias class that provides easy access to the TagBuilder class.
        /// </summary>
        public TagBuilder TB(string tagName) { return new TagBuilder(tagName); }

        /// <summary>
        /// Creates a new AJAX request object you can build on in order to make ajax call to the server
        /// </summary>
        /// <param name="url">The url agains which you are executing the ajax request</param>
        public AjaxRequest Ajax(string url) { return AjaxRequest.Create(url); }

        
        /// <summary>
        /// Creates a partial from. Partial forms are a way to make field groups within a form behave like a form
        /// for the purposes of validation, and POST.
        /// </summary>
        /// <param name="formId">The element id that wraps the field group (i.e. div etc)</param>
        /// <param name="addScriptTag">Determines whether to add the script tag</param>
        /// <returns>A script to ouputed on the page</returns>
        public string CreatePartialForm(string formId, bool addScriptTag = true) { return JS.CreatePartialForm(formId, addScriptTag); }

        /// <summary>
        /// Converts a form into an ajax form
        /// </summary>
        /// <param name="formName">The name of the form</param>
        /// <param name="postUrl">The url where the form will be posted</param>
        public AjaxForm CreateAjaxForm(string formName, string postUrl)
        {
            return AjaxForm.Create(formName, postUrl);
        }
        
        /// <summary>
        /// Submits a partial from. Partial forms are a way to make field groups "Partial Form" within a form behave like a form
        /// for the purposes of validation, and POST.
        /// </summary>
        /// <param name="formId">The element id that wraps the field group "Partial Form" (i.e. div etc)</param>
        /// <param name="url">The url you would like to submit the form to</param>
        /// <param name="validate">Determines weather to validate the form before submission or not</param>
        /// <returns>AJAX Request object upon which you can build other actions, like clearing the contents of a field upon callback etc.</returns>
        public AjaxRequest SubmitPartialForm(string formId, string url, bool validate = true) { return JS.SubmitPartialForm(formId, url, validate); }

        /// <summary>
        /// A helper builder function that outputs the text only if the specified condition is true
        /// (i.e. <%=RenderIf(user.HasGrups, user.GroupString)%>
        /// </summary>
        /// <param name="condition">The condition that determines weather the text is rendered or not</param>
        /// <param name="text">The text / html to render if the condition is true </param>
        /// <returns>Returns the RenderIfBuilder upon which you can further build with an .else statement other if conditions etc.</returns>
        public RenderIfBuilder RenderIf(bool condition, string text) { return Html.RenderIf(condition, text); }

        /// <summary>
        /// HtmlEncode the string for output
        /// </summary>
        /// <param name="data">The string to be encoded</param>
        /// <returns>HtmlEncoded string</returns>
        public string E(string data)
        {
            return HttpUtility.HtmlEncode(data);
        }

        /// <summary>
        /// Quick access to the String.Format function
        /// </summary>
        /// <param name="value">The string where the values will be inserted i.e. The bird flew {0} miles</param>
        /// <param name="values">The values to merge with the format string</param>
        /// <returns>The formated string</returns>
        public string F(string value, params string[] values)
        {
            return string.Format(value, values);
        }

        /// <summary>
        /// Helps in the creation of a unique id to later be used in collection binding
        /// </summary>
        /// <param name="prefix">The prefix that ties the colleciton together</param>
        /// <param name="id">The unique id of the collection item</param>
        /// <param name="fieldName">The name of the field to bind to for a certain item</param>
        public string ColId(string prefix, string id, string fieldName)
        {
            return prefix + "_" + id + "_" + fieldName;
        }

        /// <summary>
        /// Helps in the creation of a unique id to later be used in collection binding
        /// </summary>
        /// <param name="prefix">The prefix that ties the colleciton together</param>
        /// <param name="id">The unique id of the collection item</param>
        /// <param name="fieldName">The name of the field to bind to for a certain item</param>
        public string ColId(string prefix, int id, string fieldName)
        {
            return prefix + "_" + id + "_" + fieldName;
        }

        /// <summary>
        /// Creates a properly formated url parameter. 
        /// </summary>
        /// <param name="paramName">The name of the url parameter</param>
        /// <param name="value">The value of the url parameter</param>
        /// <returns>The properly formatted parameter i.e. parameter=value</returns>
        public string UrlP(string paramName, string value)
        {
            return paramName + "=" + value;
        }
                
        /// <summary>
        /// Easy access to the UrlBuilder class. Aids in the fluid creation of urls and url parameters
        /// </summary>
        public UrlBuilder QS { get { return new UrlBuilder(); } }

        /// <summary>
        /// Class used to aid in the collection of posted values from the request. First it tries the QueryString and second it tries the form collection
        /// </summary>
        public PostHelper Post { get { return Route.App.Values; } }

        /// <summary>
        /// Creates a DIV element with the provided content and assigns the provided css classes
        /// </summary>
        /// <param name="content">The content for the DIV</param>
        /// <param name="classes">CSS Classes to add</param>
        /// <returns>A TagBuilder</returns>
        public TagBuilder DIV(string content, string classes)
        {
            return Html.DIV(content, classes);
        }

        /// <summary>
        /// Creates a DIV element y calling string.format on the content and assigns the provided css classes
        /// </summary>
        /// <param name="content">The content for the DIV</param>
        /// <param name="classes">CSS Classes to add</param>
        /// <param name="formatValues">The values to merge with the content</param>
        /// <returns>A TagBuilder</returns>
        public TagBuilder DIV(string content, string classes, params string[] formatValues)
        {
            return Html.DIV(string.Format(content, formatValues), classes);
        }

        /// <summary>
        /// Creates a SPAN element with the provided content and assigns the provided css classes
        /// </summary>
        /// <param name="content">The content for the SPAN</param>
        /// <param name="classes">CSS Classes to add</param>
        /// <returns>A TagBuilder</returns>
        public TagBuilder SPAN(string content, string classes)
        {
            return Html.SPAN(content, classes);
        }

        /// <summary>
        /// Creates a SPAN element with the provided by calling string.format on the content and assigns the provided css classes
        /// </summary>
        /// <param name="content">The content for the SPAN</param>
        /// <param name="classes">CSS Classes to add</param>
        /// <param name="formatValues">The values to merge with the content</param>
        /// <returns>A TagBuilder</returns>
        public TagBuilder SPAN(string content, string classes, params string[] formatValues)
        {
            return Html.SPAN(string.Format(content, formatValues), classes);
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

        /// <summary>
        /// Creates a javascript reference
        /// </summary>
        public string JSReference(string url)
        {
            return Html.JSReference(url);
        }

        /// <summary>
        /// Creates a CSS reference
        /// </summary>
        public string CSSReference(string url)
        {
            return Html.CSSReference(url);
        }

        /// <summary>
        /// A url to a static file
        /// </summary>
        /// <param name="filename">
        ///     The relative path to the file. If only the file name is specified then the well known location for files will be used
        ///     e.g. image.jpg would be the equivalent of ~/content/images/image.jpg which gets translated into [applicaitonroot]/content/images/image.jpg
        ///     same with JS, css, etc.
        /// </param>
        /// <returns>A url that may be used for client resources</returns>
        public string ClientUrl(string filename)
        {
            return App.StaticUrl(filename);
        }

        /// <summary>
        /// A client usable url to a particular controller and action. 
        /// </summary>
        /// <returns>A url meant to be used as an interactive url rather than for ajax request</returns>
        public string ClientUrl(string controller, string action)
        {
            return App.MvcUrl(controller, action);
        }

        /// <summary>
        /// A client usable url to a particular controller and action which specifes some querystring
        /// </summary>
        /// <returns>A url meant to be used as an interactive url rather than for ajax request</returns>

        public string ClientUrl(string controller, string action, string querystring)
        {
            return App.MvcUrl(controller, action, querystring);
        }

        /// <summary>
        /// A client usable url to a particular controller and action for use in Ajax requests
        /// </summary>
        /// <returns>A url meant to be used for Ajax requests</returns>
        public string ClientUrlForAjax(string controller, string action)
        {
            return App.MvcAjaxUrl(controller, action);
        }

        /// <summary>
        /// A client usable url to a particular controller and action for use in Ajax requests. Specifies a certain querystring
        /// </summary>
        /// <returns>A url meant to be used for Ajax requests</returns>
        public string ClientUrlForAjax(string controller, string action, string querystring)
        {
            return App.MvcAjaxUrl(controller, action, querystring);
        }


    }

    public abstract class RazorWebPage<T>:RazorWebPage
    {
        public T Model { get; set; }
    }
}