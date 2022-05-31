using System.Diagnostics.CodeAnalysis;
using Cson.Syntax.Nodes;

namespace Cson.Syntax.Readers;

internal class IdentifierSyntaxReader : CsonSyntaxReader<IdentifierSyntax>
{
    public override bool TryRead(ref Utf8CsonReader reader, [NotNullWhen(true)] out IdentifierSyntax? syntax)
    {
        var symbol = reader.Peek();

        if (!CsonHelpers.IsLetter(symbol) && symbol != CsonConstants.Underscore)
        {
            syntax = null;
            return false;
        }

        var startPosition = reader.Position;

        for (; !reader.IsEmpty; reader.Advance())
        {
            symbol = reader.Peek();
            
            if (!CsonHelpers.IsLetterOrDigit(symbol) && symbol != CsonConstants.Underscore)
            {
                break;
            }
        }

        var valueSpan = reader.Slice(startPosition, reader.Position - startPosition);
        var value = CsonHelpers.TranscodeUtf8String(valueSpan);

        syntax = new IdentifierSyntax(value);
        return true;
    }
}
