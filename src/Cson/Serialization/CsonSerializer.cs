using System.Diagnostics;
using Cson.Syntax;

namespace Cson.Serialization;

public static class CsonSerializer
{
    public static TValue? Deserialize<TValue>(ReadOnlySpan<byte> cson, CsonSerializerOptions? options = default)
    {
        options ??= new CsonSerializerOptions();

        var valueType = typeof(TValue);
        var converters = options.GetConvertersFromType(valueType)
            .ToArray();

        if (converters.Length == 0)
        {
            throw new Exception($"No converter found for type {valueType}");
        }

        var reader = CreateUtf8CsonReader(cson, options);
        // if (!reader.TryRead(out var syntax))
        // {
        //     throw new Exception("Nothing to read");
        // }
        //
        // var syntaxType = syntax.GetType();

        CsonConverter? converter = null;
        CsonSyntaxNode? syntax = null;
        
        foreach (var csonConverter in converters)
        {
            if (reader.TryRead(csonConverter.SyntaxNodeType, out var csonSyntax))
            {
                converter = csonConverter;
                syntax = csonSyntax;
                break;
            }
        }
        
        if (converter is null)
        {
            var compatibleSyntaxes = converters.Select(c => c.SyntaxNodeType);
            throw new Exception($"Expected {string.Join(", ", compatibleSyntaxes)}");
        }
        
        Debug.Assert(syntax is not null);
        
        var value = (TValue?)converter.Deserialize(syntax, valueType);

        if (!reader.IsEmpty)
        {
            throw new Exception("Expected end of stream");
        }

        return value;
    }

    private static Utf8CsonReader CreateUtf8CsonReader(ReadOnlySpan<byte> buffer, CsonSerializerOptions options)
    {
        var readerOptions = new Utf8CsonReaderOptions();

        foreach (var syntaxReader in options.SyntaxReaders)
        {
            readerOptions.SyntaxReaders.Add(syntaxReader);
        }

        return new Utf8CsonReader(buffer, readerOptions);
    }
}
