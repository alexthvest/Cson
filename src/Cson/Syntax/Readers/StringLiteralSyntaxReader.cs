using System.Diagnostics.CodeAnalysis;
using Cson.Syntax.Nodes;

namespace Cson.Syntax.Readers;

internal class StringLiteralSyntaxReader : CsonSyntaxReader<LiteralSyntax<string>>
{
    public override bool TryRead(ref Utf8CsonReader reader, [NotNullWhen(true)] out LiteralSyntax<string>? syntax)
    {
        if (reader.Consume() != CsonConstants.DoubleQuote)
        {
            syntax = null;
            return false;
        }

        var startPosition = reader.Position;
        var nextSymbolEscaped = false;

        for (; !reader.IsEmpty; reader.Advance())
        {
            var symbol = reader.Peek();

            if (symbol == CsonConstants.DoubleQuote)
            {
                if (!nextSymbolEscaped)
                {
                    reader.Advance();

                    var valueSpan = reader.Slice(startPosition, reader.Position - startPosition - 1);
                    var value = CsonHelpers.TranscodeUtf8String(valueSpan);

                    syntax = new LiteralSyntax<string>(value);
                    return true;
                }

                nextSymbolEscaped = false;
            }
            else if (symbol == CsonConstants.BackSlash)
            {
                nextSymbolEscaped = !nextSymbolEscaped;
            }
            else if (nextSymbolEscaped)
            {
                if (!CsonConstants.EscapableChars.Contains(symbol))
                {
                    throw new Exception("Invalid character after escape within string");
                }

                nextSymbolEscaped = false;
            }
            else if (symbol < CsonConstants.Space)
            {
                throw new Exception("Invalid character within string");
            }
        }

        throw new Exception("End of string not found");
    }
}
