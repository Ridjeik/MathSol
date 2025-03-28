using System.Text;

namespace MathSol.Interpreter.FileSystem;

public class CodeFile
{
    private CodeFile()
    {

    }

    private int position = 0;

    private string Code { get; init; }
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
    public string RestOfCode => Code[Position..];
    public string FileName { get; init; }

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

    public string ConsumeUntil(Predicate<char> predicate)
    {
        SkipWhitespaces();

        var result = new StringBuilder();
        while (CurrentChar is char c && predicate(c))
        {
            result.Append(c);
            Position++;
        }

        return result.ToString();
    }

    private void SkipWhitespaces()
    {
        while (CurrentChar is char c && char.IsWhiteSpace(c))
        {
            Position++;
        }
    }

    public static CodeFile FromFile(string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);
        return new CodeFile()
        {
            Code = File.ReadAllText(fileName),
            FileName = fileName,
        };
    }

    public static CodeFile FromCode(string code)
    {
        ArgumentNullException.ThrowIfNull(code);
        return new CodeFile()
        {
            Code = code,
            FileName = "Code snippet",
        };
    }
}