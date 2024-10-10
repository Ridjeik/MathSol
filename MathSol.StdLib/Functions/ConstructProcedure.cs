using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Utils;

namespace MathSol.Interpreter.StdLib.Functions;

internal class ConstructProcedure : ProcedureImplementation
{
    public override string FunctionName => "construct";

    public override int NumberOfOperands => 2;

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
            _ => throw new InvalidOperationException($"Operator {@operator} is not supported"),
        };
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
