using System.Diagnostics.CodeAnalysis;
using Cson.Syntax.Nodes;

namespace Cson.Syntax.Readers;

internal class NumberLiteralSyntaxReader<TNumber> : CsonSyntaxReader<LiteralSyntax<TNumber>>
    where TNumber : struct
{
    public override bool TryRead(ref Utf8CsonReader reader, [NotNullWhen(true)] out LiteralSyntax<TNumber>? syntax)
    {
        var startPosition = reader.Position;

        if (!TryReadNumber(ref reader))
        {
            syntax = null;
            return false;
        }

        var valueSpan = reader.Slice(startPosition, reader.Position - startPosition);

        if (!CsonHelpers.TryParseNumber(valueSpan, out TNumber value))
        {
            throw new Exception($"Could not convert value to {typeof(TNumber)}");
        }

        syntax = new LiteralSyntax<TNumber>(value);
        return true;
    }

    private bool TryReadNumber(ref Utf8CsonReader reader)
    {
        var symbol = reader.Peek();

        if (ReadSign(ref reader, symbol))
        {
            symbol = reader.Peek();
        }

        if (symbol == '0')
        {
            reader.Advance();
        }
        else
        {
            if (!ReadDigits(ref reader, symbol))
            {
                return false;
            }
        }

        symbol = reader.Peek();

        if (symbol == CsonConstants.Period)
        {
            var nextSymbol = reader.Peek(1);
            
            if (!CsonHelpers.IsDigit(nextSymbol))
            {
                return true;
            }

            reader.Advance();
            symbol = nextSymbol;

            if (!ReadDigits(ref reader, symbol))
            {
                throw new Exception("Expected digits after period");
            }

            symbol = reader.Peek();
        }

        if (symbol == 'e' || symbol == 'E')
        {
            reader.Advance();
            symbol = reader.Peek();

            if (!ReadSign(ref reader, symbol))
            {
                throw new Exception("Expected sign after exp");
            }

            symbol = reader.Peek();

            if (!ReadDigits(ref reader, symbol))
            {
                throw new Exception("Expected exp digits");
            }
        }

        return true;
    }

    private bool ReadSign(ref Utf8CsonReader reader, byte symbol)
    {
        if (symbol == CsonConstants.Plus || symbol == CsonConstants.Hyphen)
        {
            reader.Advance();
            return true;
        }

        return false;
    }

    private bool ReadDigits(ref Utf8CsonReader reader, byte symbol)
    {
        if (!CsonHelpers.IsDigit(symbol))
        {
            return false;
        }

        for (; !reader.IsEmpty; reader.Advance())
        {
            symbol = reader.Peek();

            if (!CsonHelpers.IsDigit(symbol))
            {
                break;
            }
        }

        return true;
    }
}
