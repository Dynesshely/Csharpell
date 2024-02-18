using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Csharpell.Core;

public class CSharpScriptEngine
{
    private ScriptState<object>? _scriptState = null;

    private ScriptOptions _options = ScriptOptions.Default;

    public async Task<object?> ExecuteAsync(
        string? code,
        Func<ScriptOptions, ScriptOptions>? options = null,
        CancellationToken cancellationToken = default
    )
    {
        if (code is null)
            return null;

        _options = _options.WithImports("System", "System.Text", "System.Collections.Generic");

        var new_options = options?.Invoke(_options);

        if (new_options is not null) _options = new_options;

        if (_scriptState is null)
        {
            _scriptState = (ScriptState<object>)await CSharpScript.RunAsync(
                code,
                _options,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            _scriptState = (ScriptState<object>)await _scriptState.ContinueWithAsync(
                code,
                _options,
                cancellationToken: cancellationToken
            );
        }

        var result = _scriptState.ReturnValue;

        if (result is not null && !string.IsNullOrEmpty(result.ToString()))
            return result;

        return null;
    }
}
