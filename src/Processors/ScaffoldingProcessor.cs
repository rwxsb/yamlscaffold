using YamlScaffold.Cli.Builders;
using YamlScaffold.Cli.Deserializers;
using YamlScaffold.Cli.FileSystem;
using YamlScaffold.Cli.Output;

namespace YamlScaffold.Cli.Processors
{
    internal static class ScaffoldingProcessor
    {
        internal static async Task Process(string config)
        {
            try
            {
                var yamlContent = await FileSystemManager.ReadAllTextAsync(config);

                var scaffoldObject = YamlDeserializer.Deserialize(yamlContent);

                var builtCommand = CommandStringBuilder.Init(scaffoldObject).Build();

                CommandProcessor.Run(builtCommand, true);
            }
            catch (Exception ex)
            {
                ConsoleOutput.Log(ex.Message);
            }
        }
    }
}
