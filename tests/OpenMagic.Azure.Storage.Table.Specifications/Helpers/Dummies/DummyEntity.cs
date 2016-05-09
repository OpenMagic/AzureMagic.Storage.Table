namespace OpenMagic.Azure.Storage.Table.Specifications.Steps.Table
{
    // Note that DummyEntity does not need to implement ITableEntity. Futhermore
    // properties PartitionKey & RowKey are not required, the only reason these
    // properties are used in this class is for code readability.
    internal class DummyEntity
    {
        public string PartitionKey { get; internal set; }
        public string RowKey { get; internal set; }
    }
}