using System.Reflection;

namespace YamlScaffold.Cli.FileSystem
{
    internal static class FileSystemManager
    {
        private const int TimeoutValueInMinutes = 2;

        internal static async Task<string> ReadAllTextAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "File path is missing!");
            }

            var fileFullPath = Path.IsPathFullyQualified(filePath)
                ? filePath
                : Path.Combine(Environment.CurrentDirectory, filePath);

            if (!File.Exists(fileFullPath))
            {
                throw new FileNotFoundException($"File could not be found in that path! {fileFullPath}");
            }

            var timeoutCancellationToken = GetTimeoutCancellationToken();

            var content = await File.ReadAllTextAsync(fileFullPath, timeoutCancellationToken);

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content), "File content can not be empty!");
            }

            return content;
        }

        private static CancellationToken GetTimeoutCancellationToken()
        {
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(TimeoutValueInMinutes));

            return source.Token;
        }
    }
}
