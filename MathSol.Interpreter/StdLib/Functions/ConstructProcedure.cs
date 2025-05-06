using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Utils;
using MathSol.Interpreter.StdLib.Interfaces;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("construct")]
[FunctionParametersCount(2)]
internal class ConstructProcedure (IVariableScopeFactory variables) : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["operator", "operands"];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    { 
        if (astNodes[1] is not SetNode set)
        {
            throw new ArgumentException("Second argument must be a set");
        }

        string @operator;

        if (astNodes[0] is CharNode { Char: var @char })
        {
            @operator = @char.ToString();
        }
        else if (astNodes[0] is StringNode { String: var op })
        {
            @operator = op;
        }
        else
        {
            throw new ArgumentException("First argument must be a character or string");
        }

        return @operator switch
        {
            "+" => new AdditionNode([.. set]),
            "*" => new MultiplicationNode([.. set]),
            "-" => GetSubtractionNode(set),
            "/" => GetDivisionNode(set),
            "^" => GetExponentNode(set),
            "=" => GetEqualityNode(set),
            _ => TryGetFunctionNode(@operator, set),
        };
    }

    private static EqualityNode GetEqualityNode(SetNode set)
    {
        FunctionsUtil.EnsureSet(set, 2);
        return new EqualityNode(set.First(), set.Last());
    }

    private IAstNode TryGetFunctionNode(string @operator, SetNode set)
    {
        var def = variables.GetCurrentScope().GetVariable(new VariableNode(@operator));
        if (def is SubProgramDefinitionNode)
        {
            return new SubprogramCallNode(@operator, set.Operands.ToArray());
        }
        else if(def is FunctionDeclarationNode)
        {
            return new FunctionCallNode(@operator, set.Operands.ToArray());
        }
        if (def is VariableNode var)
        {
            var defReal = TryGetFunctionNode(var.Name, set);
            return defReal;
        }



        throw new InvalidOperationException($"Operator {@operator} is not supported");
    }

    private static ExponentNode GetExponentNode(SetNode set)
    {
        FunctionsUtil.EnsureSet(set, 2);
        return new ExponentNode(set.First(), set.Last());
    }

    private static DivisionNode GetDivisionNode(SetNode set)
    {
        FunctionsUtil.EnsureSet(set, 2);
        return new DivisionNode(set.First(), set.Last());
    }

    private static SubtractionNode GetSubtractionNode(SetNode set)
    {
        FunctionsUtil.EnsureSet(set, 2);
        return new SubtractionNode(set.First(), set.Last());
    }
}
