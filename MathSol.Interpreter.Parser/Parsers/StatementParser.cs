using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace MathSol.Interpreter.Parser.Parsers;

internal class StatementParser(IServiceProvider serviceProvider) : IInternalParser
{
    private SubprogramCallParser ProcedureParser => serviceProvider.GetRequiredService<SubprogramCallParser>();
    private AssignmentParser AssignmentParser => serviceProvider.GetRequiredService<AssignmentParser>();
    private IfParser IfParser => serviceProvider.GetRequiredService<IfParser>();
    private ProgramParser ProgramParser => serviceProvider.GetRequiredService<ProgramParser>();
    private SubProgramDefinitionParser SubProgramDefinitionParser => serviceProvider.GetRequiredService<SubProgramDefinitionParser>();
    private EqualityParser EqualityParser => serviceProvider.GetRequiredService<EqualityParser>();
    private OperandParser OperandParser => serviceProvider.GetRequiredService<OperandParser>();
    private PlusMinusParser PlusMinusParser => serviceProvider.GetRequiredService<PlusMinusParser>();
    private FunctionParser FunctionParser => serviceProvider.GetRequiredService<FunctionParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {        
        if (tokens.Current is IdentifierToken identifierToken)
        {
            var existingObject = @namespace.GetIdentifierObject(identifierToken.Value);
            if (existingObject is null)
            {
                var operation = AssignmentParser.Parse(tokens, @namespace);
                if (operation is AssignmentNode assignment)
                {
                    @namespace.AddIdentifierObject(assignment.Left);
                    return assignment;
                }
                
                else if (operation is FunctionDeclarationNode functionDeclaration)
                {
                    @namespace.AddIdentifierObject(functionDeclaration.Function);
                    return functionDeclaration;
                }
                else
                {
                    throw new Exception("FATAL ERROR");
                }
            }
            else if(existingObject is SubprogramNode)
            {
                return ProcedureParser.Parse(tokens, @namespace);
            }
            else if(existingObject is FunctionNode)
            {
                return FunctionParser.Parse(tokens, @namespace);
            }
            else
            {
                return AssignmentParser.Parse(tokens, @namespace);
            }
        }

        if (tokens.Current is IfToken)
        {       
            return IfParser.Parse(tokens, @namespace);
        }

        if (tokens.Current is LeftCurlyBracketToken)
        {
            tokens.Skip<LeftCurlyBracketToken>();
            var block = ProgramParser.Parse(tokens, @namespace.Copy());
            tokens.Skip<RightCurlyBracketToken>();
            return block;
        }

        if (tokens.Current is DefineToken)
        {
            var def = SubProgramDefinitionParser.Parse(tokens, @namespace.Copy()) as SubProgramDefinitionNode ?? throw new Exception("FATAL ERROR");
            @namespace.AddIdentifierObject(def.SubprogramSignature); 
            return def;
        }

        if (tokens.Current is ReturnToken)
        {
            tokens.Skip<ReturnToken>();
            var value = PlusMinusParser.Parse(tokens, @namespace);
            return new ReturnNode(value);
        }

        throw new InvalidOperationException($"Unexpected token {tokens.Current}");
    }
}
