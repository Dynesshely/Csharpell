using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

public class CSharpScriptEngine
{
    private static ScriptState<object>? scriptState = null;

    public static object? Execute(string? code)
    {
        if (code is null)
        {
            throw new ArgumentNullException(nameof(code), "`code` string is null.");
        }

        scriptState = scriptState is null ? CSharpScript.RunAsync(code).Result : scriptState.ContinueWithAsync(code).Result;

        if (scriptState.ReturnValue is not null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
            return scriptState.ReturnValue;

        return null;
    }
}
