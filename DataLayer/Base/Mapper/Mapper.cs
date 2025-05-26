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
				p.CanWrite);

			if (destProp != null)
			{
				var sourceValue = sourceProp.GetValue(source);

				if (sourceValue == null)
				{
					destProp.SetValue(destination, null);
					continue;
				}

				try
				{
					// اگر نوع پراپرتی primitive یا string بود، مستقیم مقداردهی کن
					if (destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
					{
						destProp.SetValue(destination, sourceValue);
					}
					// اگر نوع کلاس بود (یعنی تو در تو)، ریکرسیو مپ کن
					else if (IsComplexType(destProp.PropertyType) && IsComplexType(sourceProp.PropertyType))
					{
						var nestedMappedValue = typeof(Mapper)
							.GetMethod("Map")
							.MakeGenericMethod(sourceProp.PropertyType, destProp.PropertyType)
							.Invoke(null, new object[] { sourceValue });

						destProp.SetValue(destination, nestedMappedValue);
					}
				}
				catch
				{
					// TODO: log mapping error if needed
					continue;
				}
			}
		}

		return destination;
	}

	private static bool IsComplexType(Type type)
	{
		return type.IsClass && type != typeof(string);
	}
}