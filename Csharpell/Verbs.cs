using CommandLine;

namespace Csharpell;

[Verb("console", isDefault: true, HelpText = "Enter C# REPL console.")]
public class ConsoleVerbOptions : Options
{
    [Option('e', "execute", Default = null, Required = false, HelpText = "Execute command from arguments.")]
    public string? Command { get; set; }
}

[Verb("run", HelpText = "Run C# file as script.")]
public class RunVerbOptions : Options
{
    [Option('p', "path", Default = "./Program.cs", Required = false, HelpText = "Path to C# file to run.")]
    public string? Path { get; set; }
}
