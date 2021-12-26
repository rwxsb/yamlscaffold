// See https://aka.ms/new-console-template for more information

using YamlScaffold.Cli.Output;
using YamlScaffold.Cli.Processors;


var scaffoldConfig = args.AsQueryable().FirstOrDefault();

if (scaffoldConfig == null)
{
    ConsoleOutput.Log("No such file found in the working directory.");
    scaffoldConfig = Console.ReadLine()!.Trim();
}

await ScaffoldingProcessor.Process(scaffoldConfig);