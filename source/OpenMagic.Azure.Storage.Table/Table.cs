using System;
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

        public async Task<IEnumerable<TEntity>> FindByPartitionKeyAsync(string partitionKey)
        {
            // partitionKey is not logged as it could contain sensitive information
            var logPrefix = $"{nameof(FindByPartitionKeyAsync)}.(partitionKey: XXXXX)";
            LogTo.Trace(logPrefix);

            TableQuerySegment<DynamicTableEntity> result = null;

            var segment = 0;
            var rows = new List<DynamicTableEntity>();
            var table = GetTable();
            var query = (TableQuery<DynamicTableEntity>)table
                .CreateQuery<DynamicTableEntity>()
                .Where(row => row.PartitionKey == partitionKey);

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
