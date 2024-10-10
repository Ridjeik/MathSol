using MathSol.Interpreter.Shared.Tokens;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class PlusTokenReader : OperatorTokenReader<PlusToken>
{
    public override char OperationChar => '+';
}
