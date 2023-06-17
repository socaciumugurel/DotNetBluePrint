namespace BluePrint.core.Infrastructure
{
    public class ServiceInfo
    {
        public ServiceInfo(Type from, Type to, Lifetime lifetime)
        {
            From = from;
            To = to;
            InstanceLifetime = lifetime;
        }

        public Type From { get; }

        public Lifetime InstanceLifetime { get; }

        public Type To { get; }
    }
}
