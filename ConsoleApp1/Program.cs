using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Script<object> scripts = CSharpScript.Create("int x = 2 + 3;");
            scripts.Compile();
            ScriptState<object> state = scripts.RunAsync().Result;
            ScriptVariable variable = state.Variables[0];
            int x = (int)variable.Value;
        }
    }
}
