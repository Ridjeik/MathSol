using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Maxima;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("test")]
[FunctionParametersCount(0)]
class TestProcedure(MaximaProcess maximaProcess, ITokenizer tokenizer, IParser parser) : MaximaAdaptedFunction(maximaProcess, tokenizer, parser)
{
    public override IEnumerable<string> Arguments => [];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        return ExecuteInMaxima("diff(sin(x)+cos(x), x)");
    }
}
