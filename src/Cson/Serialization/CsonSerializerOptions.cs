using Cson.Serialization.Converters;
using Cson.Syntax;

namespace Cson.Serialization;

public class CsonSerializerOptions
{
    private readonly List<CsonConverter> _defaultConverters = new();
    private readonly List<CsonConverterFactory> _converterFactories = new();

    public CsonSerializerOptions()
    {
        _converterFactories.Add(new LiteralConverterFactory());
        _converterFactories.Add(new EnumConverterFactory());
    }

    public IList<CsonConverter> Converters { get; } = new List<CsonConverter>();

    public IList<CsonSyntaxReader> SyntaxReaders { get; } = new List<CsonSyntaxReader>();

    internal IEnumerable<CsonConverter> GetConvertersFromType(Type type)
    {
        foreach (var converter in Converters)
        {
            if (converter.CanConvert(type))
            {
                yield return converter;
            }
        }

        foreach (var defaultConverter in _defaultConverters)
        {
            if (defaultConverter.CanConvert(type))
            {
                yield return defaultConverter;
            }
        }

        foreach (var converterFactory in _converterFactories)
        {
            if (converterFactory.CanConvert(type))
            {
                var converter = converterFactory.CreateConverterCore(type);
                yield return converter;
            }
        }
    }
}
