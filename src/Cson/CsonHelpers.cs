using System.Buffers.Text;
using System.Text;

namespace Cson;

internal static class CsonHelpers
{
    public static bool IsLetter(byte symbol) => (symbol >= 'A' && symbol <= 'Z') || (symbol >= 'a' && symbol <= 'z');

    public static bool IsDigit(byte symbol) => (uint)(symbol - '0') <= '9' - '0';

    public static bool IsLetterOrDigit(byte symbol) => IsLetter(symbol) || IsDigit(symbol);

    public static bool IsWhitespace(byte symbol) =>
        symbol == CsonConstants.Space ||
        symbol == CsonConstants.Tab ||
        symbol == CsonConstants.LineFeed ||
        symbol == CsonConstants.CarriageReturn;

    public static string TranscodeUtf8String(ReadOnlySpan<byte> utf8Unescaped)
    {
        try
        {
            if (utf8Unescaped.IsEmpty)
            {
                return string.Empty;
            }

            unsafe
            {
                fixed (byte* bytePtr = utf8Unescaped)
                {
                    return Encoding.UTF8.GetString(bytePtr, utf8Unescaped.Length);
                }
            }
        }
        catch (DecoderFallbackException)
        {
            throw new Exception("Failed to decode utf8 string");
        }
    }

    public static bool TryParseNumber<TNumber>(ReadOnlySpan<byte> valueSpan, out TNumber value)
        where TNumber : struct
    {
        var numberType = typeof(TNumber);

        if (numberType == typeof(int))
        {
            if (Utf8Parser.TryParse(valueSpan, out int int32Value, out var bytesConsumed) &&
                valueSpan.Length == bytesConsumed)
            {
                value = (TNumber)Convert.ChangeType(int32Value, typeof(TNumber));
                return true;
            }
        }

        value = default;
        return false;
    }
}
