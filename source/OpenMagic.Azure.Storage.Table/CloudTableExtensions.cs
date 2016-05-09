using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace OpenMagic.Azure.Storage.Table
{
    public static class CloudTableExtensions
    {
        public static Task InsertAsync(this CloudTable table, ITableEntity tableEntity)
        {
            return table.ExecuteAsync(TableOperation.Insert(tableEntity));
        }

        public static async Task InsertAsync(this CloudTable table, IEnumerable<ITableEntity> tableEntities)
        {
            foreach (var tableEntity in tableEntities)
            {
                await table.InsertAsync(tableEntity);
            }
        }
    }
}