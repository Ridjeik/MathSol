namespace MathSol.Interpreter.Tokenizer;

[AttributeUsage(AttributeTargets.Class)]
internal class TokenReaderPriorityAttribute(int priority) : Attribute
{
    public int Priority { get; } = priority;
}
