// This project is a shell to execute csharp code immediately like python.
// You can use top-level codes with this tool to write pythonike codes.

using System.Reflection;
using CommandLine;
using Csharpell;
using Csharpell.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;

Parser.Default.ParseArguments<Options, ConsoleVerbOptions, RunVerbOptions>(args)
    .WithParsed<Options>(options =>
    {

    })
    .WithParsed<ConsoleVerbOptions>(async options =>
    {
        var verbose = options.Verbose ? " (Verbose)" : "";

        var keepRunning = true;

        var engine = new CSharpScriptEngine();

        await engine.ExecuteAsync("", addDefaultImports: false, runInReplMode: true);

        if (options.Command is not null)
        {
            try
            {
                var result = (await engine.ExecuteAsync(
                    options.Command,
                    options => options
                        .WithReferences(Assembly.GetExecutingAssembly())
                        .WithLanguageVersion(LanguageVersion.Preview),
                    addDefaultImports: true,
                    runInReplMode: false
                ))?.ToString();

                if (string.IsNullOrWhiteSpace(result) == false)
                    Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return;
        }

        Console.WriteLine($"Entered C# REPL console.{verbose}");

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
                    var result = await engine.ExecuteAsync(
                        line,
                        options => options
                            .WithReferences(Assembly.GetExecutingAssembly())
                            .WithLanguageVersion(LanguageVersion.Preview),
                        addDefaultImports: true,
                        runInReplMode: true
                    );

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
    })
    .WithParsed<RunVerbOptions>(options =>
    {
        var verbose = options.Verbose ? " (Verbose)" : "";

        Console.WriteLine($"Executing file{verbose}: \"{options?.Path}\"");

        if (File.Exists(options?.Path))
        {
            try
            {
                var content = File.ReadAllText(options?.Path ?? "");

                var engine = new CSharpScriptEngine();

                var result = engine.ExecuteAsync(content, runInReplMode: false);

                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.WriteLine(e.StackTrace);
            }
        }
        else
        {
            Console.WriteLine($"File not found: \"{options?.Path}\"");
        }
    })
    ;
