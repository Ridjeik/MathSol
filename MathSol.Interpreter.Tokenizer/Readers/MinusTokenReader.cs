using MathSol.Interpreter.Shared.Tokens;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class MinusTokenReader : OperatorTokenReader<MinusToken>
{
    public override char OperationChar => '-';
}