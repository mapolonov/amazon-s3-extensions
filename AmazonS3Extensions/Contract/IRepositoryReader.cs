using System.Threading.Tasks;

namespace AmazonS3Extensions.Contract
{
    public interface IRepositoryReader<in TKey, TValue>
    {
        Task<TValue> GetAsync(TKey key, bool includeMetadata);
    }
}
