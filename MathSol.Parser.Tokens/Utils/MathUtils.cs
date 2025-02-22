namespace MathSol.Interpreter.Shared.Utils;

public static class MathUtils
{
    public static long Gcd(long a, long b)
    {
        if (a == 0)
            return b;
        else if (b == 0)
            return a;
        if (a < b)
            return Gcd(a, b % a);
        else
            return Gcd(b, a % b);
    }
}
