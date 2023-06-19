using CommandLine;

public class Verbs
{
    [Verb("run", isDefault: true, HelpText = "Run C# file as script.")]
    public class RunVerbOptions: Options
    {
        [Option('p', "path", Default = "./Program.cs", Required = false, HelpText = "Path to C# file to run.")]
        public string? Path { get; set; }

        [Option('i', "interactive", Default = false, Required = false, HelpText = "Run with interactive mode.")]
        public bool Interactive { get; set; }
    }
}
