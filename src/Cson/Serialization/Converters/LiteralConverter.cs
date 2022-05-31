using Cson.Syntax.Nodes;

namespace Cson.Serialization.Converters;

internal class LiteralConverter<T> : CsonConverter<T, LiteralSyntax<T>>
{
    public override T Deserialize(LiteralSyntax<T> syntax, Type typeToConvert)
    {
        return syntax.Value;
    }
}
