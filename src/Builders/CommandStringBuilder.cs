using System.Text;
using YamlScaffold.Cli.Models;

namespace YamlScaffold.Cli.Builders
{
    internal class CommandStringBuilder
    {
        private const string CommandStringPrefix = "dotnet ef dbcontext scaffold";

        private static readonly IReadOnlyCollection<string> MandatoryArguments = new[] { "connection-string", "dependencies" };
        private static readonly IReadOnlyCollection<ArgumentDetail> FlagArguments = new ArgumentDetail[] { new("force", "-f") };
        private static readonly IReadOnlyCollection<ArgumentDetail> MultipleValueArguments = new ArgumentDetail[] { new("table", "--table") };

        private readonly Dictionary<dynamic, dynamic> _options;

        private CommandStringBuilder(Dictionary<dynamic, dynamic> options)
        {
            _options = options;
        }

        internal static CommandStringBuilder Init(Dictionary<dynamic, dynamic> options) => new(options);

        private class Builder
        {
            private readonly Dictionary<dynamic, dynamic> _options;
            private readonly StringBuilder _commandStringBuilder;

            internal Builder(Dictionary<dynamic, dynamic> options)
            {
                _options = options ?? throw new ArgumentNullException(nameof(options), "Yaml items could not be parsed!");
                _commandStringBuilder = new StringBuilder(CommandStringPrefix);
            }

            internal Builder WithFlagArguments()
            {
                foreach (var (argumentName, command) in FlagArguments)
                {
                    if (!_options.TryGetValue(argumentName, out var optionValue))
                    {
                        continue;
                    }

                    if (!bool.TryParse(optionValue, out bool isOptionSet) || !isOptionSet)
                    {
                        continue;
                    }

                    AppendWithSpace(command);
                }

                return this;
            }

            internal Builder WithMandatoryArguments()
            {
                foreach (var argument in MandatoryArguments)
                {
                    if (_options.TryGetValue(argument, out var value))
                    {
                        AppendWithSpace(value.ToString());
                    }
                }

                return this;
            }

            internal Builder WithMultipleValueArguments()
            {
                foreach (var (argumentName, command) in MultipleValueArguments)
                {
                    if (!_options.TryGetValue(argumentName, out var value) || value is not IEnumerable<object> values)
                    {
                        throw new Exception($"The values of {argumentName} can not be empty!");
                    }

                    var stringValues = string.Join(',', values);

                    AppendWithSpace($"{command} {stringValues}");
                }

                return this;
            }

            private void AppendWithSpace(string value)
            {
                _commandStringBuilder.Append(' ');
                _commandStringBuilder.Append(value);
            }

            internal string Build() => _commandStringBuilder.ToString();
        }

        internal string Build() =>
            new Builder(_options)
                .WithMandatoryArguments()
                .WithMultipleValueArguments()
                .WithFlagArguments()
                .Build();
    }
}
