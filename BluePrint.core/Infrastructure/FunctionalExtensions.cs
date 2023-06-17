namespace BluePrint.core.Infrastructure
{
    public static class FunctionalExtensions
    {
        public static T Tee<T>(this T instance, Action<T> action)
        {
            action(instance);
            return instance;
        }

        public static TResult Tee<TInput, TResult>(this TInput instance, Func<TInput, TResult> func)
        {
            return func(instance);
        }

        public static T Execute<T>(this T instance, IStrategy<T> strategy)
        {
            return Tee(instance, strategy.Execute);
        }

        public static TResult Map<TInput, TResult>(this TInput input, Func<TInput, TResult> func)
        {
            return func(input);
        }
    }
}
