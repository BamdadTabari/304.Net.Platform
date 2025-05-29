using System.Reflection;

namespace DataLayer.Base.Mapper;

public static class Mapper
{
    public static TDestination Map<TSource, TDestination>(TSource source)
        where TDestination : class, new()
        where TSource : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Source object cannot be null.");

        var destination = new TDestination();

        var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destProps = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceProp in sourceProps)
        {
            if (!sourceProp.CanRead) continue;

            var destProp = destProps.FirstOrDefault(p =>
                p.Name == sourceProp.Name &&
                p.PropertyType.IsAssignableFrom(sourceProp.PropertyType) &&
                p.CanWrite);

            if (destProp != null)
            {
                try
                {
                    var value = sourceProp.GetValue(source);
                    destProp.SetValue(destination, value);
                }
                catch
                {
                    // todo: optional: log mapping error
                    continue;
                }
            }
        }

        return destination;
    }
}
