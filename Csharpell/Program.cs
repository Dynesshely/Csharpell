
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

            var keepRunning = true;

            while (keepRunning)
            {
                Console.Write(">>> ");

                var line = Console.ReadLine() ?? "";

                if (line.ToLower().Equals("exit"))
                {
                    keepRunning = false;
                }
                else
                {
                    try
                    {
                        var result = CSharpScriptEngine.Execute(line);

                        // Check if result is end with new line.
                        if (result?.ToString()?.EndsWith(Environment.NewLine) ?? true)
                        {
                            Console.Write(result);
                        }
                        else
                        {
                            Console.WriteLine(result);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }
        }
        else
        {
            Console.WriteLine($"Executing file{verbose}: \"{options?.Path}\"");

            // TODO: Execute file.
        }
    })
    .WithNotParsed(errs => Console.WriteLine("Failed to parse arguments."))
    ;
