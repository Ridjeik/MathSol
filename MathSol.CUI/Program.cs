using MathSol.Interpreter;
using MathSol.Interpreter.FileSystem;

Interpreter interpreter = new Interpreter();
interpreter.Interpret(CodeFile.FromFile("test.msl"));

/*
string? input = "";
Interpreter interpreter = new Interpreter();
List<string> codeLines = new();
do
{
    Console.Write(">>> ");
    input = input + Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input == "exit")
    {
        break;
    }

    if (input.EndsWith("\\"))
    {
        input = input.Substring(0, input.Length - 1);
        continue;
    }

    var instructions = string.Join(Environment.NewLine, codeLines.Append(input));

    var code = CodeFile.FromCode(instructions);
    try {
        interpreter.Interpret(code);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        input = "";
        continue;
    }
    codeLines.Add(input.Replace("print", "nop"));
    input = "";

} while (true);
*/