using System.Reflection;

namespace BluePrint.core.Infrastructure
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ICustomAttributeProvider attributeProvider, bool inherit)
        {
            return attributeProvider.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>();
        }
    }
}
