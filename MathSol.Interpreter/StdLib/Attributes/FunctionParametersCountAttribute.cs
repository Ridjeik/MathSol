namespace MathSol.Interpreter.StdLib.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class FunctionParametersCountAttribute(int parametersCount) : Attribute
{
    public int ParametersCount { get; } = parametersCount;
}
