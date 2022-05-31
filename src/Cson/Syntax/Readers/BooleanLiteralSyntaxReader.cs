using System.Diagnostics.CodeAnalysis;
using Cson.Syntax.Nodes;

namespace Cson.Syntax.Readers;

internal class BooleanLiteralSyntaxReader : CsonSyntaxReader<LiteralSyntax<bool>>
{
    public override bool TryRead(ref Utf8CsonReader reader, [NotNullWhen(true)] out LiteralSyntax<bool>? syntax)
    {
        if (reader.TryRead(out IdentifierSyntax? identifier))
        {
            bool? literal = identifier.Value switch
            {
                "true" => true,
                "false" => false,
                _ => null
            };

            if (literal.HasValue)
            {
                syntax = new LiteralSyntax<bool>(literal.Value);
                return true;
            }

            throw new Exception("Expected boolean literal (true or false)");
        }

        syntax = null;
        return false;
    }
}
