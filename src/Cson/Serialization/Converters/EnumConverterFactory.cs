namespace Cson.Serialization.Converters;

internal class EnumConverterFactory : CsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override CsonConverter CreateConverter(Type typeToConvert)
    {
        var converterType = typeof(EnumConverter<>).MakeGenericType(typeToConvert);
        return (CsonConverter)Activator.CreateInstance(converterType)!;
    }
}
