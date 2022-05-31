using Cson.Syntax.Nodes;

namespace Cson.Serialization.Converters;

internal class EnumConverter<TEnum> : CsonConverter<TEnum, IdentifierSyntax>
    where TEnum : struct
{
    public override TEnum Deserialize(IdentifierSyntax syntax, Type typeToConvert)
    {
        if (Enum.TryParse<TEnum>(syntax.Value, out var value))
        {
            return value;
        }

        throw new Exception($@"Can't convert ""{syntax.Value}"" to {typeof(TEnum)}");
    }
}
