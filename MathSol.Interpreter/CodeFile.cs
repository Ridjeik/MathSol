namespace MathSol.Interpreter;

public class CodeFile(string fileName)
{
    private int position = 0;

    private string Code { get; } = File.ReadAllText(fileName);
    private int Position
    {
        get { return position; }
        set
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, Code.Length);
            position = value;
        }
    }

    public bool IsEnded => Position >= Code.Length;
    public char? CurrentChar => IsEnded ? null : Code[Position];
    public int CharsLeft => Code.Length - Position;

    public char? PeekChar()
    {
        SkipWhitespaces();
        return CurrentChar;
    }

    public char? ConsumeChar()
    {
        SkipWhitespaces();
        var result = PeekChar();
        Position++;
        return result;
    }

    public string PeekChars(int charCount)
    {
        SkipWhitespaces();

        ArgumentOutOfRangeException.ThrowIfNegative(charCount);

        return new string(Code.Skip(Position).Take(charCount).ToArray());
    }

    public string ConsumeChars(int charCount)
    {
        var result = PeekChars(charCount);
        Position += result.Length;
        return result;
    }

    private void SkipWhitespaces()
    {
        while (CurrentChar is char c && char.IsWhiteSpace(c))
        {
            Position++;
        }
    }
}