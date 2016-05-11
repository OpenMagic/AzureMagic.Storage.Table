using Microsoft.WindowsAzure.Storage.Table;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies;

namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers
{
    public class Actual
    {
        internal DummyEntity[] DummyEntities { get; set; }
        internal DummyEntityWithTypes DummyEntityWithTypes { get; set; }
        public DynamicTableEntity DynamicTableEntity { get; set; }
    }
}