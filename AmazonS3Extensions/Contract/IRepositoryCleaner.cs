using System.Threading.Tasks;

namespace AmazonS3Extensions.Contract
{
    public interface IRepositoryCleaner<in TKey>
    {
        Task DeleteAsync(TKey key);
    }
}
