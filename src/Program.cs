// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlScaffold.Cli.Builders;
using YamlScaffold.Cli.FileSystem;


var scaffoldConfig = args.AsQueryable().FirstOrDefault();


try
{
    var deserializer = new DeserializerBuilder()
    .WithNamingConvention(HyphenatedNamingConvention.Instance)
    .Build();

    var yamlContent = await FileSystemManager.ReadAllTextAsync(scaffoldConfig);

    var scaffoldObject = deserializer.Deserialize<Dictionary<dynamic, dynamic>>(yamlContent);

    var builtCommand = CommandStringBuilder.Init(scaffoldObject).Build();

    Console.WriteLine(builtCommand);

    var outPut = RunCommand(builtCommand, true);

    Console.WriteLine("########## Scaffolding Result #############");
    Console.WriteLine(outPut);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


static string RunCommand(string arguments, bool readOutput)
{
    var output = string.Empty;
    try
    {
        var startInfo = new ProcessStartInfo
        {
            Verb = "runas",
            FileName = "cmd.exe",
            Arguments = "/C " + arguments,
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = false
        };

        var proc = Process.Start(startInfo);

        if (readOutput)
        {
            output = proc.StandardOutput.ReadToEnd();
        }

        proc.WaitForExit(60000);

        return output;
    }
    catch (Exception)
    {
        return output;
    }
}

