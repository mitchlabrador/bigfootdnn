using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Compilation;
using System.Web.WebPages;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Modules;


namespace BigfootDNN
{
    public class RazorEngine
    {
        
        public RazorEngine(string razorScriptFile, RouteInfo route, string localResourceFile = "")
        {            
            // Validate Parameters
            if (string.IsNullOrEmpty(razorScriptFile))
            {
                throw new ArgumentException("razorScriptFile may not be empty", "razorScriptFile");
            }
            if (route == null)
            {
                throw new ArgumentException("Route may not be empty", "route");
            }

            // Set local variables
            Route = route;
            RazorScriptFile = razorScriptFile;
            LocalResourceFile = localResourceFile ??
                                Path.Combine(Path.GetDirectoryName(razorScriptFile), Localization.LocalResourceDirectory, Path.GetFileName(razorScriptFile) + ".resx");

            // Compile the script file
            var compiledType = BuildManager.GetCompiledType(RazorScriptFile);
            if (compiledType == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unable to compile script file '{0}'.", new object[] { RazorScriptFile }));
            }

            // Create a new instance of the newly compiled script file
            var pageInstance = RuntimeHelpers.GetObjectValue(Activator.CreateInstance(compiledType));
            if (pageInstance == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unable to create page instance of script file '{0}'.", new object[] { RazorScriptFile }));
            }

            // Make sure the script file page was compiled, a new instance was created and can properly be unboxed
            var objectValue = RuntimeHelpers.GetObjectValue(pageInstance);
            if ((objectValue == null))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The webpage found at '{0}' was not created.", new object[] { RazorScriptFile }));
            }

            // Ensure the newly created page is of type RazorWebPage
            Webpage = objectValue as RazorWebPage;
            if ((Webpage == null))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The webpage at '{0}' must derive from RazorWebPage.", new object[] { RazorScriptFile }));
            }

            // Set the context for the newly create page as well as the Route information etc.
            Webpage.Context = Route.App.Context;
            Webpage.VirtualPath = VirtualPathUtility.GetDirectory(RazorScriptFile);
            Webpage.Route = Route;            
        }

        /// <summary>
        /// Holds the Route for the current request
        /// </summary>
        public RouteInfo Route { get; set; }


        /// <summary>
        /// The Razor script file being rendered
        /// </summary>
        protected string RazorScriptFile { get; set; }
        
        /// <summary>
        /// The local resource file to be used within the render
        /// </summary>
        protected string LocalResourceFile { get; set; }

        /// <summary>
        /// The newly created WebPage object from the script
        /// </summary>
        public RazorWebPage Webpage { get; set; }
        
        /// <summary>
        /// Renders a strongly typed web page
        /// </summary>
        /// <param name="writer">The output writer (may be a string builder if rendering in memory)</param>
        /// <param name="model">The model object to assign to the page</param>
        /// <param name="isRenderPartial">Determines whether it is rendering a partial view</param>
        public void Render(TextWriter writer, object model = null, bool isRenderPartial = false)
        {
            if (model != null)
            {
                var prop = Webpage.GetType().GetProperty("Model");
                if (prop != null) prop.SetValue(Webpage, model, null);
            }
            Webpage.IsRenderPartial = isRenderPartial;
            Webpage.ExecutePageHierarchy(new WebPageContext(Route.App.Context, Webpage, null), writer, Webpage);
        }
            
    }
}