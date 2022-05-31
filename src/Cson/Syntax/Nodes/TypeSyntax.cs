using System.Collections.Immutable;

namespace Cson.Syntax.Nodes;

public class TypeSyntax : CsonSyntaxNode
{
    public TypeSyntax(IdentifierSyntax? name, IEnumerable<TypePropertySyntax> properties)
    {
        Name = name;
        Properties = properties.ToImmutableArray();
    }

    public IdentifierSyntax? Name { get; }
    
    public ImmutableArray<TypePropertySyntax> Properties { get; }
}
