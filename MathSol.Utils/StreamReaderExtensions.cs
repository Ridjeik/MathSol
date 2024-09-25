namespace MathSol.Utils;

public static class StreamReaderExtensions
{
    public static char? PeekAsChar(this StreamReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var result = reader.Peek();

        if (result <= -1)
            return null;

        return (char)result;
    }

    public static char? ReadAsChar(this StreamReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        if (reader.PeekAsChar() is null)
            return null;

        var result = reader.Read();

        return (char)result;
    }
}
