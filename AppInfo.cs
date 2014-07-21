using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigfootDNN
{
    public class AppInfo
    {

        /// <summary>
        /// Instantiates the information about this module to be used through the application
        /// </summary>
        /// <param name="assemblyName">This is the name of the assembly/dll that contains the controllers</param>
        /// <param name="moduleNamespace">The root namespace of the module where the controllers reside)</param>
        /// <param name="folderName">Name of the folder where the module resides inside the DesktopModules directory</param>
        /// <param name="shortName">This is the short name of your module. This is the name used in the CSS files as well as the root div element on the route user control for your mvc views</param>
        /// <param name="moduleDbQualifier">The database object qualifier for this module</param>
        /// <param name="encryptionKey">Encryption key to use for the module (license etc.)</param>
        /// <param name="encriptionTempKey">Temporary encryption key used in transmission of secured data</param>
        public AppInfo(string assemblyName, string moduleNamespace, 
                       string folderName, string shortName, 
                       string moduleDbQualifier, 
                       string encryptionKey, string encriptionTempKey)
        {
            ModuleAssemblyName = assemblyName;
            ModuleNamespace = moduleNamespace;
            ModuleFolderName = folderName;
            ModuleShortName = shortName;
            ModuleDBObjectQualifier = moduleDbQualifier;
            EncryptionKey = encryptionKey;
            EncryptionTempKey = encriptionTempKey;
        }

        /// <summary>
        /// This is the name of the assembly/dll that contains the controllers
        /// </summary>
        public string ModuleAssemblyName { get; set; }

        /// <summary>
        /// The root namespace of the module where the controllers reside)
        /// </summary>
        public string ModuleNamespace { get; set; }

        /// <summary>
        /// Name of the folder where the module resides inside the DesktopModules directory
        /// </summary>
        public string ModuleFolderName { get; set; }

        /// <summary>
        /// This is the short name of your module. This is the name used in the CSS files as well as the root div element
        /// on the route user control for your mvc views
        /// </summary>
        public string ModuleShortName { get; set; }

        /// <summary>
        /// The database object qualifier for this module
        /// </summary>
        public string ModuleDBObjectQualifier { get; set; }

        /// <summary>
        /// Encryption key to use for the module (license etc.)
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Temporary encryption key used in transmission of secured data
        /// </summary>
        public string EncryptionTempKey { get; set; }

        

    }
}