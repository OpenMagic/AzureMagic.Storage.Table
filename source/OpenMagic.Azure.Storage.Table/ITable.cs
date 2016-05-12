using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace OpenMagic.Azure.Storage.Table
{
    // todo: document
    public interface ITable<TEntity>
    {
        TableQuery<DynamicTableEntity> CreateQuery();
        Task<IEnumerable<TEntity>> ExecuteQueryAsync(TableQuery<DynamicTableEntity> query);
        Task<IEnumerable<TEntity>> FindByPartitionKeyAsync(string partitionKey);
        Task<IEnumerable<TEntity>> GetAllAsync();
        CloudTable GetCloudTable();
    }
}