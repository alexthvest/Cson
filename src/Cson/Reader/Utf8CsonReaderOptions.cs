using Cson.Syntax;
using Cson.Syntax.Readers;

namespace Cson;

public class Utf8CsonReaderOptions
{
    private readonly List<CsonSyntaxReader> _defaultSyntaxReaders = new();

    public Utf8CsonReaderOptions()
    {
        _defaultSyntaxReaders.Add(new TypeSyntaxReader());

        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<byte>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<sbyte>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<short>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<ushort>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<int>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<uint>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<long>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<ulong>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<float>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<double>());
        _defaultSyntaxReaders.Add(new NumberLiteralSyntaxReader<decimal>());
        
        _defaultSyntaxReaders.Add(new BooleanLiteralSyntaxReader());
        _defaultSyntaxReaders.Add(new StringLiteralSyntaxReader());
        _defaultSyntaxReaders.Add(new IdentifierSyntaxReader());
    }

    public IList<CsonSyntaxReader> SyntaxReaders { get; } = new List<CsonSyntaxReader>();

    internal CsonSyntaxReader? GetReaderFromSyntax(Type syntaxType)
    {
        var syntaxReaders = GetSyntaxReaders();

        foreach (var syntaxReader in syntaxReaders)
        {
            if (syntaxReader.SyntaxNodeType == syntaxType)
            {
                return syntaxReader;
            }
        }

        return null;
    }

    internal IEnumerable<CsonSyntaxReader> GetSyntaxReaders()
    {
        foreach (var syntaxReader in SyntaxReaders)
        {
            yield return syntaxReader;
        }

        foreach (var defaultSyntaxReader in _defaultSyntaxReaders)
        {
            yield return defaultSyntaxReader;
        }
    }
}
