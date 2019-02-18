using System.Collections.Generic;
using System.Linq;
using Amazon.S3.Model;

namespace AmazonS3Extensions.Extensions
{
    static class S3TagsExtensions
    {
        public static IDictionary<string, string> ToDictionary(this IEnumerable<Tag> tags)
        {
            var tagDictionary = new Dictionary<string, string>();
            foreach ( var tag in tags)
            {
                tagDictionary[tag.Key] = tag.Value;
            }

            return tagDictionary;
        }

        public static IEnumerable<Tag> ToTagSet(this IDictionary<string, string> metaData, string originKey)
        {
            return metaData
                .Where(pair => pair.Key == originKey)
                .Select(tag =>
                    new Tag()
                    {
                        Key = tag.Key,
                        Value = tag.Value
                    });
        }
    }
}
