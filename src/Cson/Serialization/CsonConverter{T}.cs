using Cson.Syntax;

namespace Cson.Serialization;

public abstract class CsonConverter<TValue, TSyntaxNode> : CsonConverter
    where TSyntaxNode : CsonSyntaxNode
{
    internal sealed override Type ValueType { get; } = typeof(TValue);

    internal sealed override Type SyntaxNodeType { get; } = typeof(TSyntaxNode);

    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(TValue).IsAssignableFrom(typeToConvert);
    }

    public sealed override object Deserialize(CsonSyntaxNode syntax, Type typeToConvert)
    {
        if (syntax is TSyntaxNode genericSyntax)
        {
            return Deserialize(genericSyntax, typeToConvert)!;
        }

        throw new Exception($"{syntax.GetType().Name} is provider, expected {typeof(TSyntaxNode)}");
    }

    public abstract TValue Deserialize(TSyntaxNode syntax, Type typeToConvert);
}
