using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers
{
    public class Given
    {
        public CloudTable CloudTable { get; internal set; }
        public CloudStorageAccount StorageAccount { get; internal set; }
        public TableEntity[] TableEntities { get; internal set; }
        public TableEntity TableEntity { get; internal set; }
    }
}