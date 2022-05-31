namespace Cson.Syntax.Nodes;

public class IdentifierSyntax : CsonSyntaxNode
{
    public IdentifierSyntax(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
