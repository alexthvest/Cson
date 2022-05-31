using System.Diagnostics.CodeAnalysis;
using Cson.Syntax.Nodes;

namespace Cson.Syntax.Readers;

internal class TypeSyntaxReader : CsonSyntaxReader<TypeSyntax>
{
    public override bool TryRead(ref Utf8CsonReader reader, [NotNullWhen(true)] out TypeSyntax? syntax)
    {
        // Read optional type name
        reader.TryRead(out IdentifierSyntax? name);

        if (reader.Consume(skipWhitespace: true) != CsonConstants.OpenParenthesis)
        {
            syntax = null;
            return false;
        }

        var properties = new List<TypePropertySyntax>();

        while (!reader.IsEmpty)
        {
            if (reader.Peek(skipWhitespace: true) == CsonConstants.CloseParenthesis)
            {
                break;
            }

            var property = ReadProperty(ref reader);
            properties.Add(property);

            if (reader.Peek(skipWhitespace: true) != CsonConstants.Comma)
            {
                break;
            }

            reader.Advance();
        }

        if (reader.Consume(skipWhitespace: true) != CsonConstants.CloseParenthesis)
        {
            throw new Exception("Expected end of type");
        }

        syntax = new TypeSyntax(name, properties);
        return true;
    }

    private TypePropertySyntax ReadProperty(ref Utf8CsonReader reader)
    {
        if (!reader.TryRead(out IdentifierSyntax? name))
        {
            throw new Exception("Expected property name");
        }

        if (reader.Consume(skipWhitespace: true) != CsonConstants.Colon)
        {
            throw new Exception("Expected property key-value separator");
        }

        if (!reader.TryRead(out var value))
        {
            throw new Exception("Expected property value");
        }

        return new TypePropertySyntax(name, value);
    }
}
