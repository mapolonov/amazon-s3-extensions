using System.Collections.Generic;
using System.IO;

namespace AmazonS3Extensions.Contract
{
    public interface IDataContainerConverter<TValue>
    {
        string GetOriginKey();
        Stream GetStream(TValue value);
        IDictionary<string, string> GetMetaData(TValue value);
        TValue GetDataContainer(Stream stream, IDictionary<string, string> metaData);
    }
}