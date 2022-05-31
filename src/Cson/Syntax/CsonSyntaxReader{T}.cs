using System.Diagnostics.CodeAnalysis;

namespace Cson.Syntax;

public abstract class CsonSyntaxReader<TSyntaxNode> : CsonSyntaxReader
    where TSyntaxNode : notnull, CsonSyntaxNode
{
    public sealed override Type SyntaxNodeType { get; } = typeof(TSyntaxNode);

    public sealed override bool TryRead(ref Utf8CsonReader reader, [NotNullWhen(true)] out CsonSyntaxNode? syntax)
    {
        var success = TryRead(ref reader, out var genericSyntax);
        syntax = genericSyntax;
        return success;
    }

    public abstract bool TryRead(ref Utf8CsonReader reader, [NotNullWhen(true)] out TSyntaxNode? syntax);
}
