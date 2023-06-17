namespace BluePrint.core.Infrastructure
{
    /// <summary>
    ///     Declares a service implementation, by decorating the class that implements it.
    ///     It may also specify the lifetime of the service instance by using the Lifetime enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute()
        {
        }

        public ServiceAttribute(Lifetime lifetime)
            : this(null, lifetime)
        {
        }

        public ServiceAttribute(Type exportType)
            : this(exportType, Lifetime.Scoped)
        {
        }

        public ServiceAttribute(Type? exportType, Lifetime lifetime)
        {
            ExportType = exportType;
            Lifetime = lifetime;
        }

        public Type? ExportType { get; }

        public Lifetime Lifetime { get; }
    }
}
