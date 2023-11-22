using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StringBuilder code = new StringBuilder();
            using (FileStream fileSteream = File.Open("file.txt", FileMode.Open))
            {
                using (StreamReader streamReader = new StreamReader(fileSteream))
                {
                    string line = "";
                    do 
                    {
                        line = streamReader.ReadLine();
                    } while (streamReader.ReadLine() != null);

                    string[] fields = line.Split(',');

                    for (int i = 0; i < fields.Length; i++)
                    {
                        code.AppendLine($@"int {fields[i]} = {i};");
                    }

                    code.AppendLine(Environment.NewLine);
                    var expr = string.Join(" + ", fields);
                    code.Append($"int result = {expr};");
                }
            }

            Script<object> script = CSharpScript.Create(code.ToString());
            ScriptState<object> state = script.RunAsync().Result;
            int result = (int)state.Variables.First(x => x.Name == "result").Value;

            Console.ReadKey();
        }
    }

    public static class Source
    {
        public static string Code = "System.Console.WriteLine(\"Hello world\")";

        public static string AnotherCode = $@"";
    }
}
