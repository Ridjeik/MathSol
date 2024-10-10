using MathSol.Interpreter.Shared.Tokens;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class DivisionTokenReader : OperatorTokenReader<DivisionToken>
{
    public override char OperationChar => '/';
}

