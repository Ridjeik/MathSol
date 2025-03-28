using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Maxima;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.StdLib.Functions;

abstract class MaximaAdaptedFunction(MaximaProcess process, ITokenizer tokenizer, IParser parser) : FunctionImplementation
{
    protected IAstNode ExecuteInMaxima(string input)
    {
        var result = process.Execute(input);
        result = this.AdaptToMathSol(result);
        var code = CodeFile.FromCode($"nop({result})");

        var tokens = tokenizer.Tokenize(code).GetEnumerator();
        tokens.MoveNext();

        var node = parser.Parse(tokens);
        return node is ProgramNode programNode ? programNode.Statements.Single() : node;
    }

    private string AdaptToMathSol(string result)
    {
        return result
            .Replace('[', '{')
            .Replace(']', '}');
    }
}
