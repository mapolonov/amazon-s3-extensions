using System.Threading.Tasks;

namespace AmazonS3Extensions.Contract
{
    public interface IBatchRepositoryCleaner<in TFilter>
    {
        Task DeleteAsync(TFilter filter);
    }
}
