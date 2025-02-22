using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using System.Text;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("print_tree")]
[FunctionParametersCount(1)]
internal class PrintTreeProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression"];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        var sb = new StringBuilder();
        TraversePreOrder(astNodes[0], "", "", sb);
        Console.WriteLine(sb.ToString());
        return new UndefinedNode();
    }

    private static void TraversePreOrder(IAstNode node, string padding, string poineter, StringBuilder stringBuilder)
    {
        stringBuilder.Append(padding);
        stringBuilder.Append(poineter);
        stringBuilder.Append(node is IOperatorAstNode operatorNode ? operatorNode.Operator : node.ToString());
        stringBuilder.AppendLine();

        if (node is not IOperatorAstNode operatorAstNode)
        {
            return;
        }

        var paddingForBoth = padding + "|  ";
        const string pointerForMiddle = "├──";
        const string pointerForLast = "└──";

        var operands = operatorAstNode.Operands.ToList();
        operands.Take(operands.Count - 1).ToList().ForEach(child => TraversePreOrder(child, $"{paddingForBoth}", pointerForMiddle, stringBuilder));
        if (operands.Count > 0)
        {
            TraversePreOrder(operands.Last(), $"{paddingForBoth}", pointerForLast, stringBuilder);
        }
    }
}
