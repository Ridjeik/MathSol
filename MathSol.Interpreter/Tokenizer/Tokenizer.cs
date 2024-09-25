using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Readers;
using MathSol.Interpreter.Tokenizer.Tokens;

namespace MathSol.Interpreter.Tokenizer;

internal class Tokenizer : ITokenizer
{
    private static readonly IEnumerable<ITokenReader> TokenParsers =
    [
        new KeywordTokenReader(),
        new PlusTokenReader(),
        new NumberTokenReader(),
        new ParenthesesTokensReader(),
    ];

    public IEnumerable<IToken> Tokenize(CodeFile codeFile)
    {
        IList<IToken> result = [];

        while (!codeFile.IsEnded)
        {
            foreach (ITokenReader tokenParser in TokenParsers)
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
