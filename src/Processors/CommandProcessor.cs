using System.Diagnostics;
using YamlScaffold.Cli.Output;

namespace YamlScaffold.Cli.Processors
{
    internal static class CommandProcessor
    {
        private const string ProcessVerb = "runas";
        private const string ProcessFileName = "cmd.exe";
        private const string ArgumentPrefix = "/C";

        internal static void Run(string arguments, bool verbosity)
        {
            ConsoleOutput.Log($"Process has been started with the arguments ({arguments})");

            var startInfo = GetProcessStartInfo(arguments);

            var proc = Process.Start(startInfo);

            if (proc == null)
            {
                throw new Exception("Scaffolding process could not be triggered!");
            }

            if (verbosity)
            {
                proc.OutputDataReceived += ProcOnOutputDataReceived;
            }
        }

        private static void ProcOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                ConsoleOutput.Log(e.Data);
            }
        }

        private static ProcessStartInfo GetProcessStartInfo(string arguments) =>
            new()
            {
                Verb = ProcessVerb,
                FileName = ProcessFileName,
                Arguments = $"{ArgumentPrefix} {arguments}",
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false
            };
    }
}
