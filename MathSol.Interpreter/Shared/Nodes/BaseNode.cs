using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public abstract class BaseNode : IAstNode
{
    public virtual bool Equals(IAstNode? other)
    {
        if (other == null)
            return false;

        if (GetType() != other.GetType())
            return false;

        if (this is IOperatorAstNode operandNode && other is IOperatorAstNode otherOperandNode)
        {
            return operandNode.Operands.SequenceEqual(otherOperandNode.Operands);
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not IAstNode otherNode)
            return false;

        return Equals(otherNode);
    }

    public override int GetHashCode()
    {
        int hash = GetType().GetHashCode();

        if (this is IOperatorAstNode operandNode)
        {
            foreach (var operand in operandNode.Operands)
            {
                hash = hash * 31 + operand.GetHashCode();
            }
        }

        return hash;
    }

    public abstract override string ToString();
}
