using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Maxima;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("diff")]
[FunctionParametersCount(2)]
class DiffProcedure(MaximaProcess process, ITokenizer tokenizer, IParser parser) : MaximaAdaptedFunction(process, tokenizer, parser)
{
    public override IEnumerable<string> Arguments => ["f", "var"];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        return this.ExecuteInMaxima($"diff({astNodes[0]}, {astNodes[1]})");
    }
}
