namespace OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies
{
    // Note that DummyEntity does not need to implement ITableEntity. Furthermore
    // properties PartitionKey & RowKey are not required, the only reason these
    // properties are used in this class is for code readability.
    internal class DummyEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}