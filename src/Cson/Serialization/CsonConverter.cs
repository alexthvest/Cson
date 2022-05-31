using Cson.Syntax;

namespace Cson.Serialization;

public abstract class CsonConverter
{
    internal abstract Type ValueType { get; }
    
    internal abstract Type SyntaxNodeType { get; }
    
    public abstract bool CanConvert(Type typeToConvert);

    public abstract object Deserialize(CsonSyntaxNode syntax, Type typeToConvert);
}