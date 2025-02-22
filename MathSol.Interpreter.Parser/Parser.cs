using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Parsers;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.StdLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser;

internal class Parser(IServiceProvider serviceProvider) : IParser
{
    private ProgramParser ProgramParser => serviceProvider.GetRequiredService<ProgramParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        var @namespace = new Namespace();
        var builtInFunctions = serviceProvider.GetRequiredService<IBuiltinFunctionImplementationFactory>().GetAllFunctionImplementations();
        builtInFunctions.ToList().ForEach(x => @namespace.AddIdentifierObject(new SubprogramNode(x.Name, x.Arguments.Select(arg => new VariableNode(arg)))));

        return ProgramParser.Parse(tokens, @namespace);
    }
}
