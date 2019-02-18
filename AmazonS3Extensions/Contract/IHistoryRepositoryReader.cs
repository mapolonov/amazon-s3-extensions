using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazonS3Extensions.Contract
{
    public interface IHistoryRepositoryReader<TKey, TValue>
    {
        Task<IEnumerable<TValue>> GetHistoryAsync(TKey key, bool includeMetadata);
        Task<IEnumerable<TKey>> GetS3FileKeyVersionsAsync(TKey key);
    }
}