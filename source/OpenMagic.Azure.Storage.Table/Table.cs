using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace OpenMagic.Azure.Storage.Table
{
    public class Table<TEntity> : ITable<TEntity>
    {
        private readonly string _connectionString;
        private readonly IDynamicTableEntitySerializer<TEntity> _serializer;
        private readonly string _tableName;

        public Table(string connectionString, string tableName, IDynamicTableEntitySerializer<TEntity> serializer)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _serializer = serializer;
        }

        public Task<IEnumerable<TEntity>> FindByPartitionKeyAsync(string partitionKey)
        {
            // partitionKey is not logged as it could contain sensitive information
            LogTo.Trace($"{nameof(FindByPartitionKeyAsync)}.(partitionKey: XXXXX)");

            var query = CreateQuery().Where(row => row.PartitionKey == partitionKey);

            return ExecuteQueryAsync((TableQuery<DynamicTableEntity>)query);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            LogTo.Trace($"{nameof(GetAllAsync)}.()");
            return ExecuteQueryAsync(CreateQuery());
        }

        private TableQuery<DynamicTableEntity> CreateQuery()
        {
            return GetTable().CreateQuery<DynamicTableEntity>();
        }

        private async Task<IEnumerable<TEntity>> ExecuteQueryAsync(TableQuery<DynamicTableEntity> query)
        {
            const string logPrefix = nameof(ExecuteQueryAsync);

            TableQuerySegment<DynamicTableEntity> result = null;

            var segment = 0;
            var rows = new List<DynamicTableEntity>();

            do
            {
                result = await query.ExecuteSegmentedAsync(result?.ContinuationToken);
                rows.AddRange(result);

                LogTo.Debug(() => $"{logPrefix}: Segment {++segment} completed, so far {rows.Count:N0} rows have been retrieved.");
            } while (result.ContinuationToken != null);

            LogTo.Debug(() => $"{logPrefix}: retrieved {rows.Count:N0} rows.");
            return rows.Select(row => _serializer.Deserialize(row));
        }

        private CloudTable GetTable()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(_tableName);

            return table;
        }
    }
}