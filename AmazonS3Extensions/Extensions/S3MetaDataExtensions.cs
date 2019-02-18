using System.Collections.Generic;
using System.Linq;
using Amazon.S3.Model;

namespace AmazonS3Extensions.Extensions
{
    static class S3MetaDataExtensions
    {
        public static IDictionary<string, string> ToDictionary(this MetadataCollection metadataCollection)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var key in metadataCollection.Keys)
            {
                var internalKey = key.Replace("x-amz-meta-", string.Empty);
                dictionary[internalKey] = metadataCollection[key];
            }

            return dictionary;
        }

        public static void CopyToMetadataCollection(this IDictionary<string, string> metaData, MetadataCollection metadataCollection, string originKey)
        {
            foreach (var data in metaData.Where(pair => pair.Key != originKey))
            {
                metadataCollection.Add(data.Key, data.Value);
            }
        }
    }
}