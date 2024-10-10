using MathSol.Interpreter.Rules.Enums;

namespace MathSol.Interpreter.Rules.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class RuleTypeAttribute(RuleType ruleType) : Attribute
{
    public RuleType RuleType { get; } = ruleType;
}
