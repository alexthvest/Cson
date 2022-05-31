namespace Cson.Serialization.Converters;

internal class LiteralConverterFactory : CsonConverterFactory
{
    private static readonly Type[] LiteralTypes = new[]
    {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(bool),
        typeof(string)
    };

    public override bool CanConvert(Type typeToConvert)
    {
        return LiteralTypes.Contains(typeToConvert);
    }

    public override CsonConverter CreateConverter(Type typeToConvert)
    {
        var converterType = typeof(LiteralConverter<>).MakeGenericType(typeToConvert);
        return (CsonConverter)Activator.CreateInstance(converterType)!;
    }
}
