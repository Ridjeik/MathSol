namespace MathSol.Interpreter.StdLib.Attributes;

internal class FunctionNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
