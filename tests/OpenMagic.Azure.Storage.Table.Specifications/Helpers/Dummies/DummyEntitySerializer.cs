using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies
{
    internal class DummyEntitySerializer : IDynamicTableEntitySerializer<DummyEntity>
    {
        public DummyEntity Deserialize(DynamicTableEntity row)
        {
            return new DummyEntity
            {
                PartitionKey = row.PartitionKey,
                RowKey = row.RowKey
            };
        }

        public DynamicTableEntity Serialize(DummyEntity row)
        {
            throw new NotImplementedException();
        }
    }
}