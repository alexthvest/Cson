namespace
           Cson.Syntax.Nodes;

public class TypePropertySyntax : CsonSyntaxNode
{
    public TypePropertySyntax(IdentifierSyntax name, CsonSyntaxNode value)
    {
        Name = name;
        Value = value;
    }

    public IdentifierSyntax Name { get; }
    
    public CsonSyntaxNode Value { get; }
}
