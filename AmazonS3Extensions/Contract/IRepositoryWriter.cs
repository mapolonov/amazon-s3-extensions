using System.Threading.Tasks;

namespace AmazonS3Extensions.Contract
{
    public interface IRepositoryWriter<in TKey, in TValue>
    {
        Task PostAsync(TKey key, TValue source);
    }
}
