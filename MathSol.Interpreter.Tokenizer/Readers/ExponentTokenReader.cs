using MathSol.Interpreter.Shared.Tokens;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class ExponentTokenReader : OperatorTokenReader<ExponentToken>
{
    public override char OperationChar => '^';
}
