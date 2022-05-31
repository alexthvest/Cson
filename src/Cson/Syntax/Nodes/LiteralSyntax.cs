namespace Cson.Syntax.Nodes;

public class LiteralSyntax<T> : CsonSyntaxNode
{
    public LiteralSyntax(T value)
    {
        Value = value;
    }

    public T Value { get; }
}
