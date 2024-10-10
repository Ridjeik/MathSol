using MathSol.Interpreter.Shared.Tokens;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class MultiplicationTokenReader : OperatorTokenReader<MultiplicationToken>
{
    public override char OperationChar => '*';
}
