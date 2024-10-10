using MathSol.Interpreter.StdLib.Enums;

namespace MathSol.Interpreter.StdLib.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class RuleTypeAttribute(RuleType ruleType) : Attribute
{
    public RuleType RuleType { get; } = ruleType;
}
