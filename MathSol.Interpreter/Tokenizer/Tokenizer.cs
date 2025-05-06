using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using System.Collections.Immutable;
using System.Reflection;

namespace MathSol.Interpreter.Tokenizer;

internal class Tokenizer(IEnumerable<ITokenReader> tokenReaders) : ITokenizer
{
    private readonly ImmutableArray<ITokenReader> TokenReaders = [.. tokenReaders.OrderByDescending(t => t.GetType().GetCustomAttribute<TokenReaderPriorityAttribute>()?.Priority ?? 0)];

    public IEnumerable<IToken> Tokenize(CodeFile codeFile)
    {
        while (!codeFile.IsEnded)
        {   
            yield return ReadTokenFromCodeFile(codeFile);
        }

        yield return new EndOfFileToken();
    }

    private IToken ReadTokenFromCodeFile(CodeFile codeFile)
    {
        foreach(var reader in TokenReaders)
        {
            var token = reader.TryGetTokenFromCodeFile(codeFile);
            if (token != null)
            {
                return token;
            }
        }

        throw new InvalidOperationException($"Unexpected token \"{codeFile.CurrentChar}\" in file {codeFile.FileName}");
    }

}
