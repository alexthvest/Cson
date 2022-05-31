namespace Cson;

internal static class CsonConstants
{
    public const byte OpenParenthesis = (byte)'(';
    public const byte CloseParenthesis = (byte)')';
    public const byte OpenBracket = (byte)'[';
    public const byte CloseBracket = (byte)']';
    public const byte OpenBrace = (byte)'{';
    public const byte CloseBrace = (byte)'}';
    
    public const byte Plus = (byte)'+';
    public const byte Hyphen = (byte)'-';
    public const byte Period = (byte)'.';
    public const byte Colon = (byte)':';
    public const byte Comma = (byte)',';
    public const byte Underscore = (byte)'_';
    public const byte DoubleQuote = (byte)'"';
    public const byte BackSlash = (byte)'\\';
    public const byte Slash = (byte)'/';

    public const byte Space = (byte)' ';
    public const byte Tab = (byte)'\t';
    public const byte CarriageReturn = (byte)'\r';
    public const byte LineFeed = (byte)'\n';

    public static ReadOnlySpan<byte> Null => new[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };

    public static ReadOnlySpan<byte> True => new[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };

    public static ReadOnlySpan<byte> False => new[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };

    public static byte[] Operators => new[]
    {
        OpenParenthesis, CloseParenthesis,
        OpenBracket, CloseBracket,
        OpenBrace, CloseBrace,
        Colon, Comma,
    };

    public static ReadOnlySpan<byte> EscapableChars => new[]
    {
        DoubleQuote, Slash, (byte)'n',
        (byte)'r', (byte)'t', (byte)'u',
        (byte)'b', (byte)'f'
    };
}
