using Microsoft.WindowsAzure.Storage.Table;

namespace OpenMagic.Azure.Storage.Table
{
    public interface IDynamicTableEntitySerializer<TEntity>
    {
        TEntity Deserialize(DynamicTableEntity row);
        DynamicTableEntity Serialize(TEntity entity);
    }
}