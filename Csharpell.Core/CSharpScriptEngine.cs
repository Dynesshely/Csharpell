using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Csharpell.Core;

public class CSharpScriptEngine
{
    private Script<object>? _script;

    private ScriptState<object>? _scriptState;

    private ScriptOptions _options = ScriptOptions.Default;

    public async Task<object?> ExecuteAsync(
        string? code,
        Func<ScriptOptions, ScriptOptions>? options = null,
        bool addDefaultImports = true,
        bool runInReplMode = true,
        CancellationToken cancellationToken = default
    )
    {
        if (code is null)
            return null;

        if (addDefaultImports)
            _options = _options.WithImports([
                "System",
                "System.Collections.Generic",
                "System.IO",
                "System.Linq",
                "System.Net.Http",
                "System.Text",
                "System.Threading",
                "System.Threading.Tasks"
            ]);

        var new_options = options?.Invoke(_options);

        if (new_options is not null) _options = new_options;

        if (_script is null)
        {
            _script = CSharpScript.Create(code, _options);

            _ = _script.Compile(cancellationToken);

            _scriptState = await _script.RunAsync(cancellationToken: cancellationToken);
        }
        else
        {
            if (runInReplMode)
            {
                _scriptState = await _scriptState!.ContinueWithAsync(code, _options, cancellationToken);
            }
            else
            {
                _script = CSharpScript.Create(code, _options);

                _ = _script.Compile(cancellationToken);

                _scriptState = await _script.RunAsync(cancellationToken: cancellationToken);
            }
        }

        var result = _scriptState.ReturnValue;

        if (result is not null && !string.IsNullOrEmpty(result.ToString()))
            return result;

        return null;
    }
}
