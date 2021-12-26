using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlScaffold.Cli.Deserializers
{
    internal static class YamlDeserializer
    {
        internal static IDictionary<dynamic, dynamic> Deserialize(string content)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<Dictionary<dynamic, dynamic>>(content);
        }
    }
}
