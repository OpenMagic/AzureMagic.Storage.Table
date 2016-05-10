namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers
{
    internal static class AzureTableNamer
    {
        private static int _tableNumber;

        internal static string GetTableName()
        {
            return GetTableName((++_tableNumber).ToString());
        }

        internal static string GetTableName(string name)
        {
            return $"am{name}";
        }

        internal static string GetTableName<TEntity>()
        {
            return GetTableName(typeof(TEntity).Name + "s");
        }
    }
}