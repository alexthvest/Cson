using Cson.Syntax;

namespace Cson.Serialization;

public abstract class CsonConverterFactory : CsonConverter
{
    internal sealed override Type ValueType => throw new InvalidOperationException();

    internal sealed override Type SyntaxNodeType => throw new InvalidOperationException();

    public sealed override object Deserialize(CsonSyntaxNode syntax, Type typeToConvert)
    {
        throw new InvalidCastException();
    }

    internal CsonConverter CreateConverterCore(Type typeToConvert)
    {
        var converter = CreateConverter(typeToConvert);

        switch (converter)
        {
            case null:
                throw new Exception("Converter must not be null");
            case CsonConverterFactory:
                throw new Exception("Converter must not be a factory");
        }

        return converter;
    }

    public abstract CsonConverter CreateConverter(Type typeToConvert);
}
