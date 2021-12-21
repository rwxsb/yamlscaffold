// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Linq;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using yamlscaffold.CLI;



var scaffoldConfig = args.AsQueryable().FirstOrDefault();

if ( scaffoldConfig == null )
{
    Console.WriteLine("No such file found in the working directory.");
    scaffoldConfig = Console.ReadLine()!.Trim();
}

try
{
    var deserializer = new DeserializerBuilder()
    .WithNamingConvention(HyphenatedNamingConvention.Instance)
    .Build();

    var scaffoldObject = deserializer.Deserialize<Dictionary<dynamic,dynamic>>(File.ReadAllText(scaffoldConfig));

    var builtCommand = BuildCommandString(scaffoldObject);

    Console.WriteLine(builtCommand);

    var outPut = RunCommand(builtCommand, true);

    Console.WriteLine("########## Scaffolding Result #############");
    Console.WriteLine(outPut);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}



static string BuildCommandString(Dictionary<dynamic, dynamic> scaffoldOption)
{
    var command = $"dotnet ef dbcontext scaffold ";
    var specialArguments = new Dictionary<string, string> { { "force", "-f" } };
    var optionlessArguments = new List<string> { "connection-string", "dependencies" };
    var listArguments = new List<string> { "table" };

    foreach (var option in scaffoldOption)
    {
        if (option.Value == null)
        {
            continue;
        }
        else
        {
            if (optionlessArguments.Contains(option.Key))
            {
                command = command + option.Value + " ";
            }
            else if (listArguments.Contains(option.Key))
            {
                var TablesList = (List<object>)option.Value;
                command = command + "--" + option.Key + " " + TablesList.GetItemsAsString<object>(" ,") + " ";
            }
            else if(specialArguments.ContainsKey(option.Key))
            {
                bool.TryParse(option.Value,out bool isOptionSet);
                if(isOptionSet)
                {
                    specialArguments.TryGetValue(option.Key,out string specialArg);
                    command += specialArg + " ";
                }
            }
            else
            {

                command = command + "--" + option.Key + " " + option.Value + " ";
            }
        }
    }

    return command;
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

