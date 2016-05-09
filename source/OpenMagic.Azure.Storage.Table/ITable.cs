using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenMagic.Azure.Storage.Table
{
    public interface ITable<TEntity>
    {
        Task<IEnumerable<TEntity>> FindByPartitionKeyAsync(string partitionKey);
    }
}