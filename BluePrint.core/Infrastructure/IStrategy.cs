namespace BluePrint.core.Infrastructure
{
    public interface IStrategy<in T>
    {
        /// <summary>
        /// Execute logic for strategy
        /// </summary>
        /// <param name="input"></param>
        void Execute(T input);
    }
}
