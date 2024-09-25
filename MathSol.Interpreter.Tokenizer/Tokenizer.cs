using MathSol.Interpreter.Lib;
using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Parsers;

namespace MathSol.Interpreter.Tokenizer;

public class Tokenizer : ITokenizer
{
    private static readonly IEnumerable<ITokenReader> TokenParsers =
    [
        new KeywordTokenReader(),
        new PlusTokenReader(),
        new NumberTokenReader(),
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
