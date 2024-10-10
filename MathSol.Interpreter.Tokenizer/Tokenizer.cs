using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer;

internal class Tokenizer(IEnumerable<ITokenReader> tokenReaders) : ITokenizer
{
    private IEnumerable<ITokenReader> TokenReaders { get; } = tokenReaders;

    public IEnumerable<IToken> Tokenize(CodeFile codeFile)
    {
        IList<IToken> result = [];

        while (!codeFile.IsEnded)
        {
            foreach (ITokenReader tokenParser in TokenReaders)
            {
                var token = tokenParser.TryGetTokenFromCodeFile(codeFile);
                if (token != null)
                {
                    result.Add(token);
                    break;
                }
             }
        }

        return result;
    }
}
