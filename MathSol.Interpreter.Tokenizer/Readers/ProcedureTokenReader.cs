using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.StdLib;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class ProcedureTokenReader (IProcedureImplementationFactory procedureImplementationFactory) : ITokenReader
{
    private IEnumerable<string> Functions => procedureImplementationFactory.GetAllProcedureNames();

    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        foreach (var word in Functions)
        {
            string? topChars;
            try
            {
                topChars = codeFile.PeekChars(word.Length);
            }
            catch (EndOfStreamException)
            {
                continue;
            }

            if (topChars.Equals(word, StringComparison.InvariantCultureIgnoreCase))
            {
                codeFile.ConsumeChars(word.Length);
                return new ProcedureToken(word);
            }
        }

        return null;
    }
}
