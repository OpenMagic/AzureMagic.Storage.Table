using Anotar.LibLog;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers
{
    internal static class AzureTableProvider
    {
        internal const string ConnectionString = "UseDevelopmentStorage=true;";

        static AzureTableProvider()
        {
            AzureStorageEmulator.StartIfNotRunning();
        }

        internal static CloudTable GetTable<TEntity>(bool clean = false, bool create = true)
        {
            return GetTable(AzureTableNamer.GetTableName<TEntity>(), clean, create);
        }

        internal static CloudTable GetTable(bool clean = false, bool create = true)
        {
            return GetTable(AzureTableNamer.GetTableName(), clean, create);
        }

        internal static CloudTable GetTable(string tableName, bool clean = false, bool create = true)
        {
            var table = CloudStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient()
                .GetTableReference(tableName);

            if (clean)
            {
                LogTo.Debug($"Cleaning table '{tableName}'...");
                table.DeleteIfExists();
            }

            if (create)
            {
                LogTo.Debug($"Creating table '{tableName}'...");
                table.CreateIfNotExists();
            }

            return table;
        }
    }
}