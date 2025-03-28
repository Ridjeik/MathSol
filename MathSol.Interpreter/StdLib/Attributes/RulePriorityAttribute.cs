namespace MathSol.Interpreter.StdLib.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class RulePriorityAttribute(int priority) : Attribute
{
    public int Priority { get; } = priority;
}
