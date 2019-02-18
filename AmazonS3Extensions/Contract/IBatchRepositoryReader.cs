using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazonS3Extensions.Contract
{
    public interface IBatchRepositoryReader<TKey, in TFilter>
    {
        Task<IEnumerable<TKey>> GetAllKeysAsync(TFilter filter);
    }
}
