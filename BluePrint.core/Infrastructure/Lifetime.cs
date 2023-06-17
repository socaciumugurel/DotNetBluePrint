namespace BluePrint.core.Infrastructure
{
    public enum Lifetime
    {
        /// <summary>
        ///     New instances are created each time a new object graph is created.
        ///     During the scope of build-up of one object graph the created instances are reused.
        /// </summary>
        Scoped,

        /// <summary>
        ///     Always creates a new instance of this class when it is injected as a dependency.
        /// </summary>
        Transient,

        /// <summary>
        ///     Lives on the application as a singleton instance
        /// </summary>
        Singleton
    }
}
