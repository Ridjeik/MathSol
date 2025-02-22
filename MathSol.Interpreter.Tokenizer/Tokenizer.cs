using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using System.Collections.Immutable;
using System.Reflection;

namespace MathSol.Interpreter.Tokenizer;

internal class Tokenizer(IEnumerable<ITokenReader> tokenReaders) : ITokenizer
{
    private readonly ImmutableArray<ITokenReader> TokenReaders = tokenReaders
            .OrderByDescending(t => t.GetType().GetCustomAttribute<TokenReaderPriorityAttribute>()?.Priority ?? 0)
            .ToImmutableArray();

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
        return TokenReaders
                .Select(reader => reader.TryGetTokenFromCodeFile(codeFile))
                .FirstOrDefault(t => t != null) ?? 
                throw new InvalidOperationException($"Unexpected token \"{codeFile.CurrentChar}\" in file {codeFile.FileName}"); ;
    }
}
