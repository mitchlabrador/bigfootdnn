using System;
using DotNetNuke.Framework.Providers;
using DotNetNuke.Common.Utilities;
using BigfootDNN.Helpers;


namespace BigfootDNN
{


    /// <summary>
    /// Implements the DataProvider pattern used by DotNetNuke. But also adds the SQL extension which which uses the 
    /// values from the provider to run LINQ like queries agains the database using BigfootSQL for more details about
    /// bigfootsql see http://bigfoot.clodeplex.com
    /// </summary>
    public class DataProvider
    {

        private const string ProviderType = "data";

        public DataProvider(string moduleDbObjectQualifier)
        {
            // Retreive the provider configuration if not in cache
            ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);

            // Read the configuration specific information for this provider
            var objProvider = (Provider)ProviderConfiguration.Providers[ProviderConfiguration.DefaultProvider];

            // Read the attributes for this provider
            //  Get Connection string from web.config
            ConnectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(ConnectionString))
            {
                // Use connection string specified in provider
                ConnectionString = objProvider.Attributes["connectionString"];
            }

            UpgradeConnectionString = !string.IsNullOrEmpty(objProvider.Attributes["upgradeConnectionString"]) ? objProvider.Attributes["upgradeConnectionString"] :
                                                                                                                  ConnectionString;

            ProviderPath = objProvider.Attributes["providerPath"];

            ObjectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(ObjectQualifier) && ObjectQualifier.EndsWith("_") == false)
            {
                ObjectQualifier += "_";
            }

            DatabaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(DatabaseOwner) && DatabaseOwner.EndsWith(".") == false)
            {
                DatabaseOwner += ".";
            }

            // Get the module qualifier
            ModuleQualifier = (string.IsNullOrEmpty(moduleDbObjectQualifier) ? "" : moduleDbObjectQualifier + "_");

        }


        /// <summary>
        /// IoC Contractor. Used by the testing framework.
        /// </summary>
        /// <param name="CNString">The connection string</param>
        /// <param name="Qualifier">The database object qualifier (e.g. dnn)</param>
        /// <param name="Owner">The database object owner (e.g. dbo)</param>
        public DataProvider(string CNString, string Qualifier, string Owner)
        {
            ConnectionString = CNString;
            ObjectQualifier = (string.IsNullOrEmpty(Qualifier) == false ? Qualifier + "_" : "");
            DatabaseOwner = Owner + ".";
        }

        /// <summary>
        /// IoC Contractor. Used by the testing framework.
        /// </summary>
        /// <param name="CNString">The connection string</param>
        /// <param name="Qualifier">The database object qualifier (e.g. dnn)</param>
        /// <param name="Owner">The database object owner (e.g. dbo)</param>
        public DataProvider(string CNString, string Qualifier, string Owner, string ModuleQualifier)
        {
            ConnectionString = CNString;
            ObjectQualifier = (string.IsNullOrEmpty(Qualifier) ? "" : Qualifier + "_");
            DatabaseOwner = Owner + ".";
            this.ModuleQualifier = (string.IsNullOrEmpty(ModuleQualifier) ? "" : ModuleQualifier + "_");
        }

        public ProviderConfiguration ProviderConfiguration { get; set; }

        public string ConnectionString { get; set; }

        public string UpgradeConnectionString { get; set; }

        public string ProviderPath { get; set; }

        public string ObjectQualifier { get; set; }

        public string DatabaseOwner { get; set; }

        public string ModuleQualifier { get; set; }

        public string GetName(string name) { return DatabaseOwner + ObjectQualifier + ModuleQualifier + name; }

        public object GetNull(object Field) { return Null.GetNull(Field, DBNull.Value); }

    }

}
