using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies;

namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers
{
    public class Given
    {
        public CloudTable CloudTable { get; internal set; }
        public CloudStorageAccount StorageAccount { get; internal set; }
        public TableEntity[] TableEntities { get; internal set; }
        public TableEntity TableEntity { get; internal set; }
        internal DummyEntityWithTypes DummyEntityWithTypes { get; set; }
        public DynamicTableEntity DynamicTableEntity { get; set; }
    }
}