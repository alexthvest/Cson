using System.Diagnostics.CodeAnalysis;

namespace Cson.Syntax;

public abstract class CsonSyntaxReader
{
    public abstract Type SyntaxNodeType { get; }
    
    public abstract bool TryRead(ref Utf8CsonReader reader, [NotNullWhen(true)] out CsonSyntaxNode? syntax);
}
