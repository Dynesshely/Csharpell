
// This project is a shell to execute csharp code immediately like python.
// You can use top-level codes with this tool to write pythonike codes.

using CommandLine;

Parser.Default.ParseArguments<Options, Verbs.RunVerbOptions>(args)
    .WithParsed<Options>(options =>
    {

    })
    .WithParsed<Verbs.RunVerbOptions>(options =>
    {
        var verbose = options.Verbose ? " (Verbose)" : "";

        if (options.Interactive)
        {
            Console.WriteLine($"Entering interactive mode.{verbose}");

            // TODO: Shell environment.
        }
        else
        {
            Console.WriteLine($"Executing file{verbose}: \"{options?.Path}\"");

            // TODO: Execute file.
        }
    })
    .WithNotParsed(errs => Console.WriteLine("Failed to parse arguments."))
    ;
