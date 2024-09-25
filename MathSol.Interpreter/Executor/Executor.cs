using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Simplifiers;

namespace MathSol.Interpreter.Executor;

internal class Executor(IAstNode root)
{
    private IAstNode SyntaxTreeRoot { get; set; } = root;

    public void Execute()
    {
        if (SyntaxTreeRoot is ActionNode actionNode and { Action: "simplify" })
        {
            Simplify(actionNode.Operand);
        }
    }

    static readonly IEnumerable<INodeSimplificationRule> simplificationRules = [
        new UnaryPlusSimplifier(),
        new RemovePlusWithOneOperandRule(),
        new MergePlusNodesRule(),
        new MergeNumbersInAdditionRule(),
    ];

    private static void Simplify(IAstNode operand)
    {
        var isSimplified = false;
        do
        {
            isSimplified = false;

            foreach (var rule in simplificationRules)
            {
                var newOperand = operand.SimplifyByRule(rule);

                if (!newOperand.Equals(operand))
                {
                    isSimplified = true;
                    operand = newOperand;
                }
            }

        } while (isSimplified);


        //Console.WriteLine(operand);
    }
}
